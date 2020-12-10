using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Net.HungryBug.Core.Reactive
{
    public class ReactiveHashSet<T> : IEnumerable<T>
    {
        private readonly HashSet<T> Data = new HashSet<T>();

        public event Action<T> OnItemRemoved;
        public event Action<T> OnItemAdded;

        public int Count { get { return this.Data.Count; } }

        public void Add(T value)
        {
            if(this.Data.Add(value))
            {
                this.OnItemAdded?.Invoke(value);
            }
        }

        public void AddRange(IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                this.Add(value);
            }
        }

        public bool Remove(T value)
        {
            if (this.Data.Remove(value))
            {
                this.OnItemRemoved?.Invoke(value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Clear()
        {
            var oldData = this.Data.ToArray();
            this.Data.Clear();

            //invoke all item removed.
            foreach (var old in oldData)
            {
                this.OnItemRemoved?.Invoke(old);
            }
        }

        public bool Contains(T value) { return this.Data.Contains(value); }
        public IEnumerator<T> GetEnumerator() { return this.Data.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }
    }
}
