using Cysharp.Threading.Tasks;
using Net.HungryBug.Core.Loading;
using Net.HungryBug.Galaxy.Network;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using Zenject;
using Net.HungryBug.Core.Engine;
using Net.HungryBug.Core.Network;
using Net.HungryBug.Galaxy.Data;
using Net.HungryBug.Core;
using TMPro;
using System.Collections;
using Net.HungryBug.Core.Attribute;
using System.Collections.Generic;

namespace Net.HungryBug.Galaxy.Loading
{
    public class s000_bootstrap : MonoBehaviour
    {
        [Inject] private readonly ILoadingService loadingService;
        [Inject] private readonly IErrorMessageHandler errorHandler;

        [Inject] protected readonly IGalaxyConfig Config;
        [Inject] protected readonly IGalaxyServer server;
        [Inject] protected readonly IUnity engine;
        [Inject] protected readonly IHttpService httpService;

        [Inject] protected readonly IMemoryStore memoryStore;

        private async void Start()
        {
            var sequence = new SequenceContent();

            //Make load and navigate to start scene after finished all steps.
            await this.loadingService
                .Create("Download lastest resources", () => DownloadLastestResource(), 3)
                .Then("Open start scene", () => LoadScene(SceneNames.S_Demo), 1)
                .OnError(this.OnBootFailed)
                .Execute();
        }

        /// <summary>
        /// Initialize game analytics.
        /// </summary>
        private async UniTask DownloadLastestResource()
        {
            return;

            var ie = this.StartCoroutine(this.UpdateLoadingIndicator("GetBodyPart"));
            await GetBodyPart();
            this.StopCoroutine(ie);

            var ie_1 = this.StartCoroutine(this.UpdateLoadingIndicator("GetAllCardAbilities"));
            await GetAllCardAbilities();
            this.StopCoroutine(ie_1);
        }

        async UniTask GetBodyPart()
        {
            this.txtProgress.text = string.Empty;

            var url = string.Format(this.Config.ApiBodyPart);

            var res = await this.server.DoUnAuthGet<ResBodyPart, SmAllBodyPart>(url);

            foreach (var item in res.Object.MyArray)
            {
                Debug.LogWarning($"{item.partId} | {item.name}");
            }

            this.txtProgress.text = $"Part Count: {res.Object.MyArray.Count}";
            this.memoryStore.SmAllBodyPart = res.Object;
        }

        async UniTask GetAllCardAbilities()
        {
            this.txtProgress.text = string.Empty;

            var url = string.Format(this.Config.ApiGetCardAbilities);
            var res = await this.server.DoUnAuthGet<ResGetAllCardAbilities, SmGetAllCardAbilities>(url);

            if (res != null)
            {
                this.memoryStore.SmGetAllCardAbilities = res.Object;


                this.txtProgress.text = $"{res.Object.CardAbilitiesDict.Count}";

               /* List<UniTask<bool>> tasks = new List<UniTask<bool>>();
                var i = 0;
                foreach (var item in res.Object.CardAbilitiesDict)
                {
                    var imgLink = string.Format(this.Config.ApiGetCardImage, item.Value.id);

                    var save_fileName = $"{item.Value.id}.png";
                    var path = System.IO.Path.Combine(CorePaths.FileService, "WebImage", save_fileName);

                    //tasks.Add(this.httpService.Download(imgLink, path));
                    var success = await this.httpService.Download(imgLink, path);
                    if (success)
                    {
                        i++;
                        this.txtProgress.text = $"{i}/{res.Object.CardAbilitiesDict.Count}";
                        //save file to resource holder
                    }
                    else
                    {
                        //notify download fail
                    }
                }*/

                //await UniTask.WhenAll(tasks);
            }
        }

        private UniTask LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            return UniTask.CompletedTask;
        }

        [UIOutlet("@[Text] Indicator")]
        [SerializeField] private TextMeshProUGUI loadingIndicator;

        [UIOutlet("@[Text] Progress")]
        [SerializeField] private TextMeshProUGUI txtProgress;
        private string[] dots = new string[]
        {
            "",
            ".",
            "..",
            "..."
        };
        private IEnumerator UpdateLoadingIndicator(string taskName)
        {
            int index = 0;
            while (this.gameObject.activeInHierarchy)
            {
                var str = taskName;
                str += dots[index];
                this.loadingIndicator.text = str;
                yield return new WaitForSecondsRealtime(0.3f);

                index++;
                if (index >= dots.Length)
                    index = 0;
            }
        }

        /// <summary>
        /// Quick the game on boot error.
        /// </summary>
        private async void OnBootFailed(Exception exception)
        {
            await this.errorHandler.HandleException(exception);
            this.LoadScene(SceneNames.S000_Boot).WrapErrors();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;

#else
            Application.Quit();
#endif
        }
    }
}