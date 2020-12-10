#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Net.HungryBug.Core.Utility.Editor
{
    public static class CoreUtility
    {
        private static string _dataPath = null;
        private static string _shortDataPath = null;
        private static string DataPath => _dataPath = _dataPath ?? Path.GetFullPath(Application.dataPath);
        private static string ShortDataPath => _shortDataPath = _shortDataPath ?? DataPath.Remove(DataPath.Length - 6, 6);

        public static string ToIOPath(this string assetPath)
        {
            var p = Path.Combine(DataPath.Remove(DataPath.Length - 6, 6), assetPath);
            return Path.GetFullPath(p);
        }

        public static string ToAssetPath(this string ioPath)
        {
            var p = Path.GetFullPath(ioPath);
            var r = p.Remove(0, ShortDataPath.Length).Replace("\\", "/");
            return r;
        }

        /// <summary>
        /// Gets all Object inside directory (contain childs).
        /// </summary>
        public static void GetAllAssets<T>(string directory, string fileExtension, List<T> result) where T : UnityEngine.Object
        {
            var dir = new DirectoryInfo(directory);
            if (!dir.Exists)
                return;

            //gather files.
            var files = dir.GetFiles($"*.{fileExtension}");
            foreach (var f in files)
            {
                var assetPath = f.FullName.ToAssetPath();
                var r = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (r != null)
                {
                    result.Add(r);
                }
            }

            //loop for child dirs.
            var dirs = dir.GetDirectories();
            foreach (var d in dirs)
            {
                GetAllAssets<T>(d.FullName, fileExtension, result);
            }
        }

        /// <summary>
        /// Create asset at path.
        /// </summary>
        public static void CreateAsset<T>(string dirPath) where T : ScriptableObject
        {
            CreateAsset(dirPath, typeof(T));
        }

        /// <summary>
        /// Create asset at path.
        /// </summary>
        public static void CreateAsset(string dirPath, Type type)
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            var des = EditorUtility.SaveFilePanel(
                                                    string.Format("Create new {0}", type.Name),
                                                    dirPath,
                                                    string.Format("{0}.asset", type.Name),
                                                    "asset");
            if (!string.IsNullOrEmpty(des))
            {
                if (!des.Contains("/Assets"))
                {
                    Debug.LogError("Invalid path, not inside Assets/ folder.");
                }
                else if (!des.Contains(dirPath))
                {
                    Debug.LogErrorFormat("Invalid path, must inside {0}", dirPath);
                }
                else
                {
                    var asset = (UnityEngine.Object)Activator.CreateInstance(type);
                    var index = des.IndexOf(dirPath);
                    des = des.Substring(index);

                    AssetDatabase.CreateAsset(asset, des);
                    AssetDatabase.SaveAssets();
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = asset;
                }
            }
        }
    }
}
#endif
