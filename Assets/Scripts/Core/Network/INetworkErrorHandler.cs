
using System;
using Cysharp.Threading.Tasks;

namespace Net.HungryBug.Core.Network
{
    public interface INetworkErrorHandler
    {
        /// <summary>
        /// Handle ann exception.
        /// </summary>
        UniTask HandleException(Exception exception);

        /// <summary>
        /// Handle ann exception.
        /// </summary>
        UniTask<string> HandleException(Exception exception, string dialogTemplate);

        /// <summary>
        /// Handle a response with its code.
        /// </summary>
        UniTask HandleResponseMessage<T>(CoreResponse<T> response);
    }
}
