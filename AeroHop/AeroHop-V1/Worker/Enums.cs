using System;

namespace Worker;

public enum Profile
{
    Basics,
    Dev,
    Gaming,
    Custom
}
public enum Pm
{
    apt,      // Debian / Ubuntu
    dnf,      // Fedora / RHEL
    pacman,   // Arch
    zypper,   // openSUSE
    apk,      // Alpine
    yum,      // Older RHEL
    snap,     // Universal Ubuntu
    flatpak   // Universal Linux
}
