using Discord;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


namespace WaterAssetRestorer.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    public class WaterInit
    {
        public static Material WAR_company_flooded = null;
        public static Material WAR_cave = null;
        public static Material WAR_pool = null;
        public static Material WAR_adamance_march_vow = null;

        //after StartOfRound, initialize the water materials
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void InitializeWaterMaterials()
        {
            WaterAssetRestorer.Logger.LogInfo("Initializing water materials...");
            WAR_company_flooded = WaterGet.GET_material("Water_mat_04");
            WAR_cave = WaterGet.GET_material("CaveWater", "CaveWaterTile");
            WAR_pool = WaterGet.GET_material("PoolWater", "PoolTile");
            //WAR_adamance_march_vow = WaterGet.GET_material("VowWater");

        }                
    }

    [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.FinishGeneratingNewLevelClientRpc))]
    public class WaterSwap
    {
        static bool skipHost = true;
        [HarmonyPostfix]
        public static void SwapWaterMaterials()
        {
            
            //only individual clients need to swap, the host does not (though they'd just run the swap twice without this which is harmless)
            if (skipHost)
            {
                WaterAssetRestorer.Logger.LogDebug("host, skipping water material swap.");
                skipHost = false;
                return;
            }
            WaterAssetRestorer.Logger.LogInfo("Swapping water materials...");
            WaterSet.SET_material("Water_mat_04", WaterInit.WAR_company_flooded);
            WaterSet.SET_material("CaveWater", WaterInit.WAR_cave);
            WaterSet.SET_material("PoolWater", WaterInit.WAR_pool);
            //WaterSet.SET_material("VowWater", WaterInit.WAR_adamance_march_vow);
        }
    }
}