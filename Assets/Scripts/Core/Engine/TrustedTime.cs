using System;
using Net.HungryBug.Core.Utility;

namespace Net.HungryBug.Core
{
    public interface ITime
    {
        bool IsTrusted { get; }

        DateTime Now { get; }

        /// <summary>
        /// Get now timestamp in miliseconds
        /// </summary>
        long Timestamp { get; }

        DateTime GMT8 { get; }
        
        /// <summary>
		/// Get now timestamp in seconds
		/// </summary>
        double Time { get; }

        /// <summary>
		/// Synchronize network time and mark time is trusted
		/// </summary>
		/// <param name="networkTimestamp"></param>
        void Synchonize(double networkTimestamp);
    }

    /// <summary>
    /// Utilities for getting trusted time
    /// This time will be synchronized with server time
    /// </summary>
    public class TrustedTime : ITime
    {
        private static TrustedTime instance;
        public static TrustedTime Instance => instance ?? (instance = new TrustedTime());

        public bool IsTrusted { private set; get; }

        private double startUpTimestamp;
        private double startUpTime;

        public TrustedTime()
        {
            instance = this;
            this.startUpTime = UnityEngine.Time.time;
            this.startUpTimestamp = TimeConverter.DateTimeToTimestamp(DateTime.UtcNow) * 0.001;
        }

        /// <summary>
        /// Synchronize network time and mark time is trusted
        /// </summary>
        /// <param name="networkTimestamp"></param>
        public void Synchonize(double networkTimestamp)
        {
            this.startUpTime = UnityEngine.Time.time;
            this.startUpTimestamp = networkTimestamp;
            this.IsTrusted = true;
        }

        /// <summary>
        /// Get now timestamp in miliseconds
        /// </summary>
        public long Timestamp => (long)(this.Time * 1000);

        /// <summary>
		/// Get now timestamp in seconds
		/// </summary>
        public double Time => (this.startUpTimestamp + (UnityEngine.Time.time - this.startUpTime));

        /// <summary>
        /// Get now datetime
        /// </summary>
        public DateTime Now => TimeConverter.TimestampToDateTime(this.Timestamp);

        public DateTime GMT8 => (this.Now.AddHours(8));
    }
}
