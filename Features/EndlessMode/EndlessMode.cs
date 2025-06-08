using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using PacosModLoader.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacosModLoader.Features.EndlessMode
{
    public class EndlessModeInfo
    {
        public const string PLUGIN_GUID = "EndlessMode";
        public const string PLUGIN_NAME = "EndlessMode";
        public const string PLUGIN_VERSION = "1.0";
    }

    [BepInPlugin(EndlessModeInfo.PLUGIN_GUID, EndlessModeInfo.PLUGIN_NAME, EndlessModeInfo.PLUGIN_VERSION)]
    public class EndlessMode : BaseUnityPlugin, IModManagerMod
    {
        public string ModName => EndlessModeInfo.PLUGIN_NAME;
        public string ModVersion => EndlessModeInfo.PLUGIN_VERSION;
        public string ModAuthor => "Paco";
        public string ModDescription => "Delets the limit of rounds of the game";

        public bool IsActive { get; private set; }
        public static EndlessMode Instance { get; private set; }

        public ManualLogSource Log;

        private Harmony _harmony;

        public void SetActive(bool isActive)
        {
            if (IsActive == isActive) return;

            if (isActive)
            {
                _harmony.PatchAll(typeof(Patches.EndlessModePatch.GameplayDataConsctructorPatch));
                _harmony.PatchAll(typeof(Patches.EndlessModePatch.GetRewardBoxDebtIndexPatch));
            } else
            {
                _harmony.UnpatchSelf();
            }
        }

        public void Awake()
        {
            Log = base.Logger;
            EndlessMode.Instance = this;
            _harmony = new Harmony(EndlessModeInfo.PLUGIN_GUID);
            ModManagerPlugin.Instance?.RegisterMod(this);
        }
    }
}
