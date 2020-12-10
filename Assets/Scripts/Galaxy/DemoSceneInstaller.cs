using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Net.HungryBug.Core.DI;

public class DemoSceneInstaller : SceneInstallerBase
{
    [SerializeField] private TestHttpService testHttpService;
    protected override void SceneBindings()
    {
        this.Container.Bind<ITestHttpService>().FromInstance(testHttpService);
    }

    
}
