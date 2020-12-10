using Net.HungryBug.Core;
using Net.HungryBug.Core.Engine;
using Net.HungryBug.Core.Network;
using UnityEngine;
using Zenject;
using Net.HungryBug.Core.Security;
using Net.HungryBug.Core.DI;
using Net.HungryBug.Galaxy.Network;
using Net.HungryBug.Galaxy.Service;
using Net.HungryBug.Galaxy.Data;
using Net.HungryBug.Core.Loading;

namespace Net.HungryBug.Galaxy.Installer
{
    public class ProjectInstaller : ProjectInstallerBase
    {
        [SerializeField] private HostAsset hostConfig;

        protected override void ProjectBindings()
        {
            //bind host setting.
            this.Container.Bind<Host>().FromMethod(() => new Host(this.hostConfig.Api, this.hostConfig.Cdn, this.hostConfig.SkymavisApi, this.hostConfig.CoingeckoApi)).AsSingle().NonLazy();

            //resolve current host setting, then verify for develop environment.
            var h = this.Container.Resolve<Host>();

            //=========================END====================//

            //setup engine.
            var serviceContainer = new GameObject("$ServiceContainer").AddComponent<ServiceContainer>();
            serviceContainer.transform.SetParent(this.transform);
            this.Container.Bind<IServiceContainer>().FromInstance(serviceContainer);
            this.Container.Bind<ITime>().FromInstance(new TrustedTime());

            this.Container.Bind<IUnity>().To<Core.Engine.Unity>().AsSingle().Lazy();
            this.Container.Bind<IChecksum>().To<Checksum>().AsSingle().Lazy();

            this.Container.Bind<IErrorMessageHandler>().To<ErrorMessageHandler>().AsSingle().Lazy();
            this.Container.Bind<INetworkErrorHandler>().FromMethod(() => Container.Resolve<IErrorMessageHandler>()).AsCached().Lazy();

            this.Container.Bind<IRSAService>().To<RSAService>().AsSingle().Lazy();


            //network
            this.Container.Bind<IGalaxyConfig>().To<GalaxyConfig>().AsSingle().Lazy();
            this.Container.Bind<IConfig>().FromMethod(() => this.Container.Resolve<IGalaxyConfig>()).AsCached().Lazy();
            this.Container.Bind<IHttpService>().To<HttpService>().AsSingle().OnInstantiated(AsService).Lazy();
            this.Container.Bind<IFileService>().To<FileService>().AsSingle().Lazy();
            //this.Container.Bind<IFileService>().To<FileService>().AsSingle().Lazy();

            this.Container.Bind<IServer>().To<GalaxyServer>().AsSingle().Lazy();
            this.Container.Bind<IGalaxyServer>().FromMethod(() => (IGalaxyServer)Container.Resolve<IServer>()).AsCached().Lazy();


            //data
            this.Container.Bind<IMemoryStore>().To<MemoryStore>().AsSingle().Lazy();
            this.Container.Bind<ILoadingService>().To<LoadingService>().AsSingle().NonLazy();
            this.Container.Bind<IPlayerprefsHelper>().To<PlayerprefsHelper>().AsSingle().Lazy();

            //register a service to service container.
            void AsService(InjectContext context, object instance)
            {
                var service = instance as IService;
                if (service != null)
                {
                    serviceContainer.AddService(service);
                }
            }
        }
    }
}
