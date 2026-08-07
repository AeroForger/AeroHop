using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;

namespace Engine;
    

class Menu
{
    public string StartMenu()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold green]AeroHop[/]")
                .PageSize(10)
                .AddChoices(new[] {
                    "Start AeroHop",
                    "Settings",
                    "Exit"
                })
        );

    }
    public List<string> SettingsMenu(bool jsonchecker, bool filechecker, bool commandchecker)
    {
        return AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("[bold yellow]Settings[/]")
                .PageSize(10)
                .AddChoices(new[] {
                    $"Toggle On/Off Json Checker",
                    $"Toggle On/Off File Checker",
                    $"Toggle On/Off Command Checker"
                    
                })
        );
    }
    public void DisplaySettings(bool jsonchecker, bool filechecker, bool commandchecker)
    {
        AnsiConsole.MarkupLine("[bold yellow]Current Settings:[/]");
        AnsiConsole.MarkupLine($"Json Checker: {(jsonchecker ? "[green]ON[/]" : "[red]OFF[/]")}");
        AnsiConsole.MarkupLine($"File Checker: {(filechecker ? "[green]ON[/]" : "[red]OFF[/]")}");
        AnsiConsole.MarkupLine($"Command Checker: {(commandchecker ? "[green]ON[/]" : "[red]OFF[/]")}");
    }
    public string MainMenu()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold green]AeroHop V1.0[/]")
                .PageSize(10)
                .AddChoices(new[] {
                    "Start Downloading",
                    "Profiles",
                    "Custom pkgs",
                    "See packages",
                    "Back to the Start Menu",
                    "Exit"
                })
        );
    }
    public List<string> ProfilesMenu()
    {
        return AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("[bold yellow]Profiles[/]")
                .PageSize(10)
                .AddChoices(new[] {
                    "Gaming",
                    "Dev",
                    "Custom",
                    "Basics"
                })
        );
    }
    public List<string> CustomPackagesMenu()
    {
        var input = AnsiConsole.Ask<string>(
            "[bold yellow]Enter custom packages (comma-separated):[/]"
        );

        var pkgs = input
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToList();
        if (pkgs.Count == 0)
        {
            AnsiConsole.MarkupLine("[bold red]No valid packages entered. Please try again.[/]");
            return new List<string>();
        }
        else
        {
            AnsiConsole.MarkupLine("[bold green]Custom packages added[/]");
            return pkgs;
        }

    }

    

}


