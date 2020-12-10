namespace Net.HungryBug.Core.Loading.Progress
{
    public interface IProgressSubscriber
    {
        /// <summary>
        /// Gets value indicating whenever this subscriber has been destroyed, 
        /// This subscriber will be removed automatically from chanels.
        /// </summary>
        bool IsDestroyed { get; }

        /// <summary>
        /// Gets the list of chanel that this subscriber want to flow.
        /// </summary>
        string[] Chanels { get; }

        /// <summary>
        /// Invoke whenever a broadcast starting to sent progress.
        /// </summary>
        void OnBroadcast(IProgress progress);

        /// <summary>
        /// Invoke whenever a broadcast has been finished. no more progress will be reported.
        /// </summary>
        void OnEndBroadcast(IProgress progress);
    }
}
