//using System;
//using System.IO;
//using UnityEditor;
//using UnityEngine;

//namespace Net.HungryBug.Core.Utility.Editor
//{
//    public static class EditorExtension
//    {
//        /// <summary>
//        /// Create config
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        public static void CreateAsset<T>(string dirPath) where T : ScriptableObject
//        {
//            if (!Directory.Exists(dirPath))
//                Directory.CreateDirectory(dirPath);

//            var des = EditorUtility.SaveFilePanel(
//                                                    string.Format("Create new {0}", typeof(T).Name),
//                                                    dirPath,
//                                                    string.Format("{0}.asset", typeof(T).Name),
//                                                    "asset");
//            if (!string.IsNullOrEmpty(des))
//            {
//                if (!des.Contains("/Assets"))
//                {
//                    Debug.LogError("Invalid path, not inside Assets/ folder.");
//                }
//                else if (!des.Contains(dirPath))
//                {
//                    Debug.LogErrorFormat("Invalid path, must inside {0}", dirPath);
//                }
//                else
//                {
//                    var asset = (T)Activator.CreateInstance(typeof(T));
//                    var index = des.IndexOf(dirPath);
//                    des = des.Substring(index);

//                    AssetDatabase.CreateAsset(asset, des);
//                    AssetDatabase.SaveAssets();
//                    EditorUtility.FocusProjectWindow();
//                    Selection.activeObject = asset;
//                }
//            }
//        }
//    }
//}