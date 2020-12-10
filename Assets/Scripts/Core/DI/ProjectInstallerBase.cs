using Zenject;

namespace Net.HungryBug.Core.DI
{
    public abstract class ProjectInstallerBase : MonoInstaller
    {
        public sealed override void InstallBindings()
        {
            GlobalApp.Create(this.Container);
            this.ProjectBindings();
        }

        protected abstract void ProjectBindings();
    }
}
