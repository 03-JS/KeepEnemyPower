using HarmonyLib;
using System.Net;

namespace KeepEnemyPower.Patches
{
    [HarmonyPatch(typeof(EnemyAI))]
    internal class EnemyAIPatch
    {
        private static AccessTools.FieldRef<RoundManager, int> oldDaytimePowerLevel;
        private static AccessTools.FieldRef<RoundManager, int> oldOutsidePowerLevel;
        private static AccessTools.FieldRef<RoundManager, int> oldInsidePowerLevel;
        private static AccessTools.FieldRef<EnemyType, int> oldEnemyPowerLevel;

        private static AccessTools.FieldRef<RoundManager, float> newDaytimePowerLevel;
        private static AccessTools.FieldRef<RoundManager, float> newOutsidePowerLevel;
        private static AccessTools.FieldRef<RoundManager, float> newInsidePowerLevel;
        private static AccessTools.FieldRef<EnemyType, float> newEnemyPowerLevel;

        public static void AssignValues()
        {
            Plugin.LogToConsole("Values are being assigned!!!");

            // Daytime power level
            var daytimePowerLevelField = AccessTools.Field(typeof(RoundManager), nameof(RoundManager.Instance.currentDaytimeEnemyPower));
            if (daytimePowerLevelField.FieldType == typeof(int))
            {
                oldDaytimePowerLevel = AccessTools.FieldRefAccess<RoundManager, int>(daytimePowerLevelField);
            }
            if (daytimePowerLevelField.FieldType == typeof(float))
            {
                newDaytimePowerLevel = AccessTools.FieldRefAccess<RoundManager, float>(daytimePowerLevelField);
            }

            // Outside power level
            var outsidePowerLevelField = AccessTools.Field(typeof(RoundManager), nameof(RoundManager.Instance.currentOutsideEnemyPower));
            if (outsidePowerLevelField.FieldType == typeof(int))
            {
                oldOutsidePowerLevel = AccessTools.FieldRefAccess<RoundManager, int>(outsidePowerLevelField);
            }
            if (outsidePowerLevelField.FieldType == typeof(float))
            {
                newOutsidePowerLevel = AccessTools.FieldRefAccess<RoundManager, float>(outsidePowerLevelField);
            }

            // Inside power level
            var insidePowerLevelField = AccessTools.Field(typeof(RoundManager), nameof(RoundManager.Instance.currentEnemyPower));
            if (insidePowerLevelField.FieldType == typeof(int))
            {
                oldInsidePowerLevel = AccessTools.FieldRefAccess<RoundManager, int>(insidePowerLevelField);
            }
            if (insidePowerLevelField.FieldType == typeof(float))
            {
                newInsidePowerLevel = AccessTools.FieldRefAccess<RoundManager, float>(insidePowerLevelField);
            }

            // Enemy power level
            var enemyPowerLevelField = AccessTools.Field(typeof(EnemyType), nameof(EnemyType.PowerLevel));
            if (enemyPowerLevelField.FieldType == typeof(int))
            {
                oldEnemyPowerLevel = AccessTools.FieldRefAccess<EnemyType, int>(enemyPowerLevelField);
            }
            if (enemyPowerLevelField.FieldType == typeof(float))
            {
                newEnemyPowerLevel = AccessTools.FieldRefAccess<EnemyType, float>(enemyPowerLevelField);
            }
        }

        [HarmonyPatch("SubtractFromPowerLevel")]
        [HarmonyPrefix]
        static void CheckPreviousPower(EnemyAI __instance, ref bool ___removedPowerLevel)
        {
            if (!___removedPowerLevel)
            {
                if (__instance.enemyType.isDaytimeEnemy)
                {
                    if (oldDaytimePowerLevel != null)
                    {
                        Plugin.LogToConsole("Previous Daytime Enemy Power -> " + oldDaytimePowerLevel(RoundManager.Instance));
                        return;
                    }
                    if (newDaytimePowerLevel != null)
                    {
                        Plugin.LogToConsole("Previous Daytime Enemy Power -> " + newDaytimePowerLevel(RoundManager.Instance));
                        return;
                    }
                }
                if (__instance.isOutside)
                {
                    if (oldOutsidePowerLevel != null)
                    {
                        Plugin.LogToConsole("Previous Outside Enemy Power -> " + oldOutsidePowerLevel(RoundManager.Instance));
                        return;
                    }
                    if (newOutsidePowerLevel != null)
                    {
                        Plugin.LogToConsole("Previous Outside Enemy Power -> " + newOutsidePowerLevel(RoundManager.Instance));
                        return;
                    }
                }
                if (oldInsidePowerLevel != null)
                {
                    Plugin.LogToConsole("Previous Inside Enemy Power -> " + oldInsidePowerLevel(RoundManager.Instance));
                }
                if (newInsidePowerLevel != null)
                {
                    Plugin.LogToConsole("Previous Inside Enemy Power -> " + newInsidePowerLevel(RoundManager.Instance)); 
                }
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
                        if (oldDaytimePowerLevel != null)
                        {
                            oldDaytimePowerLevel(RoundManager.Instance) = oldDaytimePowerLevel(RoundManager.Instance) + oldEnemyPowerLevel(__instance.enemyType);
                        }
                        else if (newDaytimePowerLevel != null)
                        {
                            newDaytimePowerLevel(RoundManager.Instance) = newDaytimePowerLevel(RoundManager.Instance) + newEnemyPowerLevel(__instance.enemyType);
                        }
                    }
                    if (oldDaytimePowerLevel != null)
                    {
                        Plugin.LogToConsole("Current Daytime Enemy Power -> " + oldDaytimePowerLevel(RoundManager.Instance));
                        return;
                    }
                    if (newDaytimePowerLevel != null)
                    {
                        Plugin.LogToConsole("Current Daytime Enemy Power -> " + newDaytimePowerLevel(RoundManager.Instance));
                        return;
                    }
                }
                if (__instance.isOutside)
                {
                    if (Plugin.keepOutdoorsPower.Value)
                    {
                        ___removedPowerLevel = false;
                        if (oldOutsidePowerLevel != null)
                        {
                            oldOutsidePowerLevel(RoundManager.Instance) = oldOutsidePowerLevel(RoundManager.Instance) + oldEnemyPowerLevel(__instance.enemyType);
                        }
                        else if (newOutsidePowerLevel != null)
                        {
                            newOutsidePowerLevel(RoundManager.Instance) = newOutsidePowerLevel(RoundManager.Instance) + newEnemyPowerLevel(__instance.enemyType);
                        }
                    }
                    if (oldOutsidePowerLevel != null)
                    {
                        Plugin.LogToConsole("Current Outside Enemy Power -> " + oldOutsidePowerLevel(RoundManager.Instance));
                        return;
                    }
                    if (newOutsidePowerLevel != null)
                    {
                        Plugin.LogToConsole("Current Outside Enemy Power -> " + newOutsidePowerLevel(RoundManager.Instance));
                        return;
                    }
                }
                if (Plugin.keepIndoorsPower.Value)
                {
                    ___removedPowerLevel = false;
                    if (oldInsidePowerLevel != null)
                    {
                        oldInsidePowerLevel(RoundManager.Instance) = oldInsidePowerLevel(RoundManager.Instance) + oldEnemyPowerLevel(__instance.enemyType);
                    }
                    else if (newInsidePowerLevel != null)
                    {
                        newInsidePowerLevel(RoundManager.Instance) = newInsidePowerLevel(RoundManager.Instance) + newEnemyPowerLevel(__instance.enemyType);
                    }
                }
                if (oldInsidePowerLevel != null)
                {
                    Plugin.LogToConsole("Current Inside Enemy Power -> " + oldInsidePowerLevel(RoundManager.Instance));
                    return;
                }
                if (newInsidePowerLevel != null)
                {
                    Plugin.LogToConsole("Current Inside Enemy Power -> " + newInsidePowerLevel(RoundManager.Instance)); 
                }
            }
        }
    }
}
