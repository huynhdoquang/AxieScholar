using System;
using System.Collections.Generic;

namespace Net.HungryBug.Core.Reactive
{
    public class ReactiveProperty<T>
    {
        private readonly IEqualityComparer<T> equalityComparer = UnityEqualityComparer.GetDefault<T>();

        /// <summary>
        /// Create a new <see cref="ReactiveProperty{T}"/> with default value.
        /// </summary>
        /// <param name="defaultValue"></param>
        public ReactiveProperty(T defaultValue)
        {
            this.value = defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public ReactiveProperty()
        {

        }

        /// <summary>
        /// Invoke whenever the property value has been changed.
        /// </summary>
        public event Action<T> OnChange;

        private T value;
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public T Value
        {
            get { return this.value; }
            set
            {
                if (this.equalityComparer.Equals(this.value, value))
                    return;

                this.value = value;
                this.OnChange?.Invoke(this.value);
            }
        }

        /// <summary>
        /// Sets value and force notify.
        /// </summary>
        public void SetValueAndForceNotify(T value)
        {
            this.value = value;
            this.OnChange?.Invoke(this.value);
        }

        /// <summary>
        /// Force nofity.
        /// </summary>
        public void ForceNofity()
        {
            this.OnChange?.Invoke(this.value);
        }
    }
}
