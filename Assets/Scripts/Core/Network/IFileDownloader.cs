using Cysharp.Threading.Tasks;

namespace Net.HungryBug.Core.Network
{
    public interface IFileDownloader<TResponse, TConfig> where TResponse : Response<TConfig>
    {
        /// <summary>
        /// Gets the config name.
        /// </summary>
        string ConfigName { get; }

        /// <summary>
        /// Has change result code.
        /// </summary>
        int HasChangeCode { get; }

        /// <summary>
        /// No change result code.
        /// </summary>
        int NoChangeCode { get; }

        /// <summary>
        /// Force download new config.
        /// </summary>
        /// <returns></returns>
        UniTask<TResponse> ForceDownload();

        /// <summary>
        /// Call download with ref and return the <see cref="ConfigDownloadInfo{TResponse, TConfig}"/>
        /// </summary>
        /// <returns></returns>
        UniTask<TResponse> DownloadWithRef(string md5);

        /// <summary>
        /// Create a <see cref="TResponse"/> from <see cref="NetResponse"/>.
        /// </summary>
        TResponse CreateResponse(NetResponse netResponse);

        /// <summary>
        /// Save references.
        /// </summary>
        void SaveRef(TResponse response);
    }
}
