using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using Shapez2Mods.FastForwarder.UI;

namespace Shapez2Mods.FastForwarder
{
    [BepInPlugin("yujis.shapez2mods.fastforwarder", "Fast Forwarder", "1.0.5")]
    public class FastForwarder : BaseUnityPlugin
    {
        internal static FastForwarder Instance = null!;
        internal static GameCore CurrentGameCore = null!;
        internal static SimulationSpeedManager SimManager = null!;
        internal static ManualLogSource Log = null!;

        private void Awake()
        {
            Instance = this;
            Log = base.Logger;

            var harmony = new Harmony("yujis.shapez2mods.fastforwarder");
            harmony.PatchAll();
            Log.LogInfo("Fast Forwarder loaded.");
        }

        public static void TryInitialize(GameCore GC)
        {
            Log.LogInfo("[FastForwarder] TryInitialize called.");
            CurrentGameCore = GC;

            if (CurrentGameCore == null)
            {
                Log.LogWarning("[FastForwarder] GameCore is null.");
                return;
            }

            SimManager = CurrentGameCore.SimulationSpeed;
            if (SimManager == null)
            {
                Log.LogWarning("[FastForwarder] SimulationSpeedManager not found.");
                return;
            }

            Log.LogInfo("[FastForwarder] SimulationSpeedManager successfully hooked.");
            CreateUI();
            
            SetSpeed(1.0f);
        }

        public static void SetSpeed(float speed)
        {
            if (SimManager == null) return;

            if (speed == 0f)
            {
                SimManager.IsPaused = true;
            }
            else
            {
                SimManager.IsPaused = false;
            }
            
            SimManager.Speed = speed;
            Log.LogInfo($"[FastForwarder] Speed set to {speed}x");
        }

        private static void CreateUI()
        {
            string objName = "FastForwarder_UI_Root";
            var existingObj = GameObject.Find(objName);
            
            if (existingObj == null)
            {
                var newObj = new GameObject(objName);
                UnityEngine.Object.DontDestroyOnLoad(newObj);
                newObj.AddComponent<FastForwarderUI>();
                Log.LogInfo("[FastForwarder] FastForwarderUI created on new GameObject.");
            }
        }
    }
}