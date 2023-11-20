using Verse;
using HarmonyLib;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Ghosts
{
    public class GhostsMod : Mod
    {
        public static GhostsMod mod;

        public GhostsMod(ModContentPack content) : base(content)
        {
            mod = this;
            var harmony = new Harmony("com.ghosts");

            harmony.Patch(original: AccessTools.PropertyGetter(typeof(ShaderTypeDef), nameof(ShaderTypeDef.Shader)),
                prefix: new HarmonyMethod(typeof(GhostsMod),
                nameof(ShaderFromAssetBundle)));

            harmony.PatchAll();
        }

        public static void ShaderFromAssetBundle(ShaderTypeDef __instance, ref Shader ___shaderInt)
        {
            if (__instance is GhostsShaderTypeDef)
            {
                ___shaderInt = GhostsContentDatabase.GhostsBundle.LoadAsset<Shader>(__instance.shaderPath);
                if (___shaderInt is null)
                {
                    Log.Message($"[<color=#4494E3FF>Ghosts</color>] <color=#e36c45FF>Failed to load Shader from path <text>\"{__instance.shaderPath}\"</text></color>");
                }
            }
        }

        public AssetBundle MainBundle
        {
            get
            {
                string text = "";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    text = "StandaloneOSX";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    text = "StandaloneWindows64";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    text = "StandaloneLinux64";
                }

                string bundlePath = Path.Combine(base.Content.RootDir, "Materials\\Bundles\\" + text + "\\ghostsbundle");
                AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);

                if (bundle == null)
                {
                    Log.Message("[<color=#4494E3FF>Ghosts</color>] <color=#e36c45FF>Failed to load bundle at path:</color> " + bundlePath);
                }

                return bundle;
            }
        }
    }
}
