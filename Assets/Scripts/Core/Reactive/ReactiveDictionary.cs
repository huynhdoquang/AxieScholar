using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.HungryBug.Core.Reactive
{
    public class DictionaryUpdateEventArgs<TKey, TValue> : EventArgs
    {
        public readonly TKey Key;
        public readonly TValue NewValue;
        public readonly TValue OldValue;

        public DictionaryUpdateEventArgs(TKey key, TValue newValue, TValue oldValue)
        {
            this.Key = key;
            this.NewValue = newValue;
            this.OldValue = oldValue;
        }
    }

    public class ReactiveDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public event Action OnFlushed;
        public event Action<TKey, TValue> OnItemRemoved;
        public event Action<TKey, TValue> OnItemAdded;
        public event EventHandler<DictionaryUpdateEventArgs<TKey, TValue>> OnItemEdited;

        public new TValue this[TKey key]
        {
            get
            {
                return base[key];
            }
            set
            {
                this.AddOrEdit(key, value);
            }
        }

        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            this.OnItemAdded?.Invoke(key, value);
        }


        public void AddOrEdit(TKey key, TValue value)
        {
            if (this.ContainsKey(key))
            {
                var oldValue = base[key];
                base[key] = value;
                this.OnItemEdited?.Invoke(this, new DictionaryUpdateEventArgs<TKey, TValue>(key, value, oldValue));
            }
            else
            {
                base.Add(key, value);
                this.OnItemAdded?.Invoke(key, value);
            }
        }

        public new bool Remove(TKey key)
        {
            if (this.TryGetValue(key, out var removeItem))
            {
                var result = base.Remove(key);

                this.OnItemRemoved?.Invoke(key, removeItem);
                return result;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Clean collection and notify for each item removed.
        /// </summary>
        public new void Clear()
        {
            var oldData = this.ToArray();
            base.Clear();

            //invoke all item removed.
            foreach (var old in oldData)
            {
                this.OnItemRemoved?.Invoke(old.Key, old.Value);
            }
        }

        /// <summary>
        /// Clean <see cref="ReactiveDictionary{TKey, TValue}"/> without notify for every removed items.
        /// </summary>
        public void CleanAndBulkNotify()
        {
            base.Clear();
            this.OnFlushed?.Invoke();
        }
    }
}
