using HarmonyLib;
using Panik;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace PacosModLoader.Features.EndlessMode.Patches
{
   public class EndlessModePatch
    {
        [HarmonyPatch(typeof(GameplayData), MethodType.Constructor)]
        public static class GameplayDataConsctructorPatch
        {
            public static void Postfix(GameplayData __instance)
            {
                FieldInfo roundOfDeadlineField = AccessTools.Field(typeof(GameplayData), "roundOfDeadline");
                roundOfDeadlineField.SetValue(__instance, int.MaxValue);
                int patchedValue = (int)roundOfDeadlineField.GetValue(__instance);
                EndlessMode.Instance.Log.LogInfo($"GameplayData roundOfDeadline patched to: {patchedValue}");
   
            }
        }

        [HarmonyPatch(typeof(GameplayData), "GetRewardBoxDebtIndex")]
        public static class GetRewardBoxDebtIndexPatch
        {
            public static bool Prefix(ref BigInteger __result)
            {
                __result = new BigInteger(int.MaxValue);
                return false;
            }
        }

    }
}
