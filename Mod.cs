using CitiesHarmony.API;
using ICities;
using UnityEngine;

namespace ThirtyFifteenCurb {
    public class Mod: LoadingExtensionBase, IUserMod {
        public string Name { get; } = "ThirtyFiftyCurb";
        public string Description { get; } = "Attempts to find all -30cm roads and lifts them to -15cm";

        public void OnEnabled() {
            Debug.Log("[30-15Curb] Enabled");
            Debug.Log("[30-15Curb] Checking if Harmony available");
            HarmonyHelper.EnsureHarmonyInstalled();
        }

        public void OnDisabled() {
            Patcher.UnpatchAll();
            Debug.Log("[30-15Curb] Disabled");
        }

        public override void OnCreated(ILoading loading) {
            Debug.Log($"[30-15Curb] OnCreated {loading.currentMode}");
            if (HarmonyHelper.IsHarmonyInstalled && loading.currentMode == AppMode.Game) {
                Patcher.PatchAll();
            }
        }

        public override void OnReleased() {
            Debug.Log("[30-15Curb] Released!");
            if (HarmonyHelper.IsHarmonyInstalled) {
                Patcher.UnpatchAll();
            }
        }
    }
}