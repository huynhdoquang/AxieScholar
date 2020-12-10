using System;
using Cysharp.Threading.Tasks;

namespace Net.HungryBug.Galaxy.Network
{
    public interface IErrorMessageHandler : Core.Network.INetworkErrorHandler
    {
        UniTask HandleResponseMessage<T>(GalaxyResponse<T> response);
        UniTask HandleResponseMessage(int code, string rawMessage);
        UniTask HandleResponseMessageMineWar(int code, string rawMessage = null);
    }
}
