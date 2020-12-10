using System.Net;
using UnityEngine.Networking;

namespace Net.HungryBug.Core.Engine
{
    public interface IUnityWebResponse
    {
        /// <summary>
        /// Gets the <see cref="UnityWebRequest.responseCode"/> as <see cref="HttpStatusCode"/>.
        /// </summary>
        HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the <see cref="DownloadHandler.data"/>.
        /// </summary>
        byte[] Data { get; }

        /// <summary>
        /// Gets the <see cref="DownloadHandler.text"/>.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets the <see cref="UnityWebRequest.isNetworkError"/>.
        /// </summary>
        bool IsNetworkError { get; }

        /// <summary>
        /// Gets the <see cref="UnityWebRequest.error"/>.
        /// </summary>
        string Error { get; }
    }

    public class UnityWebResponse : IUnityWebResponse
    {
        public HttpStatusCode StatusCode { get; }

        public byte[] Data { get; }

        public string Text { get; }

        public bool IsNetworkError { get; }

        public string Error { get; }

        public UnityWebRequest Request { get; }

        public UnityWebResponse(UnityWebRequest request)
        {
            this.Request = request;
            this.StatusCode = (HttpStatusCode)request.responseCode;
            this.Data = request.downloadHandler.data;
            this.Text = request.downloadHandler.text;
            this.IsNetworkError = request.isNetworkError;
            this.Error = request.error;
        }
    }
}
