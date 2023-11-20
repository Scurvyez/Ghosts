using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Verse;

namespace Ghosts
{
    [StaticConstructorOnStartup]
    public static class GhostsContentDatabase
    {
        private static AssetBundle bundleInt;
        private static Dictionary<string, Shader> lookupShaders;
        private static Dictionary<string, Material> lookupMaterials;
        public static readonly Shader GhostEffect = LoadShader(Path.Combine("Assets", "GhostEffect.shader"));

        public static AssetBundle GhostsBundle
        {
            get
            {
                if (bundleInt == null)
                {
                    bundleInt = GhostsMod.mod.MainBundle;
                }
                return bundleInt;
            }
        }

        public static Shader LoadShader(string shaderName)
        {
            if (lookupShaders == null)
            {
                lookupShaders = new Dictionary<string, Shader>();
            }
            if (!lookupShaders.ContainsKey(shaderName))
            {
                lookupShaders[shaderName] = GhostsBundle.LoadAsset<Shader>(shaderName);
            }
            Shader shader = lookupShaders[shaderName];
            if (shader == null)
            {
                Log.Warning("[<color=#4494E3FF>Ghosts</color>] <color=#e36c45FF>Could not load shader:</color> " + shaderName);
                return ShaderDatabase.DefaultShader;
            }
            return shader;
        }

        public static Material LoadMaterial(string materialName)
        {
            if (lookupMaterials == null)
            {
                lookupMaterials = new Dictionary<string, Material>();
            }
            if (!lookupMaterials.ContainsKey(materialName))
            {
                lookupMaterials[materialName] = GhostsBundle.LoadAsset<Material>(materialName);
            }
            Material material = lookupMaterials[materialName];
            if (material == null)
            {
                Log.Warning("[<color=#4494E3FF>Ghosts</color>] <color=#e36c45FF>Could not load material:</color> " + materialName);
                return BaseContent.BadMat;
            }
            return material;
        }
    }
}
