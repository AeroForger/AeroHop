using System;
using System.Collections.Generic;
using Spectre.Console;
using System.IO;
using System.Text.Json;

namespace Worker;

public class Store
{
    public void SaveCustomPackages(List<string> pkgs)
    {
        // remove duplicates
        var uniquePkgs = new HashSet<string>(pkgs);
        // adds custom pkgs and clones them to all package managers
        var data = new Dictionary<string, List<string>>
        {
            { "apt", new List<string>(uniquePkgs) },
            { "dnf", new List<string>(uniquePkgs) },
            { "pacman", new List<string>(uniquePkgs) },
            { "zypper", new List<string>(uniquePkgs) },
            { "apk", new List<string>(uniquePkgs) },
            { "yum", new List<string>(uniquePkgs) },
            { "snap", new List<string>(uniquePkgs) },
            { "flatpak", new List<string>(uniquePkgs) }
        };

        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        string directory = Path.Combine(AppContext.BaseDirectory, "JSON");

        Directory.CreateDirectory(directory);

        string path = Path.Combine(directory, "Custom.json");

        File.WriteAllText(path, json);

        AnsiConsole.MarkupLine("[bold green]Custom packages saved successfully.[/]");
    }
}
