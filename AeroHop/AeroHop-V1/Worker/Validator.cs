using System;
using Spectre.Console;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Diagnostics;

namespace Worker;

class Validator
{
    private string filepath = Path.Combine(AppContext.BaseDirectory, "JSON");
    Loader loader = new Loader();
    public bool Switches(bool Json, bool File, bool Command)
    {
        bool jsonValid = true;
        bool fileValid = true;
        bool commandValid = true;
        if (Json)
        {
            jsonValid = ValidateJson();
        }
        if (File)
        {
            fileValid = ValidateFile();
        }
        if (Command)
        {
            commandValid = ValidateCommand();
        }
        if (!Json && !File && !Command)
        {
            AnsiConsole.MarkupLine("[bold red]No Safety switches on. Skipping validation. This is not recommended.[/]");
            return true;
        }

        if (!jsonValid || !fileValid || !commandValid)
        {
            AnsiConsole.MarkupLine("[bold red]One or more validations failed. Please check the above messages for details.[/]");
            return false;
        }
        return true;
    }
    private string GetProfilePath(string profile)
    {
        return Path.Combine(filepath, $"{profile}.json");
    }
    public bool ValidateJson()
    {
        // Checks if all JSON files exist
        if (File.Exists(GetProfilePath("Basics")))
        {
           if (File.Exists(GetProfilePath("Dev")))
            {
                if (File.Exists(GetProfilePath("Gaming")))
                {
                    return true;
                }
  
            }
        }
        AnsiConsole.MarkupLine("[bold red]One or more JSON files are missing. Please ensure all required profiles are present.[/]");
        return false;
    }
    public bool ValidateFile()
    {
        // Validate that each profile JSON is parseable and has the expected structure
        Profile[] profiles = new[] { Profile.Basics, Profile.Dev, Profile.Gaming };
        bool allValid = true;

        foreach (var profile in profiles)
        {
            string path = GetProfilePath(profile.ToString());
            if (!File.Exists(path))
            {
                AnsiConsole.MarkupLine($"[bold red]Profile file missing: {profile}.json[/]");
                allValid = false;
                continue;
            }

            try
            {
                string json = File.ReadAllText(path);
                var dict = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
                if (dict == null || dict.Count == 0)
                {
                    AnsiConsole.MarkupLine($"[bold red]{profile}.json contains no package entries or could not be parsed.[/]");
                    allValid = false;
                    continue;
                }

                foreach (var kv in dict)
                {
                    if (string.IsNullOrWhiteSpace(kv.Key))
                    {
                        AnsiConsole.MarkupLine($"[bold red]{profile}.json has an empty package-manager key.[/]");
                        allValid = false;
                    }
                    if (kv.Value == null || kv.Value.Count == 0)
                    {
                        AnsiConsole.MarkupLine($"[bold red]{profile}.json: package list for '{kv.Key}' is empty.[/]");
                        allValid = false;
                    }
                    else
                    {
                        foreach (var pkg in kv.Value)
                        {
                            if (string.IsNullOrWhiteSpace(pkg))
                            {
                                AnsiConsole.MarkupLine($"[bold red]{profile}.json contains an empty package name under '{kv.Key}'.[/]");
                                allValid = false;
                                break;
                            }
                        }
                    }
                }
            }
            catch (JsonException)
            {
                AnsiConsole.MarkupLine($"[bold red]Invalid JSON in {profile}.json.[/]");
                allValid = false;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]Error reading {profile}.json: {ex.Message}[/]");
                allValid = false;
            }
        }

        return allValid;
        //this method will validate JSON structure and package lists
    }
    public bool ValidateCommand()
    {
        try
        {
            string PM = loader.PackageManager();
           
            if (string.IsNullOrEmpty(PM) || PM == "Unknown" || PM == "Not Linux")
            {
                AnsiConsole.MarkupLine($"[bold red]Package manager not detected (returned '{PM}'). Skipping command validation.[/]");
                return false;
            }

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = PM,
                Arguments = "--version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                return false;
            }
            return true;

        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Exception occurred while validating Package Manager: {ex.Message}[/]");
            return false;
        }
    }
    
}
