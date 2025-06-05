using BepInEx;
using BepInEx.Configuration;
using PacosModLoader.interfaces;

namespace PacosModLoader.Examples;

public class HelloWorldModInfo
{
    public const string PLUGIN_GUID = "InfiniteResourcesMod";
    public const string PLUGIN_NAME = "InfiniteResourcesMod";
    public const string PLUGIN_VERSION = "1.0";
}

[BepInPlugin(HelloWorldModInfo.PLUGIN_GUID, HelloWorldModInfo.PLUGIN_NAME, HelloWorldModInfo.PLUGIN_VERSION)]
[BepInDependency("aaaPacosModManager", BepInDependency.DependencyFlags.HardDependency)]
public class HelloWorldMod : BaseUnityPlugin, IModManagerMod
{
    public string ModName => HelloWorldModInfo.PLUGIN_NAME;
    public string ModVersion => HelloWorldModInfo.PLUGIN_VERSION;
    public string ModAuthor => "Pascal Scharnetzki";
    public string ModDescription => "An example mod for the CloverPit mod menu, featuring a configurable greeting.";
    public bool IsActive { get; private set; }

    private ConfigEntry<string> _helloMessage;

    public void SetActive(bool isActive)
    {
        if (IsActive == isActive) return;
        IsActive = isActive;             
        UpdateModState();
    }

    private void UpdateModState()
    {
        if (IsActive)
        {
            Logger.LogInfo(_helloMessage.Value);
            // Add your actual mod logic here.
            // For example, enable hooks, patch methods, start coroutines, etc.
        }
        else
        {
            // Add logic to disable your mod's features here.
            // (e.g., remove hooks, revert patches, clean up spawned objects)
        }
    }

    void Awake()
    {
        _helloMessage = Config.Bind(
            "General",             
            "GreetingMessage",        
            "Does this shit work?",    
            "The greeting message displayed in the console when the mod is active."
        );

        if (ModManagerPlugin.Instance != null)
        {
            ModManagerPlugin.Instance.RegisterMod(this);
        }
        else
        {
            Logger.LogError("ModManagerPlugin.Instance is null. Cannot register with Mod Manager. " +
                            "Ensure ModManagerPlugin is loaded before this mod.");

            IsActive = true;
            UpdateModState(); 
        }
    }

    void OnDestroy()
    {
        ModManagerPlugin.Instance?.UnregisterMod(this);
    }
}