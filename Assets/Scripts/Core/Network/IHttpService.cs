using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Net.HungryBug.Core.Network
{
    /// <summary>
    /// <see cref="IHttpService"/> will be run on default setting without request pool for the first time. 
    /// If you want to override the setting, please feel free to call <see cref="IHttpService.Initialize(int, int)"/>.
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// Gets the request count.
        /// </summary>
        int RequestCount { get; }

        /// <summary>
        /// Get or set max concurrent downloads
        /// </summary>
        int MaxConcurrentDownloads { get; set; }

        /// <summary>
        /// Download a bynary data.
        /// </summary>
        UniTask<bool> Download(string url, string savePath, Dictionary<string, string> header = null, Action<float> onProgress = null);

        /// <summary>
        /// 
        /// </summary>
        UniTask<NetResponse> Get(string url, Dictionary<string, string> header = null);

        /// <summary>
        /// 
        /// </summary>
        UniTask<NetResponse> Post(string url, byte[] base64Data, Dictionary<string, string> header = null);

        /// <summary>
        /// 
        /// </summary>
        UniTask<NetResponse> Put(string url, byte[] base64Data, Dictionary<string, string> header = null);

        /// <summary>
        /// 
        /// </summary>
        UniTask<NetResponse> Delete(string url, Dictionary<string, string> header = null);
    }
}
