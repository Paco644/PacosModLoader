using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using PacosModLoader.Features.EndlessMode.Patches;
using PacosModLoader.interfaces;
using PacosModLoader.Patches;
using Panik;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PacosModLoader;

[BepInPlugin("aaaPacosModManager", "PacosModManager", "1.0.0.0")]
public class ModManagerPlugin : BaseUnityPlugin
{
    public static ModManagerPlugin Instance { get; private set; }
    internal static ManualLogSource Log;
    private readonly List<IModManagerMod> _registeredMods = [];
    private ConfigFile _modManagerGlobalConfig;
    private ConfigEntry<bool> _enableEndlessMode;
    private Harmony _harmony;

    public ConfigFile GetGlobalConfigFile()
    {
        return _modManagerGlobalConfig;
    }

    void Awake()
    {
        Instance = this;
        Log = base.Logger;
        Log.LogInfo("Mod Manager loaded!");
        _modManagerGlobalConfig = new ConfigFile(Path.Combine(Paths.ConfigPath, "PacosModManager.cfg"), true);

        DoGamePatches();
    }

    private void DoGamePatches()
    {
        _harmony = new Harmony("aaaPacosModManager");
        _harmony.PatchAll(typeof(GeneralUiScriptStartPatch));
        Log.LogInfo("Game patches applied successfully.");
    }

    public void RegisterMod(IModManagerMod mod)
    {
        if (!_registeredMods.Contains(mod))
        {
            _registeredMods.Add(mod);
            Logger.LogInfo($"Mod '{mod.ModName}' registered with Mod Manager.");

            ConfigEntry<bool> modActiveEntry = _modManagerGlobalConfig.Bind("ModStates", mod.ModName, true, $"Is mod '{mod.ModName}' active?");
            mod.SetActive(modActiveEntry.Value);

            modActiveEntry.SettingChanged += (sender, args) =>
            {
                mod.SetActive(modActiveEntry.Value);
                Logger.LogInfo($"Mod '{mod.ModName}' active state updated to {modActiveEntry.Value} from config change.");
            };
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

}
