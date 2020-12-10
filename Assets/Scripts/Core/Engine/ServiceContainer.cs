using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Net.HungryBug.Core.Engine
{
    public interface IService
    {
        /// <summary>
        /// Gets the value indicating if this service is active.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Invoke when this service has been added to the <see cref="ServiceContainer"/>.
        /// </summary>
        void Start();

        /// <summary>
        /// Invoke perframe if <see cref="IsRunning"/> is <see cref="true"/>
        /// </summary>
        /// <param name="timestamp">timestamp in seconds</param>
        void Update(float timestamp);

        /// <summary>
        /// Invoke before <see cref="IServiceContainer"/> has been destroyed.
        /// </summary>
        void OnDestroy();

        /// <summary>
        /// Invoke whenever the app has been pause or lost the focus.
        /// </summary>
        void OnAppPause();

        /// <summary>
        /// Invoke whenever the app has been focus again or unpause from paused status.
        /// </summary>
        void OnAppResume();
    }

    public interface IServiceContainer
    {
        /// <summary>
        /// Invoke whenever application get pause or unpause
        /// </summary>
        event Action<bool> OnApplicationPauseEvent;

        /// <summary>
        /// Invoke whenever application get focused or unfocused.
        /// </summary>
        event Action<bool> OnApplicationFocusEvent;

        /// <summary>
        /// Register a service to this container.
        /// </summary>
        void AddService(IService service);

        /// <summary>
        /// Start a coroutine.
        /// </summary>
        Coroutine StartCoroutine(IEnumerator coroutine);

        /// <summary>
        /// Stop a coroutine.
        /// </summary>
        void StopCoroutine(Coroutine coroutine);
    }

    /// <summary>
    /// Implementation of <see cref="IServiceContainer"/>. Wrapping some helpful Unity's methods and functions.
    /// </summary>
    public class ServiceContainer : MonoBehaviour, IServiceContainer
    {
        /// <summary>
        /// Invoke whenever application get pause or unpause
        /// </summary>
        public event Action<bool> OnApplicationPauseEvent;

        /// <summary>
        /// Invoke whenever application get focused or unfocused.
        /// </summary>
        public event Action<bool> OnApplicationFocusEvent;

        /// <summary>
        /// Storage all registed service.
        /// </summary>

        List<IService> services = new List<IService>();

        /// <summary>
        /// Register a service to this container.
        /// </summary>
        public void AddService(IService service)
        {
            this.services.Add(service);
            service.Start();
        }

        /// <summary>
        /// Invoke perframe.
        /// </summary>
        private void FixedUpdate()
        {
            for (int i = this.services.Count - 1; i >= 0; i--)
            {
                if (services[i] == null)
                {
                    this.services.RemoveAt(i);
                    continue;
                }

                if (services[i].IsRunning)
                    services[i].Update(Time.realtimeSinceStartup);
            }
        }

        /// <summary>
        /// Invoke on destroy.
        /// </summary>
        private void OnDestroy()
        {
            for (int i = this.services.Count - 1; i >= 0; i--)
            {
                if (services[i] == null)
                {
                    this.services.RemoveAt(i);
                    continue;
                }

                services[i].OnDestroy();
            }

            this.services.Clear();
        }

        /// <summary>
        /// Invoke whenever the app has been paused or unpaused again.
        /// </summary>
        private void OnApplicationPause(bool pause)
        {
            //invoke application pause event.
            this.OnApplicationPauseEvent?.Invoke(pause);

            if (pause)
            {
                this.OnAppPause();
            }
            else
            {
                this.OnAppResume();
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            //invoke application focus event.
            this.OnApplicationFocusEvent?.Invoke(focus);
        }

        /// <summary>
        /// Invoke all <see cref="IService.OnAppResume"/>
        /// </summary>
        private void OnAppResume()
        {
            for (int i = this.services.Count - 1; i >= 0; i--)
            {
                if (services[i] == null)
                {
                    this.services.RemoveAt(i);
                    continue;
                }

                if (services[i].IsRunning)
                    services[i].OnAppResume();
            }
        }

        /// <summary>
        /// Invoke all <see cref="IService.OnAppPause"/>
        /// </summary>
        private void OnAppPause()
        {
            for (int i = this.services.Count - 1; i >= 0; i--)
            {
                if (services[i] == null)
                {
                    this.services.RemoveAt(i);
                    continue;
                }

                if (services[i].IsRunning)
                    services[i].OnAppPause();
            }
        }
    }
}
