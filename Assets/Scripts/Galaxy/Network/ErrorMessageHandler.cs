using Net.HungryBug.Core.Network;
using Net.HungryBug.Galaxy.Network;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Net.HungryBug.Galaxy.Service
{
    public class ErrorMessageHandler : IErrorMessageHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public UniTask HandleResponseMessage<T>(GalaxyResponse<T> response)
        {
            if (response.IsSuccess)
                return UniTask.CompletedTask;
            else
                return this.HandleResponseMessage((int)response.ResultCode, response.Message);
        }

        /// <summary>
        /// 
        /// </summary>
        public async UniTask HandleResponseMessage(int code, string rawMessage)
        {
            if (code == 0)
                return;

            Debug.LogError($"{rawMessage}: {code}");
        }

        public async UniTask HandleResponseMessageMineWar(int code, string rawMessage = "")
        {
            if (code == 0)
                return;
            Debug.LogError($"Error {rawMessage}: {code}");
        }

        /// <summary>
        /// 
        /// </summary>
        public async UniTask HandleResponseMessage<T>(CoreResponse<T> response)
        {
            if (response.IsSuccess)
            {
                return;
            }
            else
            {
                Debug.LogError($"{response.Message}: {(int)response.ResultCode}");
            }
        }

        /// <summary>
        /// Handle ann exception.
        /// </summary>
        public async UniTask<string> HandleException(Exception ex, string dialogTemplate)
        {
            var exception = ex.FinalException();
            Debug.LogError(exception);
            string messageText = "An unknown error has occurred.";

            if (exception is HttpException)
            {
                var http = exception as HttpException;
                switch (http.Type)
                {
                    case HttpExceptionType.Connection:
                        {
                            messageText = "Network connection error!";
                            break;
                        }

                    case HttpExceptionType.HttpStatus:
                        {
                            if (http.Code == System.Net.HttpStatusCode.RequestTimeout || http.Code == System.Net.HttpStatusCode.GatewayTimeout)
                            {
                                messageText = "Request timeout, please try again later.";
                            }
                            else
                            {
                                messageText = "Unexpected error. Please try again.";
                            }
                            break;
                        }
                    case HttpExceptionType.Parse:
                        {
                            messageText = "An unknown error has occurred.";
                            break;
                        }
                }
            }

            Debug.LogError($"{messageText}");

            return messageText;
        }

        /// <summary>
        /// 
        /// </summary>
        public UniTask HandleException(Exception ex) { return this.HandleException(ex, "DialogNames.Yes"); }

    }
}
