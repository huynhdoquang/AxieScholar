namespace Net.HungryBug.Core.Engine
{
    public interface IEventListener<T>
    {
        /// <summary>
        /// Gets value indicate if this listener still valid for invoking.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// 
        /// </summary>
        void OnInvoke(T arg);
    }
}
