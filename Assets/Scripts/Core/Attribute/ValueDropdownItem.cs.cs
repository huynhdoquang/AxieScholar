namespace Net.HungryBug.Core.Attribute
{
    [System.Serializable]
    public class ValueDropdownItem
    {
        public object Value;
        public string Label;

        public ValueDropdownItem() { }

        public ValueDropdownItem(string label, object value)
        {
            this.Value = value;
            this.Label = label;
        }
    }
}
