using HarmonyLib;
using Panik;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace PacosModLoader.Patches
{
    [HarmonyPatch(typeof(GeneralUiScript), "Start")]
    public static class GeneralUiScriptStartPatch
    {
        public static void Postfix(GeneralUiScript __instance)
        {
            __instance.textGameVersion.text = (Master.IsDemo ? "Demo " : "") + "v" + Application.version + " (Modded)";
        }
    }
}
