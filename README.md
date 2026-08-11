# AeroHop
A tool light as air that helps you hop between Linux distributions.

---

## Purpose
AeroHop simplifies distro hopping by automating the installation of your core software stack. It seamlessly manages packages defined in profile configs (`Basics.json`, `Dev.json`, `Gaming.json`) or your own personal `Custom.json`.

---

## features
AeroHop provides many useful features such as:
* User's custom pkgs - AeroHop automatically saves user inputted pkgs to Custom.json making your config reproducible
* Good looking ui - AeroHop uses [Spectre.Console](https://spectreconsole.net/) which provides great looks
* Toggle safety features - AeroHop has 3 main safety features that can be turned off/on in the start menu
* Profiles - AeroHop has 3 pre-made profiles : `Basics.json`, `Dev.json` and `Gaming.json` 
* lightweight

---

## How to install and run

1. Download the latest release from the [Releases page](../../releases).
2. Extract the archive and make the executable run:
 
```bash

tar -xzf AeroHop.tar.gz //or whatever is the name of current release

chmod +x AeroHop

./AeroHop or Sudo ./AeroHop

```

---

## Custom Packages Menu

To add Custom Packages to Custom.json you need to go to the Custom pkgs menu section and type your desired packages in the input field

Example:
```
code,dotnet-sdk,swayfx

```

---

### License
Apache V2.0 License
