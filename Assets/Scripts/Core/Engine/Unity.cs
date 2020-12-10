using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Net.HungryBug.Core.Engine
{
    /// <summary>
    /// Unity3D engine services
    /// </summary>
    public interface IUnity
    {
        #region [System.IO]
        UniTask<byte[]> LoadFileBinary(string path);
        UniTask SaveFileBinary(string path, byte[] data);
        void DeleteFileBinary(string path);
        bool ExistsFile(string path);
        string GetFileName(string path);
        void CreateDirectory(string path);
        void DeleteDirectory(string path, bool recursive);
        bool ExistsDirectory(string path);
        string[] GetFilesInDirectory(string path);
        void MoveDirectory(string sourceDirName, string destDirName);
        #endregion

        #region [UnityEngine]
        UnityEngine.Object Resources_Load(string path, Type systemTypeInstance);
        UnityEngine.GameObject GameObject_Instantiate(UnityEngine.GameObject prefab, UnityEngine.Transform parent);
        UniTask<IUnityWebResponse> SendWebRequest(UnityWebRequest request);
        UniTask<bool> Download(string url, string savedPath, Dictionary<string, string> headers, Action<float> onProgress = null);
        void QuitApp();
        void OpenURL(string url);
        SystemLanguage GetSystemLanguage();
        #endregion

        #region [StreamingAsset]
        /// <summary>
        /// Load assetbundle from streaming asset folder.
        /// </summary>
        UniTask<AssetBundle> LoadBundleFromStreamingAsset(string streamingPath, string hash128);

        /// <summary>
        /// Load a binary file from streaming asset folder.
        /// </summary>
        UniTask<byte[]> LoadFileFromStreamingAsset(string filePath);
        
        /// <summary>
        /// Load assetbundle from file/
        /// </summary>
        UniTask<AssetBundle> LoadBundleFromFile(string path, string hash128);

        /// <summary>
        /// Post processing downloaded asset bundle file to optimize loading time for usage
        /// </summary>
        UniTask PostProcessAssetBundle(string bundleFilePath);
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class Unity : IUnity
    {
        #region [System.IO]
        public async UniTask<byte[]> LoadFileBinary(string path)
        {
            if (!File.Exists(path))
                return null;

            using (var sourceStream = File.Open(path, FileMode.Open))
            {
                var result = new byte[sourceStream.Length];
                await sourceStream.ReadAsync(result, 0, (int)sourceStream.Length);
                return result;
            }
        }

        public async UniTask SaveFileBinary(string path, byte[] data)
        {
            var directoryName = Path.GetDirectoryName(path);
            if (directoryName != null && !Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            using (var sourceStream = File.Open(path, FileMode.Create))
            {
                await sourceStream.WriteAsync(data, 0, data.Length);
            }

#if UNITY_IOS
            UnityEngine.iOS.Device.SetNoBackupFlag(path);
#endif
        }

        public void DeleteFileBinary(string path) { if (File.Exists(path)) File.Delete(path); }
        public bool ExistsFile(string path) { return File.Exists(path); }
        public string GetFileName(string path) { return Path.GetFileName(path); }

        public void CreateDirectory(string path) { Directory.CreateDirectory(path); }
        public void DeleteDirectory(string path, bool recursive) { Directory.Delete(path, recursive); }
        public bool ExistsDirectory(string path) { return Directory.Exists(path); }
        public string[] GetFilesInDirectory(string path) { return Directory.GetFiles(path); }
        public void MoveDirectory(string sourceDirName, string destDirName) { Directory.Move(sourceDirName, destDirName); }
#endregion

#region [UnityEngine]
        public UnityEngine.Object Resources_Load(string path, Type systemTypeInstance) { return UnityEngine.Resources.Load(path, systemTypeInstance); }
        public UnityEngine.GameObject GameObject_Instantiate(UnityEngine.GameObject prefab, UnityEngine.Transform parent) { return UnityEngine.GameObject.Instantiate(prefab, parent); }
        public void OpenURL(string url) { Application.OpenURL(url); }
        public SystemLanguage GetSystemLanguage() { return Application.systemLanguage; }

        public async UniTask<IUnityWebResponse> SendWebRequest(UnityWebRequest request)
        {
            await request.SendWebRequest();
            return new UnityWebResponse(request);
        }

        public async UniTask<bool> Download(string url, string savedPath, Dictionary<string, string> headers, Action<float> onProgress = null)
        {
            using (var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET))
            {
                request.downloadHandler = new DownloadHandlerFile(savedPath);

                if (headers != null && headers.Count > 0)
                {
                    foreach (var pair in headers)
                    {
                        request.SetRequestHeader(pair.Key, pair.Value);
                    }
                }
                var downloadTask = request.SendWebRequest();
                while (!downloadTask.isDone)
                {
                    onProgress?.Invoke(request.downloadProgress);
                    await UniTask.Yield();
                }
                onProgress?.Invoke(1.0f);
                if (!request.isNetworkError && !request.isHttpError)
                {
#if UNITY_IOS
                UnityEngine.iOS.Device.SetNoBackupFlag(savedPath);
#endif
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Quit the app.
        /// </summary>
        public void QuitApp()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;

#else
            Application.Quit();
#endif
        }
#endregion

#region [AssetBundle]
        /// <summary>
        /// Load assetbundle from streaming asset folder.
        /// </summary>
        public async UniTask<AssetBundle> LoadBundleFromStreamingAsset(string streamingPath, string hash128)
        {
            using (var request = UnityWebRequestAssetBundle.GetAssetBundle(streamingPath, Hash128.Parse(hash128)))
            {
                //var request = UnityWebRequestAssetBundle.GetAssetBundle(streamingPath);
                var res = await request.SendWebRequest();

                if (res.isNetworkError || res.isHttpError)
                {
                    throw new System.InvalidOperationException($"[AssetManager][LoadBundleFromStreamingAsset] no file on {streamingPath}.");
                }
                else
                {
                    return DownloadHandlerAssetBundle.GetContent(res);
                }
            }
        }

        /// <summary>
        /// Load a binary file from streaming asset folder.
        /// </summary>
        public async UniTask<byte[]> LoadFileFromStreamingAsset(string filePath)
        {
            using (var request = UnityWebRequest.Get(filePath))
            {
                var res = await request.SendWebRequest();

                if (res.isNetworkError || res.isHttpError)
                {
                    throw new System.InvalidOperationException($"[AssetManager][LoadFileFromStreamingAsset] no file on {filePath}.");
                }
                else
                {
                    return res.downloadHandler.data;
                }
            }
        }

        /// <summary>
        /// Load assetbundle from file/
        /// </summary>
        public async UniTask<AssetBundle> LoadBundleFromFile(string path, string hash128)
        {
            var fileUrl = path;
            if(!fileUrl.StartsWith("file://"))
            {
                fileUrl = $"file://{fileUrl}";
            }
            using (var request = UnityWebRequestAssetBundle.GetAssetBundle(fileUrl, Hash128.Parse(hash128)))
            {
                //var request = UnityWebRequestAssetBundle.GetAssetBundle(fileUrl);
                var res = await request.SendWebRequest();
                if (res.isNetworkError || res.isHttpError)
                {
                    throw new System.InvalidOperationException($"[AssetManager][LoadBundleFromFile] no file on {fileUrl}.");
                }
                else
                {
                    return DownloadHandlerAssetBundle.GetContent(res);
                }
            }
        }

        /// <summary>
        /// Post processing downloaded asset bundle file to optimize loading time for usage
        /// </summary>
        public async UniTask PostProcessAssetBundle(string bundleFilePath)
        {
            await AssetBundle.RecompressAssetBundleAsync(bundleFilePath, bundleFilePath, BuildCompression.LZ4Runtime);
        }
        #endregion
    }
}