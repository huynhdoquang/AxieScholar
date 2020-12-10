using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Net.HungryBug.Core.Utility
{
    public class GameObjectPool<T> where T : MonoBehaviour
    {
        public event Action<T> OnCreatedItem;
        public event Action<T> OnCollectedItem;

        private bool initialized = false;
        private readonly T template;
        private readonly Transform container;
        private readonly int poolCount;
        private readonly bool isPrefabTemplate;
        public readonly List<T> Items = new List<T>();
        private readonly Stack<T> pool = new Stack<T>();
        private readonly List<T> usingItems = new List<T>();
        private readonly Transform cacheTransform;

        public bool Initialized { get => initialized; set => initialized = value; }

        /// <summary>
        /// Create the new object pool, the template will be disabled.
        /// </summary>
        public GameObjectPool(T template, Transform container, int poolCount, bool prefabTemplate = false, Transform cacheTransform = null)
        {
            this.template = template;
            this.container = container;
            this.poolCount = poolCount;
            this.isPrefabTemplate = prefabTemplate;
            this.cacheTransform = cacheTransform != null ? cacheTransform : container;

            if (!this.isPrefabTemplate)
                this.template.gameObject.SetActive(false);
        }

        public int InUsed()
        {
            return usingItems.Count;
        }

        /// <summary>
        /// Initialize the pool.
        /// </summary>
        public async UniTask Initialize()
        {
            if (this.Initialized)
                return;
            this.Initialized = true;

            for (int i = 0; i < this.poolCount; i++)
            {
                if ((i + 1) % 20 == 0)
                {
                    await UniTask.Yield();
                }
                var clone = GameObject.Instantiate(this.template.gameObject, this.container);
                clone.SetActive(false);

                var item = clone.GetComponent<T>();
                this.Items.Add(item);
                this.pool.Push(item);
            }
        }

        public async UniTask Initialize(int numberPerFrame)
        {
            if (this.Initialized)
                return;

            for (int i = 0; i < this.poolCount; i++)
            {
                if ((i + 1) % numberPerFrame == 0)
                {
                    await UniTask.Yield();
                }
                var clone = GameObject.Instantiate(this.template.gameObject, this.container);
                clone.SetActive(false);

                var item = clone.GetComponent<T>();
                this.Items.Add(item);
                this.pool.Push(item);
            }
            this.Initialized = true;
        }

        /// <summary>
        /// Provide and active an instance of <see cref="T"/>.
        /// </summary>
        public T Provide(Transform overrideParent = null)
        {
            T item = null;
            var p = overrideParent != null ? overrideParent : this.container;
            if (this.pool.Count > 0)
            {
                item = this.pool.Pop();
                item.gameObject.SetActive(true);
                item.transform.SetParent(p);
            }
            else
            {
                var clone = GameObject.Instantiate(this.template.gameObject, p);
                clone.SetActive(true);
                item = clone.GetComponent<T>();
                this.Items.Add(item);
            }

            //item.transform.SetAsLastSibling();
            this.usingItems.Add(item);
            this.OnCreatedItem?.Invoke(item);
            return item;
        }


        /// <summary>
        /// Collect and deactive an instance of <see cref="T"/>.
        /// </summary>
        public void Collect(T item)
        {
            try
            {
                if (item == null)
                    for (int i = this.usingItems.Count - 1; i >= 0; i--)
                        this.usingItems.RemoveAt(i);

                if (this.usingItems.Contains(item))
                {
                    item.gameObject.SetActive(false);

                    //restore parent if it has been overrided.
                    if (item.transform != this.cacheTransform)
                        item.transform.SetParent(this.cacheTransform);

                    this.usingItems.Remove(item);
                    this.pool.Push(item);
                    this.OnCollectedItem?.Invoke(item);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// Collect and deactive all instances.
        /// </summary>
        public void CollectAll()
        {
            try
            {
                foreach (var item in this.usingItems)
                {
                    item.gameObject.SetActive(false);

                    //restore parent if it has been overrided.
                    if (item.transform != this.cacheTransform)
                        item.transform.SetParent(this.cacheTransform);

                    //this.usingItems.Remove(item);
                    this.pool.Push(item);
                    this.OnCollectedItem?.Invoke(item);
                }
                this.usingItems.Clear();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
