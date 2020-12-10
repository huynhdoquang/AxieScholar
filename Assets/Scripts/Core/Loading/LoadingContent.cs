using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Net.HungryBug.Core.Loading
{
    public abstract class LoadingContent : ILoadingContent
    {
        public event Action<byte> OnProgressChanged;

        #region [Private Data]
        private LoadingContent parent;
        protected Dictionary<ILoadingContent, int> contents;

        private float accumulatePercent = 0;
        private int childrenWeight;
        #endregion

        #region [ILoadingContent]

        byte progress = 0;
        /// <summary>
        /// Gets the loading progress.
        /// </summary>
        public byte Progress
        {
            get { return this.progress; }
            set
            {
                if (this.progress == value)
                    return;

                this.progress = value;
                this.OnProgressChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// Run all contents.
        /// </summary>
        public UniTask Run()
        {
            this.Progress = 0;
            this.accumulatePercent = 0;
            return this.RunContents();
        }
        #endregion

        /// <summary>
        /// Construct the <see cref="LoadingContent"/>.
        /// </summary>
        public LoadingContent()
        {
            this.contents = new Dictionary<ILoadingContent, int>();
            this.Progress = 0;
        }

        #region [Public Methods]
        /// <summary>
        /// Add a content as a child.
        /// </summary>
        public LoadingContent Add(ILoadingContent child, int weight)
        {
            this.contents.Add(child, weight);

            if (child is LoadingContent)
                ((LoadingContent)child).parent = this;

            this.childrenWeight += weight;
            return this;
        }

        /// <summary>
        /// Add a async task as a child.
        /// </summary>
        public LoadingContent Add(Func<UniTask> child, int weight = 1)
        {
            this.contents.Add(new AsyncContent(child), weight);
            this.childrenWeight += weight;
            return this;
        }

        /// <summary>
        /// Add a async task as a child.
        /// </summary>
        public LoadingContent Add<T>(Func<UniTask<T>> child, int weight = 1, string name = "")
        {
            this.contents.Add(new AsyncContent<T>(child), weight);
            this.childrenWeight += weight;
            return this;
        }
        #endregion

        #region [Private Methods]
        /// <summary>
        /// Run the <see cref="contents"/>.
        /// </summary>
        protected abstract UniTask RunContents();

        /// <summary>
        /// Call to parent and update parent progress.
        /// </summary>
        protected void UpdateProgress(ILoadingContent child, byte childDeltaProgress)
        {
            if (this.contents.TryGetValue(child, out var weight))
            {
                this.accumulatePercent += 1.0f * childDeltaProgress * weight / this.childrenWeight;
                byte delta = (byte)this.accumulatePercent;
                if (delta > 0)
                {
                    this.Progress += delta;
                    this.accumulatePercent = this.accumulatePercent - delta;

                    //call to parent and update parent's process.
                    this.parent?.UpdateProgress(this, delta);
                }
            }
        }
        #endregion
    }
}
