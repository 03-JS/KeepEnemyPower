using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using KeepEnemyPower.Patches;

namespace KeepEnemyPower
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string modGUID = "JS03.KeepEnemyPower";
        private const string modName = "Keep Enemy Power";
        private const string modVersion = "1.0.81";

        // Config values
        public static ConfigEntry<bool> keepIndoorsPower;
        public static ConfigEntry<bool> keepOutdoorsPower;

        private readonly Harmony harmony = new Harmony(modGUID);
        private static Plugin Instance;
        internal static ManualLogSource mls;

        void Awake()
        {
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("Keep Enemy Power active");

            // EnemyAIPatch.AssignValues();

            GenerateConfigValues();
            harmony.PatchAll(typeof(EnemyAIPatch));
        }

        internal void GenerateConfigValues()
        {
            keepIndoorsPower = Config.Bind(
                "General", // Config section
                "Keep Indoors Power", // Key of this config
                true, // Default value
                "Keeps the indoor enemy power when an indoor enemy dies" // Description
            );

            keepOutdoorsPower = Config.Bind(
                "General", // Config section
                "Keep Outdoors Power", // Key of this config
                true, // Default value
                "Keeps the outdoor enemy power when an outdoor enemy dies" // Description
            );
        }
    }
}
