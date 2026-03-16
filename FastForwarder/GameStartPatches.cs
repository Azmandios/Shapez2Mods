using Game.Core.Serialization;
using HarmonyLib;

namespace Shapez2Mods.FastForwarder.Patches
{
    [HarmonyPatch]
    public static class GameStartPatches
    {
        private static bool menuWasActive = true;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameStartOptionsContinueExisting), MethodType.Constructor,
            new System.Type[] { typeof(SavegameBlobReader), typeof(bool), typeof(string) })]
        private static void ContinueExisting_Postfix(GameStartOptionsContinueExisting __instance)
        {
            CheckMenuMode(__instance.MenuMode);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameStartOptionsStartNew), MethodType.Constructor,
            new System.Type[] { typeof(GameParameters), typeof(bool), typeof(string) })]
        private static void StartNew_Postfix(GameStartOptionsStartNew __instance)
        {
            CheckMenuMode(__instance.MenuMode);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameCore), "Init")]
        private static void GameCore_Init_Postfix(GameCore __instance)
        {
            if (menuWasActive)
            {
                FastForwarder.Log.LogInfo("[FastForwarder] Skipping GameCore.Init (still in menu).");
                return;
            }
            
            FastForwarder.Log.LogInfo("[FastForwarder] GameCore.Init() detected — setting up Fast Forwarder...");
            FastForwarder.TryInitialize(__instance);
        }

        private static void CheckMenuMode(bool menuMode)
        {
            if (menuMode && !menuWasActive)
            {
                menuWasActive = true;
            }
            else if (!menuMode && menuWasActive)
            {
                menuWasActive = false;
                FastForwarder.Log.LogInfo("[FastForwarder] Menu closed, preparing for game start...");
            }
        }
    }
}