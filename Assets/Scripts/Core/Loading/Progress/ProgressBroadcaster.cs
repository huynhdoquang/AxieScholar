using System.Collections.Generic;
using System.Linq;

namespace Net.HungryBug.Core.Loading.Progress
{
    /// <summary>
    /// 
    /// </summary>
    public class ProgressBroadcaster
    {
        private readonly HashSet<IProgress> progresses;
        private readonly Dictionary<IProgressSubscriber, SubscriberInfo> subscribers;

        /// <summary>
        /// 
        /// </summary>
        public ProgressBroadcaster()
        {
            this.progresses = new HashSet<IProgress>();
            this.subscribers = new Dictionary<IProgressSubscriber, SubscriberInfo>();
        }

        /// <summary>
        /// Gets all current active progress by chanel.
        /// </summary>
        public IProgress[] GetAllActiveProgresses(string chanel)
        {
            var result = new List<IProgress>();
            foreach (var p in this.progresses)
            {
                if (p.Chanel == chanel)
                {
                    result.Add(p);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Gets all current active progress by chanels.
        /// </summary>
        public IProgress[] GetAllActiveProgresses(string[] chanels)
        {
            var hashSet = new HashSet<string>();
            hashSet.AddRange(chanels);

            var result = new List<IProgress>();
            foreach (var p in this.progresses)
            {
                if(hashSet.Contains(p.Chanel))
                {
                    result.Add(p);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Starting a new broad cast.
        /// </summary>
        public void StartBroadcast(IProgress progress)
        {
            if (this.progresses.Add(progress))
            {
                var rm = new List<IProgressSubscriber>();

                //broadcast to subscribers.
                foreach (var pair in this.subscribers)
                {
                    if (pair.Key.IsDestroyed)
                    {
                        rm.Add(pair.Key);
                    }
                    else if (pair.Value.HasSubscribeTo(progress))
                    {
                        pair.Key.OnBroadcast(progress);
                    }
                }

                //cleanup destroyed subscribers
                foreach (var subscriber in rm)
                {
                    this.UnSubscribe(subscriber);
                }
            }
        }

        /// <summary>
        /// Stop a running broad cast, done, no more progress will be reported.
        /// </summary>
        public void EndBroadcast(IProgress progress)
        {
            if (this.progresses.Remove(progress))
            {
                var rm = new List<IProgressSubscriber>();

                //end broadcast to subscribers.
                foreach (var pair in this.subscribers)
                {
                    if (pair.Key.IsDestroyed)
                    {
                        rm.Add(pair.Key);
                    }
                    else if (pair.Value.HasSubscribeTo(progress))
                    {
                        pair.Key.OnEndBroadcast(progress);
                    }
                }

                //cleanup destroyed subscribers
                foreach (var subscriber in rm)
                {
                    this.UnSubscribe(subscriber);
                }
            }
        }

        /// <summary>
        /// Subscribe a progress task.
        /// </summary>
        public void Subscribe(IProgressSubscriber subscriber)
        {
            if (!this.subscribers.ContainsKey(subscriber))
            {
                this.subscribers.Add(subscriber, new SubscriberInfo(subscriber));
            }
        }

        /// <summary>
        /// Unsubscribe a progress task.
        /// </summary>
        public void UnSubscribe(IProgressSubscriber subscriber)
        {
            this.subscribers.Remove(subscriber);
        }

        #region [Extra Classes]
        private class SubscriberInfo
        {
            public readonly IProgressSubscriber Subscriber;

            /// <summary>
            /// Store a list of chanel for quick check.
            /// </summary>
            private readonly HashSet<string> chanels;

            /// <summary>
            /// Create a new instance of SubscriberInfo.
            /// </summary>
            public SubscriberInfo(IProgressSubscriber subscriber)
            {
                this.Subscriber = subscriber;
                this.chanels = new HashSet<string>();

                //hash the list of chanel that this subscriber whant to flow.
                for (int i = 0; i < subscriber.Chanels.Length; i++)
                {
                    this.chanels.Add(subscriber.Chanels[i]);
                }
            }

            /// <summary>
            /// Check if this subsciber care or not care about incomming progress.
            /// </summary>
            public bool HasSubscribeTo(IProgress progress) { return this.chanels.Contains(progress.Chanel); }
        }
        #endregion
    }
}
