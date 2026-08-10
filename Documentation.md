# AeroHop

## Code structure

**AeroHop is split into 3 sections**

| Section | Description |
| ----------- | ----------- |
| Engine/ | Engine contains **Core.cs** and **Menu.cs** and is responsible for the **flow** |
| Worker/ | Worker contains many files and **they are controlled by core.cs** | 
| Program.cs | Starts the app |

---

### Worker/

> Downloader -> Downlaods all the files

> storage.cs -> stores inputted data to Custom.json

> Validator.cs -> validates commands , files , json and returns either true or false

> Loader.cs -> loads json files and parses them

> Enums.cs -> contains enums for other files

### Engine

> Core.cs -> Controls the whole app sends data to each file

> Menu.cs -> is responsible for the menu it uses spectre console for the visuals it also returns the choice 
