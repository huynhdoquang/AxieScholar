using Cysharp.Threading.Tasks;
using System;
using System.IO;
using Net.HungryBug.Core.Engine;
using UnityEngine;
using Net.HungryBug.Core.Security;

namespace Net.HungryBug.Core.Network
{
    public class FileService : IFileService
    {
        private readonly IUnity engine;
        private readonly IRSAService rsaService;
        private readonly IChecksum checksum;

        public FileService(IUnity engine, IRSAService rSAService, IChecksum checksum)
        {
            this.engine = engine;
            this.rsaService = rSAService;
            this.checksum = checksum;
        }

        /// <summary>
        /// Download and return a config, load from cache or download new if not found or has changed.
        /// </summary>
        public async UniTask<TConfig> DownloadConfig<TResponse, TConfig>(IFileDownloader<TResponse, TConfig> downloader) where TResponse : Response<TConfig>
        {
            var detail = await this.DownloadConfigDetail(downloader);
            return detail.Config;
        }

        /// <summary>
        /// Download and return a config, load from cache or download new if not found or has changed.
        /// </summary>
        public async UniTask<ConfigDownloadInfo<TResponse, TConfig>> DownloadConfigDetail<TResponse, TConfig>(IFileDownloader<TResponse, TConfig> downloader) where TResponse : Response<TConfig>
        {
            var path = Path.Combine(CorePaths.FileService, $"{downloader.ConfigName}.dat");
            var fileInfo = new FileInfo(path);

            //check if need to refresh or making a new download.
            if (!fileInfo.Exists)
            {
                Debug.Log($"[FileService] {downloader.ConfigName} not found on local, download new");
                var res = await this.ForceDownloadAndSave(path, downloader);
                return new ConfigDownloadInfo<TResponse, TConfig>(res, true);
            }
            else
            {
                //check file md5 and compare with saved md5.
                await UniTask.SwitchToThreadPool();
                var cacheData = await this.engine.LoadFileBinary(path);
                cacheData = this.rsaService.DecryptBytes(cacheData);
                string md5 = this.checksum.HashMd5Data(cacheData);
                await UniTask.Yield();

                //then using the ref to download.
                var res = await downloader.DownloadWithRef(md5);
                if (res.Code != downloader.NoChangeCode)
                {
                    Debug.Log($"[FileService] {downloader.ConfigName} has changed, save new");
                    await UniTask.SwitchToThreadPool();
                    var bytes = this.rsaService.EncryptBytes(res.ObjectRawData);
                    await this.engine.SaveFileBinary(path, bytes);
                    await UniTask.Yield();

                    downloader.SaveRef(res);
                    return new ConfigDownloadInfo<TResponse, TConfig>(res.Object, true);
                }
                else if (res.Code == downloader.NoChangeCode)
                {
                    try
                    {
                        Debug.Log($"[FileService] {downloader.ConfigName} has no update, load local");
                        var net = new NetResponse(0, "OK", cacheData, 0, string.Empty);
                        var cache = downloader.CreateResponse(net);
                        return new ConfigDownloadInfo<TResponse, TConfig>(cache.Object, false);
                    }

                    //retry download if can not load saved config.
                    catch (Exception e)
                    {
                        Debug.Log($"[FileService] {downloader.ConfigName} failed to load local, try to redownload");
                        var resRetry = await this.ForceDownloadAndSave(path, downloader);
                        return new ConfigDownloadInfo<TResponse, TConfig>(resRetry, true);
                    }
                }
                else
                {
                    throw new System.Exception($"[FileService] Failed to download {downloader.ConfigName}, code = {res.Code}");
                }
            }
        }

        /// <summary>
        /// Force download a config then save to cache for next time loading.
        /// </summary>
        public async UniTask<TConfig> ForceDownloadAndSave<TResponse, TConfig>(string path, IFileDownloader<TResponse, TConfig> downloader) where TResponse : Response<TConfig>
        {
            var res = await downloader.ForceDownload();
            if (res.Code == 0)
            {
                await UniTask.SwitchToThreadPool();
                var bytes = this.rsaService.EncryptBytes(res.ObjectRawData);
                await this.engine.SaveFileBinary(path, bytes);
                await UniTask.Yield();

                downloader.SaveRef(res);
                return res.Object;

            }
            else
            {
                throw new System.Exception($"[FileService] Failed to download {downloader.ConfigName}, code = {res.Code}");
            }
        }
    }
}
