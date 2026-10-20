using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using HarmonyLib;

namespace NoAshSpill
{
    [BepInPlugin(Guid, Name, Version)]
    public sealed class Plugin : BaseUnityPlugin
    {
        public const string Guid = "obenseuer.absurdmane.noashspill";
        public const string Name = "No Ash Spill";
        public const string Version = "1.0.0";

        internal static ManualLogSource Log;
        private static Harmony harmony;

        internal static ConfigEntry<bool> Enabled;

        private void Awake()
        {
            Log = Logger;

            Enabled = Config.Bind("Mod", "Enabled", true,
                "Off = the mod does nothing at all: overflowing ash drops on the ground as in vanilla. Works without a restart.");

            harmony = new Harmony(Guid);
            harmony.PatchAll(typeof(Plugin).Assembly);
            Log.LogInfo($"{Name} {Version} loaded");
        }

        /// <summary>Undoes everything for an unload. The patches are the only thing the mod installs.</summary>
        private void OnDestroy()
        {
            if (harmony == null)
                return;
            harmony.UnpatchSelf();
            harmony = null;
        }

        internal static bool On => Enabled.Value;
    }
}
