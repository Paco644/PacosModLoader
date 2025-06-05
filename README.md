# Pacos Mod Manager
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A plugin for the game **Clover Pit (Demo)** using **BepInEx**.

The Pacos Mod Manager is a lightweight BepInEx plugin that helps you load and disable other mods, manage their configurations, and shows an in-game "MODDED" text when active.


---

## Installation

1.  **Install BepInEx:** Follow the official [BepInEx 5.x installation guide](https://docs.bepinex.dev/articles/user_guide/installation/index.html).
2.  **Download Pacos Mod Manager:** Get `PacosModManager.dll` from the [Releases](https://github.com/Paco644/PacosModLoader/releases) section.
3.  **Place the DLL:** Put `PacosModManager.dll` into your game's `BepInEx/plugins` folder.
4.  **Run the Game:** Launch CloverPit once to generate the necessary config files.

---


## For Developers: Integrate Your Mod

To make your BepInEx mod compatible with Pacos Mod Manager, follow these steps:

### 1. Reference the Manager

Add a reference to **`PacosModLoader.dll`** in your mod project.

### 2. Implement `IModManagerMod`

Your main plugin class must implement **`PacosModLoader.interfaces.IModManagerMod`**.

* **Properties:** Implement `ModName`, `ModVersion`, `ModAuthor`, `ModDescription`, and `IsActive`.
* **`SetActive(bool isActive)`:** This method will be called by the Mod Manager to enable or disable your mod. Add logic here to turn your mod's features on or off.

### 3. Bind Configs & Register Your Mod

In your plugin's `Awake()` method:
* **Bind your `BepInEx.Configuration.ConfigEntry` objects** as usual.
* **Register your mod** with the manager:
    ```csharp
    if (PacosModLoader.ModManagerPlugin.Instance != null)
    {
        PacosModLoader.ModManagerPlugin.Instance.RegisterMod(this);
    }
    ```

### 4. Run Plugin Code Conditionally

In your `Update()` (or other relevant game logic) methods, **only execute your mod's core functionality if `IsActive` is `true`**:
```csharp
if (IsActive)
{
    // Your mod's main code here
}
```

### 5. Unregister on Destory

In your `OnDestroy()` method, unregister your mod:

```c#
PacosModLoader.ModManagerPlugin.Instance?.UnregisterMod(this);
```

### Configuration
After running the game with your mod and the manager, find config files in `BepInEx/config/`:
- **Enable/Disable Mods:** `PacosModManager.cfg` and change `[ModStates]` entries.
- **Adjust Mod Values:** Open your mod's specific config file to change its settings.

For now mods only update / start if you restart the game.


### Examples
For a selection of examples please visit the [Examples](https://github.com/Paco644/PacosModLoader/tree/master/Examples)

### Planned Features:
* **In-game Mod Menu:** Disable, enable, and configure mods directly in-game, without needing a game restart.
* **Mod Profiles / Loadouts:** Create and switch between different sets of enabled mods and their saved configurations.
* **Mod Update Notifications & Management:** Get notified when updates are available for your installed mods, with potential for one-click updates and installing of dependencies (integrating with mod repositories).
* **Enhanced In-Game Mod Details:** The in-game menu will display more information like mod author, full description, and version.
