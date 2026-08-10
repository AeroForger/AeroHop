using System;
using System.Collections.Generic;
using Spectre.Console;
using Worker;

namespace Engine;

public class Core
{
    Menu menu = new();
    Validator validator = new();
    Loader loader = new();
    Store store = new();
    Downloader downloader = new();

    bool jsonchecker = true;
    bool filechecker = true;
    bool commandchecker = true;

    public List<string> Profiles = new List<string>();
    public (string Distro, string PM) SysData;

    public void Data()
    {
        SysData = loader.SendData();
    }

    public void Start()
    {
        
        AnsiConsole.MarkupLine("[bold green]AeroHop[/] [bold yellow]v1.0[/]");

        while (true)
        {
            var choice = menu.StartMenu();
            switch (choice)
            {
                case "Start AeroHop":
                    menu.DisplaySettings(jsonchecker, filechecker, commandchecker);
                    MainMenuLogic();
                    break;
                case "Settings":
                    var settingsChoice = menu.SettingsMenu(jsonchecker, filechecker, commandchecker);
                    foreach (var item in settingsChoice)
                    {
                        switch (item)
                        {
                            case "Toggle On/Off Json Checker":
                                jsonchecker = !jsonchecker;
                                break;
                            case "Toggle On/Off File Checker":
                                filechecker = !filechecker;
                                break;
                            case "Toggle On/Off Command Checker":
                                commandchecker = !commandchecker;
                                break;
                        }
                    }
                    break;
                case "Exit":
                    AnsiConsole.MarkupLine("[bold red]Exiting AeroHop...[/]");
                    Environment.Exit(0);
                    break;
                default:
                    AnsiConsole.MarkupLine("[bold red]Invalid choice.[/]"); //this is useless but who knows what will user do
                    break;
            }
        }
    }

    public void MainMenuLogic()
    {
        while (true)
        {
            // Changes SysData to include Distro + Package Manager
            Data();
            var choice = menu.MainMenu();
            switch (choice)
            {
                case "Start Downloading":
                {
                    AnsiConsole.MarkupLine("[bold green]Start Downloading[/]");
                    bool success = validator.Switches(jsonchecker, filechecker, commandchecker);
                    if (success)
                    {
                        if (!Profiles.Contains("Basics") && !Profiles.Contains("Custom") && !Profiles.Contains("Dev") && !Profiles.Contains("Gaming"))
                        {
                            Profiles.Add("Basics");
                        }
                        var pkgsData = loader.LoadProfiles(Profiles);
                        AnsiConsole.MarkupLine("[bold green]This may take a while[/]");
                        bool downloadSuccess = downloader.Download(SysData.PM, pkgsData);
                        if (!downloadSuccess)
                        {
                            AnsiConsole.MarkupLine("[bold red]Failed to download packages.[/]");
                        }
                        
                        AnsiConsole.MarkupLine("[bold green]Download process completed.[/]");
                    }
                    break;
                }
                case "Profiles":

                    AnsiConsole.MarkupLine("[bold yellow]Profiles Menu[/]");
                    Profiles.AddRange(menu.ProfilesMenu());
                    break;
                case "Custom pkgs":
                {
                    AnsiConsole.MarkupLine("[bold yellow]Custom Packages Menu[/]");
                    var customPkgs = menu.CustomPackagesMenu();
                    store.SaveCustomPackages(customPkgs);
                    break;
                }
                case "See packages":
                {
                    AnsiConsole.MarkupLine("[bold yellow]Packages Menu[/]");
                    if (!Profiles.Contains("Basics") && !Profiles.Contains("Custom") && !Profiles.Contains("Dev") && !Profiles.Contains("Gaming"))
                    {
                        AnsiConsole.MarkupLine("[bold blue] A profile isnt chosen[/]");
                    }
                    else
                    {
                        var pkgsData = loader.LoadProfiles(Profiles);
                        AnsiConsole.MarkupLine("[bold green]Packages loaded successfully. Here are the packages:[/]");
                        AnsiConsole.MarkupLine($"[bold red]Profiles toggled: {string.Join(", ", Profiles)}[/]");
                        if (pkgsData.Count == 0)
                        {
                            AnsiConsole.MarkupLine("[bold red]No packages found for the selected profiles.[/]");
                            return;
                        }
                        foreach (var pkg in pkgsData)
                        {
                            AnsiConsole.MarkupLine($"[blue]- {pkg}[/]");
                        }
                    }

                    break;
                
                }
                case "Exit":
                    AnsiConsole.MarkupLine("[bold red]Exiting AeroHop...[/]");
                    Environment.Exit(0);
                    break;
                case "Back to the Start Menu":
                    return;
                default:
                    AnsiConsole.MarkupLine("[bold red]Invalid choice.[/]"); //this useless but who knows
                    break;

            }
    
        }
    }

}
    
