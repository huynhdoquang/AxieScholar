using System.Collections.Generic;

namespace Net.HungryBug.Core.Attribute
{
    [System.Serializable]
    public class ValueDropdownList
    {
        public List<ValueDropdownItem> Items { get; private set; }

        /// <summary>
        /// Construct a dropdown list.
        /// </summary>
        public ValueDropdownList() { this.Items = new List<ValueDropdownItem>(); }
    }
}
