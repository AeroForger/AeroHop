# AeroHop
A tool light as air that helps you hop between Linux distributions.

---

## Purpose
AeroHop's main purpose is to make Distro hopping much easier as it can automatically download packages that are in `Basics.json`, `Dev.json`, `Gaming.json` or `Custom.json`

---

## features
AeroHop provides many useful features such as:
* User's custom pkgs - AeroHop automatically saves user inputted pkgs to Custom.json making your config reproducible
* Good looking ui - AeroHop uses spectre.console which provides great looks
* Toggle safety features - AeroHop has 3 main safety features that can be turned off/on in the start menu
* lightweight

---

## How to install and run

*download the latest release and run these commands*
 
```bash
tar -xzf AeroHop-v1.0-linux-x64.tar.gz

cd AeroHop-v1.0-linux-x64

chmod +x AeroHop

./AeroHop
```

## Code structure

AeroHop is separated into different components to keep the code organized and easier to maintain.


```
  AeroHop/
    ├── Engine/
    │   ├── Core.cs
    │   └── Menu.cs
    ├── Worker/
    │   ├── Validator.cs
    │   ├── Downloader.cs
    │   ├── Storage.cs
    │   └── Loader.cs
    ├── JSON/
    │   ├── Gaming.json
    │   ├── Dev.json
    │   ├── Basics.json
    │   └── Custom.json
    ├── Program.cs
    └── AeroHop.csproj
```
`You can use` [tree nathanfriend](https://tree.nathanfriend.com) `to create this type of path visualization`

### License
Apache V2.0 License
