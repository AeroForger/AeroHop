using System;
using Spectre.Console;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;


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
            var outputLines = new List<string>();
            var errorLines = new List<string>();
            bool useSudo = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && !IsRootUser();

            process.StartInfo = new ProcessStartInfo
            {
                FileName = useSudo ? "sudo" : command,
                Arguments = useSudo ? $"{command} {args} {string.Join(" ", pkgs)}" : $"{args} {string.Join(" ", pkgs)}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            process.OutputDataReceived += (s, e) => { if (e.Data != null) outputLines.Add(e.Data); };
            process.ErrorDataReceived += (s, e) => { if (e.Data != null) errorLines.Add(e.Data); };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // Wait with timeout (10 minutes)
            const int timeoutMs = 600_000;
            if (!process.WaitForExit(timeoutMs))
            {
                try { process.Kill(); } catch { }
                AnsiConsole.MarkupLine("[bold red]Download process timed out and was terminated.[/]");
                return false;
            }

            // Ensure asynchronous handlers have finished
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                AnsiConsole.MarkupLine($"[bold red]Package manager exited with code {process.ExitCode}.[/]");
                if (errorLines.Count > 0)
                {
                    AnsiConsole.MarkupLine("[bold yellow]Error output:[/]");
                    foreach (var line in errorLines)
                    {
                        AnsiConsole.MarkupLine($"[red]{line}[/]");
                    }
                }
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

    private bool IsRootUser()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return false;
        }

        try
        {
            return Environment.UserName == "root";
        }
        catch
        {
            return false;
        }
    }
}
