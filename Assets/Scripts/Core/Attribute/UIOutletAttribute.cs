using System;

namespace Net.HungryBug.Core.Attribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class UIOutletAttribute : System.Attribute
    {
        public UIOutletAttribute(string targetName, bool optional = false) { }
    }
}
