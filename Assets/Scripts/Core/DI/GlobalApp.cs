using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Net.HungryBug.Core.DI
{
    public class GlobalApp
    {
        private static GlobalApp instance;
        private static GlobalApp Instance { get { return instance = instance ?? new GlobalApp(); } }

        private DiContainer projectContainer;
        private Dictionary<string, SceneContainer> pairs = new Dictionary<string, SceneContainer>();

        public static void Create(DiContainer project)
        {
            if (Instance.projectContainer != null)
                throw new System.Exception("Already created a GlobalApp!");

            Instance.projectContainer = project;
        }

        /// <summary>
        /// register scene and its container.
        /// </summary>
        public static void Register(Scene scene, DiContainer container)
        {
            if (!Instance.pairs.ContainsKey(scene.path))
            {
                Instance.pairs.Add(scene.path, new SceneContainer(scene, container));
            }
            else
            {
                throw new System.Exception("[GlobalApp][Register] found duplicate scene " + scene.name);
            }
        }

        public static void UnRegister(Scene scene)
        {
            if (Instance.pairs.TryGetValue(scene.path, out var container))
            {
                Instance.pairs.Remove(scene.path);
            }
            else
            {
                Debug.Log("[GlobalApp][UnRegister] scene not found to unregister " + scene.name);
            }
        }

        /// <summary>
        /// Inject dependencies to target.
        /// </summary>
        public static void Inject(object target)
        {
            if (target is Component)
            {
                var targetScene = ((Component)target).gameObject.scene;
                GlobalApp.Inject(target, targetScene);
            }
            else
            {
                GlobalApp.Inject(target, null);
            }
        }

        /// <summary>
        /// Inject dependencies to object.
        /// </summary>
        public static void Inject(object target, Scene? sceneContext)
        {
            Exception lastEx = null;

            //try inject using project context.
            try
            {
                Instance.projectContainer.Inject(target);
                return;
            }
            catch (Exception e) { lastEx = e; }

            //try inject using scene contexts.
            if (sceneContext.HasValue)
            {
                if (Instance.pairs.TryGetValue(sceneContext.Value.path, out var container))
                {
                    try
                    {
                        container.di.Inject(target);
                        return;
                    }
                    catch (Exception e) { lastEx = e; }
                }
            }

            throw lastEx;
        }

        public static T Resolve<T>(Scene? scene = null)
        {
            Exception lastEx = null;

            //try inject using project context.
            try
            {
                return instance.projectContainer.Resolve<T>();
            }
            catch (Exception e) { lastEx = e; }

            if (scene.HasValue)
            {
                //try resolve using scene contexts.
                if (Instance.pairs.TryGetValue(scene.Value.path, out var container))
                {
                    try
                    {
                        return container.di.Resolve<T>();
                    }
                    catch (Exception e) { lastEx = e; }
                }
            }

            throw lastEx;
        }

        private class SceneContainer
        {
            public readonly Scene scene;
            public readonly DiContainer di;

            public SceneContainer(Scene scene, DiContainer container)
            {
                this.scene = scene;
                this.di = container;
            }
        }
    }
}
