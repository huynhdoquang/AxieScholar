using Net.HungryBug.Core.DI;
using Net.HungryBug.Core.Engine;
using Net.HungryBug.Core.Network;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Net.HungryBug.Core.UI
{
    [RequireComponent(typeof(RawImage))]
    public class UIWebImage : MonoBehaviour
    {
#if !UNITY_WEBGL
        [Inject]
        IHttpService httpService;
#endif

        [Inject]
        IUnity engine;

        RawImage rawImage;

        [SerializeField]
        private string imageUrl;
        public string ImageUrl
        {
            get { return this.imageUrl; }
            set
            {
                if (this.imageUrl == value || string.IsNullOrEmpty(value))
                    return;

                this.imageUrl = value;
                _ = this.RefreshImage();
            }
        }

        [Tooltip("parent rect transform to Stretch raw image")]
        [SerializeField] private RectTransform parentRectTransform;

        /// <summary>
        /// Initialize.
        /// </summary>
        private void Awake()
        {
            this.rawImage = this.GetComponent<RawImage>();
        }

        /// <summary>
        /// Initialize image.
        /// </summary>
        private void Start()
        {
            if (!string.IsNullOrEmpty(this.ImageUrl))
                _ = this.RefreshImage();
        }

        /// <summary>
        /// Refresh the image by new url.
        /// </summary>
        private async UniTask RefreshImage()
        {
#if !UNITY_WEBGL
            try
            {
                if (this.httpService == null)
                {
                    GlobalApp.Inject(this);
                }

                if (this.isOnRefresh)
                {
                    Debug.Log("[RefreshImage on Start] return by refreshing.");
                    return;
                }

                var path = System.IO.Path.Combine(CorePaths.FileService, "WebImage", this.HashUrl(this.imageUrl));
                var success = await this.httpService.Download(this.imageUrl, path);
                if (success)
                {
                    var imageBytes = await this.engine.LoadFileBinary(path);
                    if (imageBytes != null)
                    {
                        var t = new Texture2D(2, 2);
                        t.LoadImage(imageBytes, true);
                        rawImage.texture = t;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
#endif
        }


        Queue queue = new Queue();

        bool isOnRefresh = false;
        public async UniTask<bool> RefreshImage(string imgLink, string fileName = "", bool isAddQueue = true)
        {
#if !UNITY_WEBGL
            try
            {
                if(isAddQueue)
                    queue.Enqueue($"{imgLink}|{fileName}");

                if (this.httpService == null)
                {
                    GlobalApp.Inject(this);
                }

                if (this.isOnRefresh)
                {
                    //Debug.Log("[RefreshImage ] return by refreshing.");
                    return true;
                }

                this.isOnRefresh = true;

                this.imageUrl = imgLink;

                var save_fileName = string.Empty;
                if (string.IsNullOrEmpty(fileName) == true)
                {
                    save_fileName = this.HashUrl(this.imageUrl);
                }
                else
                {
                    save_fileName = fileName;
                }

                var path = System.IO.Path.Combine(CorePaths.FileService, "WebImage", save_fileName);

                var isExitsFile = engine.ExistsFile(path);
                if (isExitsFile == true)
                {
                    var result = await LoadImage();

                    queue.Dequeue();
                    while (queue.Count > 1)
                    {
                        queue.Dequeue();
                    }
                    if (queue.Count == 1)
                    {
                        await RefreshImage(queue.Peek() as string, isAddQueue: false);
                    }

                    return result;
                }
                else
                {
                    var success = await this.httpService.Download(this.imageUrl, path);
                    if (success)
                    {
                        var result = await LoadImage();
                        queue.Dequeue();

                        while (queue.Count > 1)
                        {
                            queue.Dequeue();
                        }
                        if (queue.Count == 1)
                        {
                            var names = queue.Peek() as string;
                            var split = names.Split('|');
                            var split_link = split[0];
                            var split_name = split.Length > 1 ? split[1] : string.Empty;
                            await RefreshImage(split_link, split_name, isAddQueue: false);
                        }
                        return result;
                    }
                    else
                    {
                        this.isOnRefresh = false;
                        return false;
                    }
                }

                async UniTask<bool> LoadImage()
                {
                    var imageBytes = await this.engine.LoadFileBinary(path);
                    if (imageBytes != null)
                    {
                        var t = new Texture2D(2, 2);
                        t.LoadImage(imageBytes, true);
                        rawImage.texture = t;
                        this.isOnRefresh = false;
                        return true;
                    }
                    this.isOnRefresh = false;
                    return false;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                this.isOnRefresh = false;
                return false;
            }

#else
            return false;
#endif
        }

        /// <summary>
        /// Convert url to md5 hash.
        /// </summary>
        private string HashUrl(string url)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(url));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public Vector2 SizeToParent(float padding = 0)
        {
            float w = 0, h = 0;
            var imageTransform = rawImage.GetComponent<RectTransform>();
            var parent = parentRectTransform;
            if (parent == null)
            {
                return imageTransform.sizeDelta;
            }

            // check if there is something to do
            if (rawImage.texture != null)
            {
                if (!parent) { return imageTransform.sizeDelta; } //if we don't have a parent, just return our current width;
                padding = 1 - padding;
                float ratio = rawImage.texture.width / (float)rawImage.texture.height;
                var bounds = new Rect(0, 0, parent.rect.width, parent.rect.height);
                if (Mathf.RoundToInt(imageTransform.eulerAngles.z) % 180 == 90)
                {
                    //Invert the bounds if the image is rotated
                    bounds.size = new Vector2(bounds.height, bounds.width);
                }
                //Size by height first
                h = bounds.height * padding;
                w = h * ratio;
                if (w > bounds.width * padding)
                { //If it doesn't fit, fallback to width;
                    w = bounds.width * padding;
                    h = w / ratio;
                }
            }
            imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
            imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
            return imageTransform.sizeDelta;
        }

        public void StretchFitToParent(float padding = 0)
        {
            float w = 0, h = 0;
            var imageTransform = rawImage.GetComponent<RectTransform>();
            var parent = parentRectTransform;
            if (parent == null)
            {
                return;
            }

            // check if there is something to do
            if (rawImage.texture != null)
            {
                if (!parent) { return;} //if we don't have a parent, just return our current width;
                padding = 1 - padding;
                float ratio = rawImage.texture.width / (float)rawImage.texture.height;
                var bounds = new Rect(0, 0, parent.rect.width, parent.rect.height);
                if (Mathf.RoundToInt(imageTransform.eulerAngles.z) % 180 == 90)
                {
                    //Invert the bounds if the image is rotated
                    bounds.size = new Vector2(bounds.height, bounds.width);
                }
                //Size by height first
                h = bounds.height * padding;
                w = bounds.width * padding;
            }
            imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
            imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
        }
    }
}
