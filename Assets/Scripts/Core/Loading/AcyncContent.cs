using System;
using Cysharp.Threading.Tasks;

namespace Net.HungryBug.Core.Loading
{
    /// <summary>
    /// Create a <see cref="ILoadingContent"/> with an async task.
    /// </summary>
    public sealed class AsyncContent : ILoadingContent
    {
        public event Action<byte> OnProgressChanged;

        /// <summary>
        /// The async task.
        /// </summary>
        private Func<UniTask> task;

        /// <summary>
        /// Construct the <see cref="AsyncContent"/>.
        /// </summary>
        public AsyncContent(Func<UniTask> task)
        {
            this.task = task;
            this.Progress = 0;
        }

        /// <summary>
        /// Gets the loading progress.
        /// </summary>
        public byte Progress { get; private set; }


        /// <summary>
        /// Run all contents.
        /// </summary>
        public async UniTask Run()
        {
            await this.task.Invoke();
            this.Progress = 100;
            this.OnProgressChanged?.Invoke(100);
        }
    }

    /// <summary>
    /// Create a <see cref="ILoadingContent"/> with an async task.
    /// </summary>
    public sealed class AsyncContent<T> : ILoadingContent
    {
        public event Action<byte> OnProgressChanged;

        /// <summary>
        /// The async task.
        /// </summary>
        private Func<UniTask<T>> task;

        /// <summary>
        /// Construct the <see cref="AsyncContent{T}"/>.
        /// </summary>
        public AsyncContent(Func<UniTask<T>> task)
        {
            this.task = task;
            this.Progress = 0;
        }

        /// <summary>
        /// Gets the loading progress.
        /// </summary>
        public byte Progress { get; private set; }

        /// <summary>
        /// Run all contents.
        /// </summary>
        public async UniTask Run()
        {
            await this.task.Invoke();
            this.Progress = 100;
            this.OnProgressChanged?.Invoke(100);
        }
    }
}
