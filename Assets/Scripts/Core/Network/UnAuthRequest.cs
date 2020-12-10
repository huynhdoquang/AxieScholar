using Net.HungryBug.Core.DI;
using Net.HungryBug.Core.Network;
using System;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Net.HungryBug.Galaxy.Network
{
    public class UnAuthRequest
    {

        private static UnAuthRequest instance;
        public UnAuthRequest()
        {
            if (instance != null)
            {
                throw new System.InvalidOperationException("Already created an instance of AuthRequest");
            }
            else
            {
                instance = this;
            }

            //Inject dependencies.
            GlobalApp.Inject(this);
        }

        public static UniTask<TResponse> SentRequest<TResponse, TModel>(Func<UniTask<TResponse>> requestFunc)
            where TResponse : Core.Network.Response<TModel>
        {
            if (instance == null)
            {
                new UnAuthRequest();
            }

            return instance.DoSentRequest<TResponse, TModel>(requestFunc);
        }

        private async UniTask<TResponse> DoSentRequest<TResponse, TModel>(Func<UniTask<TResponse>> requestFunc)
                where TResponse : Core.Network.Response<TModel>
        {
            try
            {
                return await requestFunc();
            }
            catch(Exception e)
            {
                /* bool retry = await this.authAction.NetworkConnectionAndRetryDialog(e);
                 if (retry)
                 {
                     return await DoSentRequest<TResponse, TModel>(requestFunc);
                 }
                 else
                 {
                     return null;
                 }*/

                return null;
            }
        }

    }
}

