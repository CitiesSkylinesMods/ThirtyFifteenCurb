using HarmonyLib;

namespace ThirtyFifteenCurb {
    public static class Patcher {
        private const string HarmonyId = "krzychu1245.ThirtyFiftyCurb";

        public static void PatchAll() {
            if (patched) return;

            patched = true;
            var harmony = new Harmony(HarmonyId);
            harmony.Patch(
                typeof(NetManager.Data).GetMethod(nameof(NetManager.Data.AfterDeserialize)),
                transpiler: new HarmonyMethod(typeof(CurbPatch), nameof(CurbPatch.Transpiler)));
        }

        private static bool patched = false;

        public static void UnpatchAll() {
            if (!patched) return;

            var harmony = new Harmony(HarmonyId);
            harmony.UnpatchAll(HarmonyId);
            patched = false;
        }
    }
}