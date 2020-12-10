using Zenject;

namespace Net.HungryBug.Core.DI
{
    public abstract class SceneInstallerBase : MonoInstaller
    {
        public sealed override void InstallBindings()
        {
            GlobalApp.Register(this.gameObject.scene, this.Container);
            this.SceneBindings();
        }

        /// <summary>
        /// Unregister context on scene destroy.
        /// </summary>
        protected virtual void OnDestroy()
        {
            GlobalApp.UnRegister(this.gameObject.scene);
        }

        protected abstract void SceneBindings();
    }
}
