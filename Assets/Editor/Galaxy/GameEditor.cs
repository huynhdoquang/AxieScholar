using Net.HungryBug.Core;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Net.HungryBug.Galaxy.Game.Editor
{
    public class OpenSceneEditor
    {
        private static string workingScene = null;

        [MenuItem("Galaxy/Play", false, 0)]
        public static void Play()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                workingScene = EditorSceneManager.GetActiveScene().path;
                EditorSceneManager.OpenScene($"Assets/Scenes/{SceneNames.S000_Boot}.unity");
                EditorApplication.isPlaying = true;
            }
        }

        [MenuItem("Galaxy/Scenes/Demo")]
        public static void S000_Boot()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene($"Assets/Scenes/{SceneNames.S_Demo}.unity");
            }
        }
    }

    public static class LocalStoreEditor
    {
        [UnityEditor.MenuItem("Galaxy/Local data/Clear all")]
        public static void ResetLocalStore()
        {
            //reset player prefs.
            PlayerPrefs.DeleteAll();

            //delete file service folder.
            if (System.IO.Directory.Exists(CorePaths.FileService))
            {
                Directory.Delete(CorePaths.FileService, true);
                Directory.CreateDirectory(CorePaths.FileService);
            }

            //delete local store.
            if (Directory.Exists(CorePaths.LocalDatabase))
            {
                Directory.Delete(CorePaths.LocalDatabase, true);
                Directory.CreateDirectory(CorePaths.LocalDatabase);
            }

            //delete bundle.
            if (Directory.Exists(CorePaths.AssetBundle))
            {
                Directory.Delete(CorePaths.AssetBundle, true);
                Directory.CreateDirectory(CorePaths.AssetBundle);
            }

            //delete bundle.
            if (Directory.Exists(CorePaths.AssetBundleV1))
            {
                Directory.Delete(CorePaths.AssetBundleV1, true);
                Directory.CreateDirectory(CorePaths.AssetBundleV1);
            }

            //delete localization
            if (Directory.Exists(CorePaths.Localization))
            {
                Directory.Delete(CorePaths.Localization, true);
                Directory.CreateDirectory(CorePaths.Localization);
            }
        }

        [UnityEditor.MenuItem("Galaxy/Local data/Clear Player Prefs")]
        public static void ResetPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Galaxy/Local data/Open")]
        public static void OpenCache()
        {
            EditorUtility.OpenFilePanel("Galaxy Cache Folder", CorePaths.Cache, string.Empty);
        }
    }
}
