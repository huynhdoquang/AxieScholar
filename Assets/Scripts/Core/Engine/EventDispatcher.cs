using UnityEngine;
using System.Collections.Generic;

namespace Net.HungryBug.Core.Engine
{
    public class EventDispatcher<T>
    {
        private readonly List<IEventListener<T>> listeners = new List<IEventListener<T>>();
        private bool isInvoking = false;

        /// <summary>
        /// 
        /// </summary>
        public void AddListener(IEventListener<T> listener)
        {
            if (listener == null)
                return;

            if (this.listeners.Contains(listener) == false)
                this.listeners.Add(listener);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveListener(IEventListener<T> listener)
        {
            if (listener == null)
                return;

            this.listeners.Remove(listener);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveAllListeners()
        {
            this.listeners.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void RaiseEvent(T arg)
        {
            if (this.isInvoking == true)
            {
                Debug.LogError($"EventDispatcher<{typeof(T).Name}> another invoking is in progress. skip to avoid infinity loop!");
                return;
            }

            if (this.listeners.Count == 0)
                return;

#if UNITY_EDITOR
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
# endif
            this.isInvoking = true;
            var arr = this.listeners.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != null && arr[i].IsValid == true)
                {
                    try
                    {
                        arr[i].OnInvoke(arg);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }

            this.isInvoking = false;
            this.CleanupListeners();

#if UNITY_EDITOR
            watch.Stop();
            if (watch.ElapsedMilliseconds > 500)
            {
                Debug.LogWarning($"EventDispatcher<{typeof(T).Name}> Found heavy listener, please optimize your listener!");
            }
# endif
        }

        /// <summary>
        /// 
        /// </summary>
        private void CleanupListeners()
        {
            for (int i = this.listeners.Count - 1; i >= 0; i--)
            {
                if (this.listeners[i] == null || this.listeners[i].IsValid == false)
                {
                    this.listeners.RemoveAt(i);
                }
            }
        }
    }
}
