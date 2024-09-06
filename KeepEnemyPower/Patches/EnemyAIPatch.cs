using HarmonyLib;
using UnityEngine;

namespace KeepEnemyPower.Patches
{
    [HarmonyPatch(typeof(EnemyAI))]
    internal class EnemyAIPatch
    {
        [HarmonyPatch("SubtractFromPowerLevel")]
        [HarmonyPrefix]
        static void CheckPreviousPower(EnemyAI __instance, ref bool ___removedPowerLevel)
        {
            if (!___removedPowerLevel)
            {
                if (__instance.enemyType.isDaytimeEnemy)
                {
                    Plugin.LogToConsole("Previous Daytime Enemy Power -> " + RoundManager.Instance.currentDaytimeEnemyPower);
                    return;
                }
                if (__instance.isOutside)
                {
                    Plugin.LogToConsole("Previous Outside Enemy Power -> " + RoundManager.Instance.currentOutsideEnemyPower);
                    return;
                }
                Plugin.LogToConsole("Previous Inside Enemy Power -> " + RoundManager.Instance.currentEnemyPower);
            }
        }

        [HarmonyPatch("SubtractFromPowerLevel")]
        [HarmonyPostfix]
        static void KeepPowerLevel(EnemyAI __instance, ref bool ___removedPowerLevel)
        {
            if (___removedPowerLevel)
            {
                if (__instance.enemyType.isDaytimeEnemy)
                {
                    if (Plugin.keepOutdoorsPower.Value)
                    {
                        ___removedPowerLevel = false;
                        RoundManager.Instance.currentDaytimeEnemyPower = Mathf.Max(RoundManager.Instance.currentDaytimeEnemyPower + __instance.enemyType.PowerLevel, 0);
                    }
                    Plugin.LogToConsole("Current Daytime Enemy Power -> " + RoundManager.Instance.currentDaytimeEnemyPower);
                    return;
                }
                if (__instance.isOutside)
                {
                    if (Plugin.keepOutdoorsPower.Value)
                    {
                        ___removedPowerLevel = false;
                        RoundManager.Instance.currentOutsideEnemyPower = Mathf.Max(RoundManager.Instance.currentOutsideEnemyPower + __instance.enemyType.PowerLevel, 0);
                    }
                    Plugin.LogToConsole("Current Outside Enemy Power -> " + RoundManager.Instance.currentOutsideEnemyPower);
                    return;
                }
                if (Plugin.keepIndoorsPower.Value)
                {
                    ___removedPowerLevel = false;
                    // RoundManager.Instance.cannotSpawnMoreInsideEnemies = true;
                    RoundManager.Instance.currentEnemyPower = Mathf.Max(RoundManager.Instance.currentEnemyPower + __instance.enemyType.PowerLevel, 0);
                }
                Plugin.LogToConsole("Current Inside Enemy Power -> " + RoundManager.Instance.currentEnemyPower);
            }
        }
    }
}
