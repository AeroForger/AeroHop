using System;
using System.IO;
using System.Text.Json;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Spectre.Console;

namespace Worker;
public class Loader
{
    private string filepath = Path.Combine(AppContext.BaseDirectory, "JSON");
    public List<string> LoadProfiles(List<string> profile)
    {
        // 1. Define the valid profile types we look for
        var validTargets = new List<string> { "Gaming", "Dev", "Custom", "Basics" };
        
        // Use a HashSet to automatically prevent duplicate packages across profiles
        var combinedPackages = new HashSet<string>();
        bool atLeastOneProfileFound = false;

        // Get the current system package manager once
        string PM = PackageManager(); 

        // 2. Loop through each string inside the user's input list
        foreach (string target in profile)
        {
            // Only process if it matches one of our known profiles
            if (validTargets.Contains(target))
            {
                string path = GetProfilePath(target);
                
                if (!File.Exists(path))
                {
                    AnsiConsole.MarkupLine($"[bold red]Profile '{target}' file not found.[/]");
                    continue; // Skip to the next profile in the list
                }

                atLeastOneProfileFound = true;
                string jsonString = File.ReadAllText(path);

                try
                {
                    // 3. Parse and extract packages for the active package manager
                    var dict = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(jsonString);
                    
                    if (dict != null && dict.TryGetValue(PM, out List<string>? profileData))
                    {
                        // Add all found packages into our combined set
                        foreach (var pkg in profileData)
                        {
                            combinedPackages.Add(pkg);
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[bold red]Package manager '{PM}' not found inside {target} profile.[/]");
                    }
                }
                catch (JsonException)
                {
                    AnsiConsole.MarkupLine($"[bold red]Error parsing JSON file for profile '{target}'.[/]");
                }
            }
        }

        // 4. Final safety checks and output
        if (!atLeastOneProfileFound)
        {
            AnsiConsole.MarkupLine($"[bold red]No valid profile types were found in the requested list.[/]");
            return new List<string>();
        }

        // Convert the unique set of packages back to a list
        return new List<string>(combinedPackages);
    }

    // Method to get the full path of a profile JSON file
    private string GetProfilePath(string profile)
    {
        return Path.Combine(filepath, $"{profile}.json");
    }
    public (string distro, string packageManager) SendData()
    {
        return (Distro(), PackageManager());
    }

    private string Distro()
    {
        if (File.Exists("/etc/os-release"))
        {
            var lines = File.ReadAllLines("/etc/os-release");
            foreach (var line in lines)
            {
                if (line.StartsWith("ID="))
                {
                    return line.Substring(3).Trim('"'); 
                }
            }
        }
        else
        {
            return "Unknown";
        }
        return "Unknown";
    }
    
    public string PackageManager()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return "Not Linux";
        }

        // Ordered by priority: native distros first, then universal fallbacks
        string[] packageManagers = { 
            "apt",      // Debian / Ubuntu
            "dnf",      // Fedora / RHEL
            "pacman",   // Arch
            "zypper",   // openSUSE
            "apk",      // Alpine
            "yum",      // Older RHEL
            "snap",     // Universal Ubuntu
            "flatpak"   // Universal Linux
        };

        // Split the system PATH into individual directory chunks
        string pathEnv = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
        string[] directories = pathEnv.Split(Path.PathSeparator);

        // Scan for the first matching executable found in the PATH
        foreach (var manager in packageManagers)
        {
            foreach (var dir in directories)
            {
                if (File.Exists(Path.Combine(dir, manager)))
                {
                    return manager; // Instantly returns e.g. "apt", "dnf", etc.
                }
            }
        }

        return "Unknown";    
    }
}