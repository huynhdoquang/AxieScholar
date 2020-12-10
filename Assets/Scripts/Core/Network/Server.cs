using Net.HungryBug.Core.Engine;
using Net.HungryBug.Core.Security;
using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Net.HungryBug.Core.Network
{
    /// <summary>
    /// The implementation for <see cref="IServer"/>.
    /// </summary>
    public abstract class Server : IServer
    {
        [Inject] protected readonly IConfig Config;
        [Inject] protected readonly IHttpService HttpService;
        [Inject] protected readonly IUnity Engine;
        [Inject] private readonly IRSAService rsaService;
        [Inject] protected readonly IChecksum checksum;
    }
}