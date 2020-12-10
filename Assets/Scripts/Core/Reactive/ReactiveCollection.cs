using System;
using System.Collections.Generic;

namespace Net.HungryBug.Core.Reactive
{
    public class CollectionUpdateEventArgs<T> : EventArgs
    {
        public readonly T NewValue;
        public readonly T OldValue;
        public readonly int Index;

        public CollectionUpdateEventArgs(int index, T newValue, T oldValue)
        {
            this.Index = index;
            this.NewValue = newValue;
            this.OldValue = oldValue;
        }
    }

    public class ReactiveCollection<T> : List<T>
    {
        public event Action<T> OnItemRemoved;
        public event Action<T> OnItemAdded;
        public event EventHandler<CollectionUpdateEventArgs<T>> OnItemEdited;

        public new T this[int index]
        {
            get { return base[index]; }
            set { this.Edit(value, index); }
        }

        public new void Add(T value)
        {
            base.Add(value);
            this.OnItemAdded?.Invoke(value);
        }

        public new void AddRange(IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                this.Add(value);
            }
        }

        public new bool Remove(T value)
        {
            if (base.Remove(value))
            {
                this.OnItemRemoved?.Invoke(value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public new void RemoveAt(int index)
        {
            if (index < this.Count)
            {
                var item = base[index];
                base.RemoveAt(index);
                this.OnItemRemoved?.Invoke(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Edit(T newValue, int index)
        {
            if (index < this.Count && index >= 0)
            {
                var oldValue = base[index];
                base[index] = newValue;
                this.OnItemEdited?.Invoke(this, new CollectionUpdateEventArgs<T>(index, newValue, oldValue));
            }
        }

        public new void Clear()
        {
            var oldData = base.ToArray();
            base.Clear();

            //invoke all item removed.
            foreach (var old in oldData)
            {
                this.OnItemRemoved?.Invoke(old);
            }
        }
    }
}
