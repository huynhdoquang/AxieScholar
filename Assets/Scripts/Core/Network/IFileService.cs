using Cysharp.Threading.Tasks;
using System;
using System.IO;
using Net.HungryBug.Core.Engine;
using UnityEngine;

namespace Net.HungryBug.Core.Network
{
    public class ConfigDownloadInfo<TResponse, TConfig> where TResponse : Response<TConfig>
    {
        /// <summary>
        /// Gets the downloaded config.
        /// </summary>
        public readonly TConfig Config;

        /// <summary>
        /// Gets the status of config downloading.
        /// </summary>
        public readonly bool HasChanged;

        /// <summary>
        /// Create new instance of <see cref="ConfigDownloadInfo{TResponse, TConfig}"/>
        /// </summary>
        public ConfigDownloadInfo(TConfig config, bool hasChanged)
        {
            this.Config = config;
            this.HasChanged = hasChanged;
        }
    }

    public interface IFileService
    {
        /// <summary>
        /// Download and return a config, load from cache or download new if not found or has changed, throws exception if can not fetch the lastest config.
        /// </summary>
        UniTask<TConfig> DownloadConfig<TResponse, TConfig>(IFileDownloader<TResponse, TConfig> downloader) where TResponse : Response<TConfig>;

        /// <summary>
        /// Download and return a config, load from cache or download new if not found or has changed, throws exception if can not fetch the lastest config..
        /// </summary>
        UniTask<ConfigDownloadInfo<TResponse, TConfig>> DownloadConfigDetail<TResponse, TConfig>(IFileDownloader<TResponse, TConfig> downloader) where TResponse : Response<TConfig>;

        /// <summary>
        /// Force download a config then save to cache for next time loading, throws exception if can not fetch the lastest config.
        /// </summary>
        UniTask<TConfig> ForceDownloadAndSave<TResponse, TConfig>(string path, IFileDownloader<TResponse, TConfig> downloader) where TResponse : Response<TConfig>;

    }
}
