using BepInEx;
using BepInEx.Configuration;
using PacosModLoader; // Required to access PacosModLoader.ModManagerPlugin
using PacosModLoader.interfaces; // Required for the IModManagerMod interface
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteResourcesMod;

public class MyPluginInfo
{
    public const string PLUGIN_GUID = "InfiniteResourcesMod";
    public const string PLUGIN_NAME = "InfiniteResourcesMod";
    public const string PLUGIN_VERSION = "1.0";
}

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
// This line declares a hard dependency on PacosModManager, ensuring it loads first.
[BepInDependency("aaaPacosModManager", BepInDependency.DependencyFlags.HardDependency)]
// Your plugin class must implement IModManagerMod to work with the Mod Manager.
public class InfiniteResourcesMod : BaseUnityPlugin, IModManagerMod
{
    public static InfiniteResourcesMod Instance { get; private set; }

    // --- IModManagerMod REQUIRED PROPERTIES ---
    // These properties provide metadata about your mod to the Mod Manager.
    public string ModName => MyPluginInfo.PLUGIN_NAME;
    public string ModVersion => MyPluginInfo.PLUGIN_VERSION;
    public string ModAuthor => "Paco";
    public string ModDescription => "Provides configurable amounts of coins and clovers in the game.";
    public bool IsActive { get; private set; } // Mod Manager controls this state.
    // --- END IModManagerMod REQUIRED PROPERTIES ---

    private ConfigEntry<string> _numberOfCoins;
    private ConfigEntry<string> _numberOfClovers;
    private ConfigEntry<bool> _disableCoins;
    private ConfigEntry<bool> _disableClovers;

    private int _currentCoinAmount;
    private int _currentCloverAmount;

    // --- IModManagerMod REQUIRED METHOD: SetActive ---
    // This method is called by PacosModManager to toggle your mod's active state.
    public void SetActive(bool isActive)
    {
        if (IsActive == isActive) return;
        IsActive = isActive;
        UpdateModState(); // Call helper to apply/remove effects based on state.
    }

    private void UpdateModState()
    {
        if (!IsActive)
        {
            // Add logic here to explicitly disable any active mod effects if !IsActive.
            Logger.LogInfo($"{ModName} has been deactivated.");
            return;
        }

        ParseAndSetCoinAmount(_numberOfCoins.Value);
        ParseAndSetCloverAmount(_numberOfClovers.Value);
        Logger.LogInfo($"{ModName} is now ACTIVE. Coins: {_currentCoinAmount} (Disabled: {_disableCoins.Value}), Clovers: {_currentCloverAmount} (Disabled: {_disableClovers.Value})");
    }

    void Awake()
    {
        Instance = this;

        // Bind your BepInEx ConfigEntries as usual.
        _numberOfCoins = Config.Bind("Settings", "Coins", "99999", "A fixed value of coins.");
        _numberOfClovers = Config.Bind("Settings", "Clovers", "99999", "A fixed value of clovers.");
        _disableCoins = Config.Bind("Settings", "DisableCoins", true, "If true, the mod will not set the player's coins.");
        _disableClovers = Config.Bind("Settings", "DisableClovers", false, "If true, the mod will not set the player's clovers.");

        ParseAndSetCoinAmount(_numberOfCoins.Value);
        ParseAndSetCloverAmount(_numberOfClovers.Value);

        // --- REGISTERING WITH PACOSMODMANAGER ---
        // Register your mod instance with the PacosModManager.
        if (PacosModLoader.ModManagerPlugin.Instance != null)
        {
            PacosModLoader.ModManagerPlugin.Instance.RegisterMod(this);
        }
        else
        {
            Logger.LogError($"{ModName}: ModManagerPlugin.Instance is null. Running in standalone mode.");
            IsActive = true; // Fallback to active if manager is missing.
            UpdateModState();
        }
    }

    void Update()
    {
        // --- CRITICAL: Only run your mod's main logic if it is active. ---
        if (IsActive)
        {
            if (GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.preparation)
            {
                if (!_disableCoins.Value && _currentCoinAmount >= 0)
                {
                    GameplayData.CoinsSet(_currentCoinAmount);
                }
                if (!_disableClovers.Value && _currentCloverAmount >= 0)
                {
                    GameplayData.CloverTicketsSet(_currentCloverAmount);
                }
            }
        }
    }

    private void ParseAndSetCoinAmount(string stringValue)
    {
        if (int.TryParse(stringValue, out int parsedAmount)) { _currentCoinAmount = parsedAmount; }
        else { Logger.LogWarning($"{ModName}: Invalid coin amount '{stringValue}'. Defaulting to 0."); _currentCoinAmount = 0; }
    }

    private void ParseAndSetCloverAmount(string stringValue)
    {
        if (int.TryParse(stringValue, out int parsedAmount)) { _currentCloverAmount = parsedAmount; }
        else { Logger.LogWarning($"{ModName}: Invalid clover amount '{stringValue}'. Defaulting to 0."); _currentCloverAmount = 0; }
    }

    void OnDestroy()
    {
        // Unregister your mod when it's unloaded for clean shutdown.
        PacosModLoader.ModManagerPlugin.Instance?.UnregisterMod(this);
    }
}