using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WaterAssetRestorer.Patches
{
    public class WaterGet
    {
        /// <summary>
        /// Gets a material by name, optionally searching within a specific prefab.
        /// </summary>
        /// <param name="materialToFind">The name of the material to find.</param>
        /// <param name="prefabToSearch">The name of the prefab to search within (optional).</param>
        /// <returns>The found material, or null if not found.</returns>
        public static Material GET_material(string materialToFind, string prefabToSearch=null) 
        {
            
            Material materialToReturn = null;

            if (string.IsNullOrEmpty(materialToFind)) 
            {
                WaterAssetRestorer.Logger.LogWarning("Material to find is null.");
                return null; 
            }

            // if a prefabToSearch is provided, try to find the material in that prefab first
            if (!string.IsNullOrEmpty(prefabToSearch))
            {
                GameObject prefab = null;
                var objects = Resources.FindObjectsOfTypeAll<GameObject>();
                foreach (var obj in objects)
                {
                    if (obj.name == prefabToSearch)
                    {
                        prefab = obj;   
                    }
                }

                if (prefab == null)
                {
                    WaterAssetRestorer.Logger.LogWarning($"Prefab '{prefabToSearch}' not found.");
                    return null;
                }
                else
                {
                    WaterAssetRestorer.Logger.LogDebug($"Found prefab '{prefabToSearch}'.");
                    var renderers = prefab.GetComponentsInChildren<Renderer>(true);
                    foreach (var renderer in renderers)
                    {
                        if (renderer.sharedMaterial.name == materialToFind)
                        {
                            WaterAssetRestorer.Logger.LogDebug($"Found material '{materialToFind}' in prefab '{prefabToSearch}'.");
                            materialToReturn= renderer.sharedMaterial;
                        }
                    }
                }
            }

            // else we check the current scene
            else
            {
                var renderers = GameObject.FindObjectsOfType<Renderer>(true);
                foreach (var renderer in renderers)
                {
                    if (renderer.sharedMaterial!=null && renderer.sharedMaterial.name == materialToFind)
                    {
                        WaterAssetRestorer.Logger.LogDebug($"Found material '{materialToFind}' in scene.");
                        materialToReturn = renderer.sharedMaterial;
                    }
                }
            }

            WaterAssetRestorer.Logger.LogInfo($"Material '{materialToFind}' search completed. Found: {(materialToReturn != null ? "Yes" : "No")}");
            return materialToReturn;
        }

        /// <summary>
        /// used to recreate the standard lake water material based on values I found through UnityExplorer
        /// </summary>
        /// <returns>the water material or null if not found</returns>
        public static Material build_lake_water() 
        {
            Material mat = null;
            Shader shader = null;

            //if LethalLevelLoader is installed, use its reference of the water shader since it caches it early to avoid
            //accidentally using an empty shader named identical to the target shader from an AssetBundle
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("imabatby.lethallevelloader"))
            {
                WaterAssetRestorer.Logger.LogDebug("Using LLL's reference to water shader");
                shader = LethalLevelLoader.LevelLoader.vanillaWaterShader;
            }
            //if LLL didn't find it (either because it's not installed or failed for some reason), I look for it
            if(shader == null)
            {
                WaterAssetRestorer.Logger.LogDebug("Getting vanilla water shader");
                shader = Shader.Find("Shader Graphs/WaterShaderHDRP");
            }

            if (shader != null)
            {
                WaterAssetRestorer.Logger.LogDebug("Found WaterShaderHDRP shader.");
                mat=new Material(shader);

                mat.color=new Color(0.0f, 0.0f, 0.0f, 0.0f);
                mat.doubleSidedGI = false;
                mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                mat.EnableKeyword("_ENABLE_FOG_ON_TRANSPARENT");
                mat.EnableKeyword("_DISSABLE_SSR_TRANSPARENT");
                mat.enableInstancing = true;
                mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
                mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                mat.shaderKeywords = new string[] { "_DISSABLE_SSR_TRANSPARENT", "_ENABLE_FOG_ON_TRANSPARENT", "_SURFACE_TYPE_TRANSPARENT"};

            }
            else
            {
                WaterAssetRestorer.Logger.LogWarning("Could not find WaterShaderHDRP shader.");
            }
            return mat;
        }
    }
}
