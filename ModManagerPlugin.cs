using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using PacosModLoader.interfaces;
using Panik;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PacosModLoader;

[BepInPlugin("aaaPacosModManager", "PacosModManager", "1.0.0.0")]
public class ModManagerPlugin : BaseUnityPlugin
{
    public static ModManagerPlugin Instance { get; private set; }
    internal static new ManualLogSource Log;
    private List<IModManagerMod> _registeredMods = [];
    private ConfigFile _modManagerGlobalConfig;
    private Harmony _harmony;

    void Awake()
    {
        Instance = this;
        Log = base.Logger;
        Log.LogInfo("Mod Manager loaded!");
        _modManagerGlobalConfig = new ConfigFile(Path.Combine(Paths.ConfigPath, "PacosModManager.cfg"), true);

        _harmony = new Harmony("aaaPacosModManager");

        _harmony.PatchAll(typeof(PacosModLoader.Patches.GeneralUiScriptStartPatch));
        Log.LogInfo("Mod Manager: Applied game patches.");

    }

    public void RegisterMod(IModManagerMod mod)
    {
        if (!_registeredMods.Contains(mod))
        {
            _registeredMods.Add(mod);
            Logger.LogInfo($"Mod '{mod.ModName}' registered with Mod Manager.");

            ConfigEntry<bool> modActiveEntry = _modManagerGlobalConfig.Bind("ModStates", mod.ModName, true, $"Is mod '{mod.ModName}' active?");
            mod.SetActive(modActiveEntry.Value);
        }
    }

    public void UnregisterMod(IModManagerMod mod)
    {
        if (_registeredMods.Contains(mod))
        {
            _registeredMods.Remove(mod);
            Logger.LogInfo($"Mod '{mod.ModName}' unregistered from Mod Manager.");
        }
    }

    public List<IModManagerMod> GetRegisteredMods()
    {
        return [.. _registeredMods];
    }

    public void SetModActive(IModManagerMod mod, bool isActive)
    {
        if (_registeredMods.Contains(mod))
        {
            mod.SetActive(isActive);
            Logger.LogInfo($"Mod '{mod.ModName}' active state set to {isActive}.");

            ConfigEntry<bool> modActiveEntry = _modManagerGlobalConfig.Bind("ModStates", mod.ModName, isActive, $"Is mod '{mod.ModName}' active?");
            modActiveEntry.Value = isActive;
            _modManagerGlobalConfig.Save();
        }
    }
}
