
using Net.HungryBug.Core.Network;
using Net.HungryBug.Core.Network.Data;
using System;
using Cysharp.Threading.Tasks;
using GraphQL;
using UnityEngine;
using Newtonsoft.Json;

namespace Net.HungryBug.Galaxy.Network
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGalaxyServer : IServer
    {
        UniTask<TResponse> DoUnAuthGet<TResponse, TModel>(string url, bool useThreadPool = true) where TResponse : Response<TModel>;

        UniTask<TResponse> DoUnAuthPut<TResponse, TModel>(string url, IRequestData data, bool useThreadPool = true) where TResponse : Response<TModel>;

        UniTask<TResponse> DoUnAuthPost<TResponse, TModel>(string url, IRequestData data, bool useThreadPool = true) where TResponse : Response<TModel>;


        //query

        void GetProfileNameByEthAddress(string ethereumAddress);
    }

    /// <summary>
    /// 
    /// </summary>
    public class GalaxyServer : Server, IGalaxyServer
    {
        #region UnAuth
        private async UniTask<TResponse> CreateResponseIntance<TResponse, TModel>(NetResponse res, bool useThreadPool) where TResponse : Response<TModel>
        {
            TResponse result = null;

            if (useThreadPool)
            {
                await UniTask.SwitchToThreadPool();
                result = (TResponse)Activator.CreateInstance(typeof(TResponse), res);
                await UniTask.Yield();
            }
            else
                result = (TResponse)Activator.CreateInstance(typeof(TResponse), res);

            return result;
        }

        public UniTask<TResponse> DoUnAuthGet<TResponse, TModel>(string url, bool useThreadPool = true) where TResponse : Response<TModel>
        {
            return UnAuthRequest.SentRequest<TResponse, TModel>(async () =>
            {
                var res = await this.HttpService.Get(url);

                return await CreateResponseIntance<TResponse, TModel>(res, useThreadPool);
            });
        }

        public UniTask<TResponse> DoUnAuthPut<TResponse, TModel>(string url, IRequestData data, bool useThreadPool = true) where TResponse : Response<TModel>
        {
            return UnAuthRequest.SentRequest<TResponse, TModel>(async () =>
            {

                byte[] reqData = null;
                if (data == null)
                    reqData = new byte[1] { 0 };
                else
                {
                    await UniTask.SwitchToThreadPool();
                    reqData = data.Serialize();
                    await UniTask.Yield();
                }

                var res = await this.HttpService.Put(url, reqData);

                return await CreateResponseIntance<TResponse, TModel>(res, useThreadPool);
            });
        }

        public UniTask<TResponse> DoUnAuthPost<TResponse, TModel>(string url, IRequestData data, bool useThreadPool = true) where TResponse : Response<TModel>
        {
            return UnAuthRequest.SentRequest<TResponse, TModel>(async () =>
            {

                byte[] reqData = null;
                if (data == null)
                    reqData = new byte[1] { 0 };
                else
                {
                    await UniTask.SwitchToThreadPool();
                    reqData = data.Serialize();
                    await UniTask.Yield();
                }

                var res = await this.HttpService.Post(url, reqData);

                return await CreateResponseIntance<TResponse, TModel>(res, useThreadPool);
            });
        }
        #endregion

        public void GetProfileNameByEthAddress(string ethereumAddress)
        {

            string query =
              @"query GetProfileNameByEthAddress($ethereumAddress: String!) {
                  publicProfileWithEthereumAddress(ethereumAddress: $ethereumAddress) {
                    accountId
                    name
                    __typename
                  }
                }";


            APIGraphQL.Query(query, new { ethereumAddress = ethereumAddress }, response => {
                QmGetPublicAdress myDeserializedClass = JsonConvert.DeserializeObject<QmGetPublicAdress>(response.Raw);
                var name = myDeserializedClass.data.publicProfileWithEthereumAddress.name;
                Debug.Log("name: " + name); });
        }


    }
}
