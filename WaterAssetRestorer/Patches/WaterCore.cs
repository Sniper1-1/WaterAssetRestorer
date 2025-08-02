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
            //WAR_adamance_march_vow = WaterGet.build_lake_water();
            WAR_adamance_march_vow = search("VowWater");

        }

        public static Material search(string name) 
        {
            Material foundMat = null;
            Material[] allMaterials = Resources.LoadAll<Material>("");

            foreach (var mat in allMaterials)
            {
                Debug.Log($"Found material: {mat.name} | Shader: {mat.shader.name}");
                if (mat.name == name) // or whatever you're looking for
                {
                    foundMat = mat;
                    Debug.Log("Found the material: " + mat.name);
                    break;
                }
            }
            return foundMat;
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

        [HarmonyPostfix]
        public static void WaterPropertiesDebug(RoundManager __instance) 
        { 
            WaterAssetRestorer.Logger.LogDebug($"Current level is: {__instance.currentLevel.name}");
            if (__instance.currentLevel.name== "VowLevel")
            {
                Material mat = null;
                mat=GameObject.Find("WaterBig").GetComponent<Renderer>().sharedMaterial;

                LogMaterialProperties(mat);

            }
        }
        public static void LogMaterialProperties(Material mat) 
        {
            if (mat != null)
            {
                Shader shader = mat.shader;
                int count = shader.GetPropertyCount();
                Debug.Log($"--- Dumping Material: {mat.name} ---");

                for (int i = 0; i < count; i++)
                {
                    string propName = shader.GetPropertyName(i);
                    ShaderPropertyType type = shader.GetPropertyType(i);

                    switch (type)
                    {
                        case ShaderPropertyType.Color:
                            Debug.Log($"Color: {propName} = {mat.GetColor(propName)}");
                            break;

                        case ShaderPropertyType.Vector:
                            Debug.Log($"Vector: {propName} = {mat.GetVector(propName)}");
                            break;

                        case ShaderPropertyType.Float:
                        case ShaderPropertyType.Range:
                            Debug.Log($"Float: {propName} = {mat.GetFloat(propName)}");
                            break;

                        case ShaderPropertyType.Texture:
                            Debug.Log($"Texture: {propName} = {mat.GetTexture(propName)}");
                            break;

                        default:
                            Debug.Log($"Unknown: {propName}");
                            break;
                    }
                }

                Debug.Log($"Shader Keywords: {string.Join(", ", mat.shaderKeywords)}");
                Debug.Log("--- End Dump ---");
            }
            else
            {
                WaterAssetRestorer.Logger.LogWarning("Water material not found.");
            }
        }

    }
}
