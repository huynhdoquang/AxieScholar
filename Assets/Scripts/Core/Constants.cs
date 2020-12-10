using System.IO;
using UnityEngine;

namespace Net.HungryBug.Core
{
    public static class CorePaths
    {
        public static string Cache = Path.Combine(Application.persistentDataPath, "GLX_Cache");
        public static string FileService = Path.Combine(Cache, "FileService");
        public static string AssetBundle = Path.Combine(Cache, "AssetBundles");
        public static string AssetBundleV1 = Path.Combine(Cache, "AssetBundlesV1");
        public static string LocalDatabase = Path.Combine(Cache, "LocalDatabase");
        public static string Localization = Path.Combine(Cache, "Localization");
        public static string SavedGame = Path.Combine(Cache, "SavedGame");
        public static string SaveTeamPreset = Path.Combine(Cache, "TeamPreset");
        public static string SaveBattlePreset = Path.Combine(Cache, "BattlePreset");
        public static string StreamingAssetBundle = Path.Combine(Application.streamingAssetsPath, "AssetBundle");
    }
}
