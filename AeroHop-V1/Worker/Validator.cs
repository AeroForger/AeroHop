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
        return true;
        //this method is useless for now as i couldn't find an useful use for it it will get an update in the future.
    }
    public bool ValidateCommand()
    {
        try
        {
            string PM = loader.PackageManager();
            var process = Process.Start(
                $"{PM} --version"
            );
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                AnsiConsole.MarkupLine("[bold red]Error occurred while validating Package Manager.[/]");
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
