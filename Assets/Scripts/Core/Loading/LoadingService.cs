using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Net.HungryBug.Core.Loading
{

    public interface ILoadingService
    {
        /// <summary>
        /// Invoke before the loading.
        /// </summary>
        event Action OnStart;

        /// <summary>
        /// Invoke after fully loading.
        /// </summary>
        event Action OnCompleted;

        /// <summary>
        /// Decsription of current step
        /// </summary>
        string Decsription { get; }

        /// <summary>
        /// Gets value indicating if the <see cref="ILoadingService"/> is busy.
        /// </summary>
        bool IsCreated { get; }

        /// <summary>
        /// Gets value indicating whenever the loading service is executing.
        /// </summary>
        bool IsExecuting { get; }

        /// <summary>
        /// Gets the loading progress.
        /// </summary>
        byte Progress { get; }

        /// <summary>
        /// Gets the current content is loading.
        /// </summary>
        string CurrentStep { get; }

        /// <summary>
        /// Create a new loading,
        /// </summary>
        ILoadingService Create(string name, ILoadingContent content, int weight = 1, string displayName = "LOADING_SCENE");

        /// <summary>
        /// Create a new loading,
        /// </summary>
        ILoadingService Create(string name, Func<UniTask> content, int weight = 1, string displayName = "LOADING_SCENE");

        /// <summary>
        /// Create a new loading,
        /// </summary>
        ILoadingService Create<T>(string name, Func<UniTask<T>> content, int weight = 1, string displayName = "LOADING_SCENE");

        /// <summary>
        /// On boot error.
        /// </summary>
        ILoadingService OnError(Action<Exception> onFailed);

        /// <summary>
        /// Add a list of contents to run sequentially
        /// </summary>
        ILoadingService Then(string name, ILoadingContent content, int weight = 1, string displayName = "LOADING_SCENE");

        /// <summary>
        /// Add an async method to run sequentially
        /// </summary>
        ILoadingService Then(string name, Func<UniTask> content, int weight = 1, string displayName = "LOADING_SCENE");

        /// <summary>
        /// Add an async method to run sequentially
        /// </summary>
        ILoadingService Then<T>(string name, Func<UniTask<T>> content, int weight = 1, string displayName = "LOADING_SCENE");

        /// <summary>
        /// Excute built content.
        /// </summary>
        UniTask Execute();
    }

    public class LoadingService : ILoadingService
    {
        /// <summary>
        /// 
        /// </summary>
        private class Step
        {
            public int Weight;
            public string Name;
            public string DisplayName;

            public Step(string name, int weight, string displayName) { this.Weight = weight; this.Name = name; this.DisplayName = displayName; }
        }

        private Dictionary<ILoadingContent, Step> steps;

        private int childrenWeight = 0;

        /// <summary>
        /// Invoke before the loading.
        /// </summary>
        public event Action OnStart;

        /// <summary>
        /// Invoke after fully loading.
        /// </summary>
        public event Action OnCompleted;

        /// <summary>
        /// Gets value indicating if the <see cref="ILoadingService"/> is busy.
        /// </summary>
        public bool IsCreated { get; private set; }

        /// <summary>
        /// Gets value indicating whenever the loading service is executing.
        /// </summary>
        public bool IsExecuting => this.isExecuting;

        /// <summary>
        /// Gets the loading progress.
        /// </summary>
        public byte Progress { get; private set; }

        /// <summary>
        /// Gets the current content is loading.
        /// </summary>
        public string CurrentStep { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Decsription { get; set; }

        /// <summary>
        /// Invoke action on failed to load.
        /// </summary>
        private Action<Exception> onFailed;
        private bool isExecuting = false;

        /// <summary>
        /// Construct the loading service.
        /// </summary>
        public LoadingService()
        {
            this.steps = new Dictionary<ILoadingContent, Step>();
            this.IsCreated = false;
            this.isExecuting = false;
        }

        /// <summary>
        /// Create a new loading,
        /// </summary>
        public ILoadingService Create(string name, ILoadingContent content, int weight, string displayName = "LOADING_SCENE")
        {
            if (!this.IsCreated && !this.isExecuting)
            {
                this.steps.Clear();
                this.onFailed = null;
                this.IsCreated = true;
                this.childrenWeight = weight;
                this.steps.Add(content, new Step(name, weight, displayName));
                return this;
            }
            else
            {
                Debug.LogError("Loading: Is busy!");
            }

            return this;
        }

        /// <summary>
        /// Create a new loading,
        /// </summary>
        public ILoadingService Create(string name, Func<UniTask> content, int weight = 1, string displayName = "LOADING_SCENE")
        {
            if (!this.IsCreated && !this.isExecuting)
            {
                this.steps.Clear();
                this.onFailed = null;
                this.IsCreated = true;
                this.childrenWeight = weight;
                this.steps.Add(new AsyncContent(content), new Step(name, weight, displayName));
                return this;
            }
            else
            {
                Debug.LogError("[LoadingService][Create] Loading: The previous loading must be done to start a new one.");
            }

            return this;
        }

        /// <summary>
        /// Create a new loading,
        /// </summary>
        public ILoadingService Create<T>(string name, Func<UniTask<T>> content, int weight = 1, string displayName = "LOADING_SCENE")
        {
            if (!this.IsCreated && !this.isExecuting)
            {
                this.steps.Clear();
                this.onFailed = null;
                this.IsCreated = true;
                this.childrenWeight = weight;
                this.steps.Add(new AsyncContent<T>(content), new Step(name, weight, displayName));
                return this;
            }
            else
            {
                Debug.LogError("[LoadingService][Create] Loading: The previous loading must be done to start a new one.");
            }

            return this;
        }

        /// <summary>
        /// Add a list of contents to run sequentially
        /// </summary>
        public ILoadingService Then(string name, ILoadingContent content, int weight = 1, string displayName = "LOADING_SCENE")
        {
            if (content != null)
            {
                if (this.IsCreated && !this.isExecuting)
                {
                    this.steps.Add(content, new Step(name, weight, displayName));
                    this.childrenWeight += weight;
                }
                else
                {
                    Debug.LogError($"[LoadingService][Then] Invalid state: isCreated {this.IsCreated}, isExecuting {this.isExecuting}");
                }

            }

            return this;
        }

        /// <summary>
        /// Add an async method to run sequentially
        /// </summary>
        public ILoadingService Then(string name, Func<UniTask> content, int weight = 1, string displayName = "LOADING_SCENE")
        {
            if (this.IsCreated && !this.isExecuting)
            {
                this.steps.Add(new AsyncContent(content), new Step(name, weight, displayName));
                this.childrenWeight += weight;
            }
            else
            {
                Debug.LogError($"[LoadingService][Then] Invalid state: isCreated {this.IsCreated}, isExecuting {this.isExecuting}");
            }

            return this;
        }

        /// <summary>
        /// Add an async method to run sequentially
        /// </summary>
        public ILoadingService Then<T>(string name, Func<UniTask<T>> content, int weight = 1, string displayName = "LOADING_SCENE")
        {
            if (this.IsCreated && !this.isExecuting)
            {
                this.steps.Add(new AsyncContent<T>(content), new Step(name, weight, displayName));
                this.childrenWeight += weight;
            }
            else
            {
                Debug.LogError($"[LoadingService][Then] Invalid state: isCreated {this.IsCreated}, isExecuting {this.isExecuting}");
            }

            return this;
        }

        /// <summary>
        /// On boot error.
        /// </summary>
        public ILoadingService OnError(Action<Exception> onFailed)
        {
            if (this.IsCreated)
            {
                this.onFailed = onFailed;
            }
            else
            {
                Debug.LogError($"[LoadingService][OnError] Invalid state: isCreated {this.IsCreated}, isExecuting {this.isExecuting}");
            }

            return this;
        }

        /// <summary>
        /// Excute built content.
        /// </summary>
        public async UniTask Execute()
        {
            if (this.IsCreated && !this.isExecuting)
            {
                this.Progress = 0;
                this.isExecuting = true;
                try
                {
                    byte doneProgress = 0;
                    float stepPercent = 0f;

                    this.OnStart?.Invoke();
                    foreach (var pair in this.steps)
                    {
                        await UniTask.Yield();
                        stepPercent = 1.0f * pair.Value.Weight / this.childrenWeight;
                        this.CurrentStep = pair.Value.DisplayName;

                        pair.Key.OnProgressChanged += OnProgressChanged;
                        await pair.Key.Run();
                        pair.Key.OnProgressChanged -= OnProgressChanged;

                        //recalculate weight - percent
                        doneProgress += (byte)(100 * stepPercent);
                        this.Progress = doneProgress;
                        stepPercent = 0;
                    }

                    //end.
                    this.IsCreated = false;
                    this.isExecuting = false;


                    this.OnCompleted?.Invoke();
                    void OnProgressChanged(byte p) { this.Progress = (byte)(doneProgress + stepPercent * p); }
                }
                catch (Exception e)
                {
                    this.IsCreated = false;
                    this.isExecuting = false;

                    this.OnCompleted?.Invoke();

                    if (this.onFailed != null)
                    {
                        //Debug.LogError(e);
                        this.onFailed(e);
                    }
                    else
                    {
                        this.Progress = 1;
                        throw e;
                    }
                }

                this.Progress = 1;
            }
        }
    }
}
