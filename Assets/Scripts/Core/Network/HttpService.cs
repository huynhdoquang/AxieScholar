using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using System.Net;
using Net.HungryBug.Core.Engine;
using Net.HungryBug.Core.Network.Data;
using UnityEngine;
using System;

namespace Net.HungryBug.Core.Network
{
    public class HttpService : IHttpService, IService
    {
        private const int requestTimeout = 15;
        public int MaxConcurrentDownloads { get; set; }
        public int RequestCount { get; private set; }

        private readonly IUnity engine;
        private readonly ITime time;

        private readonly Queue<DownloadTask> downloadTasks;
        private int downloading;

        private readonly Dictionary<string, string> defaultHeaders;

        /// <summary>
        /// Construct the <see cref="HttpService"/>.
        /// </summary>
        public HttpService(IUnity wrapper, ITime time)
        {
            this.engine = wrapper;
            this.time = time;
            this.downloadTasks = new Queue<DownloadTask>();
            this.defaultHeaders = new Dictionary<string, string>();
            this.MaxConcurrentDownloads = 4;
        }

        #region [IService]
        public bool IsRunning { get; private set; }
        public void Start() {
            this.IsRunning = true;
            ProcessDownloadTask().WrapErrors();
        }
        public void OnDestroy()
        {
            this.IsRunning = false;
        }
        public void OnAppPause() { }
        public void OnAppResume() { }
        public void Update(float timestamp) { }

        #endregion

        #region [IHttpService]
        /// <summary>
        /// Make a get request with custom header.
        /// </summary>
        public async UniTask<NetResponse> Get(string url, Dictionary<string, string> headers = null)
        {
            //Debug.LogWarning($"GET {url}");
            // Setup the request
            this.RequestCount++;
            using (var request = UnityWebRequest.Get(url))
            {
                request.method = UnityWebRequest.kHttpVerbGET;
                request.timeout = requestTimeout;
                SetRequestHeader(request, headers);

                //await for completed.
                var response = await this.engine.SendWebRequest(request);

                // Parse response and return
                return await this.DeserializeResponse(response);
            }
        }

        /// <summary>
        /// Make a Post request with custom header.
        /// </summary>
        public async UniTask<NetResponse> Post(string url, byte[] base64Data, Dictionary<string, string> headers = null)
        {
            //Debug.LogWarning($"POST {url}");
            // Setup the request
            this.RequestCount++;
            var requestData = await SerializeRequest(base64Data);
            using (var request = UnityWebRequest.Post(url, requestData))
            {
                request.method = UnityWebRequest.kHttpVerbPOST;
                request.timeout = requestTimeout;
                SetRequestHeader(request, headers);
                var response = await this.engine.SendWebRequest(request);

                // Parse response and return
                return await this.DeserializeResponse(response);
            }
        }

        /// <summary>
        /// Make a Put request with custom header.
        /// </summary>
        public async UniTask<NetResponse> Put(string url, byte[] base64Data, Dictionary<string, string> headers = null)
        {
            //Debug.LogWarning($"PUT {url}");
            // Setup the request
            this.RequestCount++;
            var requestData = await SerializeRequest(base64Data);
            using (var request = UnityWebRequest.Post(url, requestData))
            {
                request.method = UnityWebRequest.kHttpVerbPUT;
                request.timeout = requestTimeout;
                SetRequestHeader(request, headers);
                var response = await this.engine.SendWebRequest(request);

                // Parse response and return
                return await this.DeserializeResponse(response);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async UniTask<NetResponse> Delete(string url, Dictionary<string, string> headers = null)
        {
            this.RequestCount++;
            using (var request = UnityWebRequest.Delete(url))
            {
                request.method = UnityWebRequest.kHttpVerbDELETE;

                //set download handler by hand.
                if (request.downloadHandler == null)
                {
                    DownloadHandlerBuffer dH = new DownloadHandlerBuffer();
                    request.downloadHandler = dH;
                }

                request.timeout = requestTimeout;
                SetRequestHeader(request, headers);
                var response = await this.engine.SendWebRequest(request);

                // Parse response and return
                return await this.DeserializeResponse(response);
            }
        }

        /// <summary>
        /// Make a download request with custom header.
        /// </summary>
        public async UniTask<bool> Download(string url, string savePath, Dictionary<string, string> headers = null, Action<float> onProgress = null)
        {
            Debug.LogWarning($"DOWNLOAD {url}");
            this.RequestCount++;
            var download = new DownloadTask()
            {
                url = url,
                header = headers,
                path = savePath,
                done = false,
                result = false,
                retry = 0,
                progress = 0.0f
            };

            this.downloadTasks.Enqueue(download);
            while (!download.done)
            {
                onProgress?.Invoke(download.progress);
                await UniTask.Yield();
            }

            onProgress?.Invoke(1.0f);
            return download.result;
        }
        #endregion
        #region [Private Methods]
        /// <summary>
        /// Check and execute download tasks.
        /// </summary>
        private async UniTask ProcessDownloadTask()
        {
            while (this.IsRunning)
            {
                // If no download task or reach max downloading threads => Skip
                if (this.downloadTasks.Count > 0 && this.downloading < this.MaxConcurrentDownloads)
                {
                    // Else pick a task from the task queue
                    DownloadTask task = this.downloadTasks.Dequeue();
                    // Increase downloading counter
                    this.downloading++;

                    // Setup the request
                    var success = await this.engine.Download(task.url, task.path, task.header, progress =>
                    {
                        task.progress = progress;
                    });

                    // Debug.Log($"Run on Thread {Thread.CurrentThread.ManagedThreadId}");
                    if (success)
                    {
                        // If success, mark this task as done
                        task.done = true;
                        task.result = true;
                    }
                    else
                    {
                        // If fail, check for retrying
                        if (task.retry >= 3)
                        {
                            // If retried 3 times, exit with error
                            task.done = true;
                            task.result = false;
                        }
                        else
                        {
                            // Else put it again to download tasks
                            task.retry++;
                            this.downloadTasks.Enqueue(task);
                        }
                    }
                    // Decrease downloading counter
                    this.downloading--;
                }
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }

        /// <summary>
        /// Deserialize response to NetResponse
        /// </summary>
        private async UniTask<NetResponse> DeserializeResponse(IUnityWebResponse response)
        {
            if (response.IsNetworkError)
                throw new HttpException(HttpExceptionType.Connection, response.StatusCode, response.Error);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpException(HttpExceptionType.HttpStatus, response.StatusCode, response.Error);
            NetResponse result;
            await UniTask.SwitchToThreadPool();

            var container = new SmContainer(response.Text);

            result = new NetResponse(container.Code, container.Message, container.Data, container.Time, container.DataString);
            await UniTask.Yield();
            // Synchronize time here
            if (container.Time > 0.0)
            {
                time.Synchonize(container.Time);
            }
            //Debug.LogWarning($"{container.Code} | {container.Message}");
            return result;
        }

        private async UniTask<WWWForm> SerializeRequest(byte[] data)
        {
            var form = new WWWForm();
            await UniTask.SwitchToThreadPool();
            form.AddField("data", Convert.ToBase64String(data));
            await UniTask.Yield();
            return form;
        }

        private void SetRequestHeader(UnityWebRequest request, Dictionary<string, string> headers)
        {
            if (headers == null)
            {
                headers = this.defaultHeaders;
            }
            if (headers != null && headers.Count > 0)
            {
                foreach (var item in headers)
                    request.SetRequestHeader(item.Key, item.Value);
            }
        }
        #endregion


        #region [Utilities and Extra define]
        /// <summary>
        /// Request data.
        /// </summary>
        private class DownloadTask
        {
            public string url;
            public Dictionary<string, string> header;
            public string path;
            public bool done;
            public bool result;
            public int retry;
            public float progress;
        }
        #endregion
    }
}
