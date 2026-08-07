using System;
using Spectre.Console;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace Worker;

    
class Downloader
{
    private (string command, string args) CommandArgs(string pm)
    {
        string command = "";
        string args = "";
        switch (pm)
        {
            case "apt":
                command = "apt";
                args = "install -y";
                break;
            case "dnf":
                command = "dnf";
                args = "install -y";
                break;
            case "pacman":
                command = "pacman";
                args = "-Syu --noconfirm";
                break;
            case "zypper":
                command = "zypper";
                args = "install -y";
                break;
            case "yum":
                command = "yum";
                args = "install -y";
                break;
            case "apk":
                command = "apk";
                args = "add";
                break;
            case "snap":
                command = "snap";
                args = "install";
                break;
            case "flatpak":
                command = "flatpak";
                args = "install flathub";
                // Flatpak packages use application IDs instead of normal package names.
                // And because flatpak uses IDs, it can be tricky to use Custom + AnyProfile at the same time.
                // So i let users install flatpak packages manually. 
                break;
            default:
                AnsiConsole.MarkupLine("[bold red]Unsupported package manager.[/]");
                break;
        }
        return (command, args);
    }
    public bool Download(string pm, List<string> pkgs)
    {
        if (pkgs.Count == 0)
        {
            return false;
        }
        try
        {
            
            (string command, string args) = CommandArgs(pm);
            if (string.IsNullOrEmpty(command) || string.IsNullOrEmpty(args))
            {
                return false;
            }
            
            Process process = new Process();
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = $"{args} {string.Join(" ", pkgs)}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                return false;
            }
            
            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Error occurred while downloading packages: {ex.Message}[/]");
            return false;
        }
    }

}