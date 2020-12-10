
namespace Net.HungryBug.Core.Attribute
{
    using System;
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ValueDropdownAttribute : System.Attribute
    {
        public ValueDropdownAttribute(string source, string target, string onValueChanged = null) { }
    }
}

#if UNITY_EDITOR
namespace Net.HungryBug.Core.Attribute.Editor
{
    using System;
    using System.Linq;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// 
    /// </summary>
    [CustomPropertyDrawer(typeof(ValueDropdownList))]
    public class ValueDropdownListAttributeDrawer : PropertyDrawer
    {
        private int selecIndex = -1;
        private int prevIndex = -1;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string[] values = null;
            ValueDropdownItem[] list = null;
            string bindingFieldName = null;
            string onChangeMethod = null;

            MemberInfo sourceMember = property.serializedObject.targetObject.GetType().DeepGetField(property.name);
            if (sourceMember == null)
                sourceMember = property.serializedObject.targetObject.GetType().DeepGetProperty(property.name);

            if (sourceMember.CustomAttributes != null && sourceMember.CustomAttributes.Count() > 0)
            {
                var attr =
                    sourceMember
                   .CustomAttributes
                   .Where(x => x.AttributeType == typeof(ValueDropdownAttribute))
                   .FirstOrDefault();

                if (attr != null)
                {
                    string sourceMethod = attr.ConstructorArguments[0].Value as string;
                    bindingFieldName = attr.ConstructorArguments[1].Value as string;
                    onChangeMethod = attr.ConstructorArguments[2].Value as string;

                    var items = property.serializedObject
                        .targetObject
                        .GetType()
                        .DeepGetMethod(sourceMethod)
                        .Invoke(property.serializedObject.targetObject, null);

                    list = items != null ? items as ValueDropdownItem[] : null;

                    if (list != null && list.Length > 0)
                    {
                        values = new string[list.Length];
                        for (int i = 0; i < list.Length; i++)
                        {
                            values[i] = list[i].Label;
                        }
                    }
                }
            }

            if (values != null && values.Length > 0)
            {
                MemberInfo targetMember = property.serializedObject.targetObject.GetType().DeepGetField(bindingFieldName);
                if (targetMember == null)
                    targetMember = property.serializedObject.targetObject.GetType().DeepGetProperty(bindingFieldName);

                //Select prev value.
                if (selecIndex == -1)
                {
                    var currentValue = targetMember.GetValue(property.serializedObject.targetObject);
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (list[i].Value.Equals(currentValue))
                        {
                            selecIndex = i;
                            break;
                        }
                    }

                    if (selecIndex == -1)
                    {
                        selecIndex = 0;
                    }

                    prevIndex = selecIndex;
                }

                selecIndex = EditorGUI.Popup(position, label.text, selecIndex, values);

                if (prevIndex != selecIndex)
                {
                    targetMember.SetValue(property.serializedObject.targetObject, list[selecIndex].Value);
                    prevIndex = selecIndex;

                    //raise on change.
                    if (!string.IsNullOrEmpty(onChangeMethod))
                    {
                        property.serializedObject
                        .targetObject
                        .GetType()
                        .DeepGetMethod(onChangeMethod)
                        .Invoke(property.serializedObject.targetObject, new object[] { list[selecIndex].Value });
                    }

                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                    AssetDatabase.SaveAssets();
                }
                else
                {
                    var currentValue = targetMember.GetValue(property.serializedObject.targetObject);
                    if (currentValue == null || !currentValue.Equals(list[selecIndex].Value))
                    {
                        targetMember.SetValue(property.serializedObject.targetObject, list[selecIndex].Value);
                        EditorUtility.SetDirty(property.serializedObject.targetObject);
                        AssetDatabase.SaveAssets();
                    }
                }
            }
            else
            {
                selecIndex = EditorGUI.Popup(position, label.text, selecIndex, new string[] { });
            }
        }
    }

    public static class EditorExtension
    {
        public static MethodInfo DeepGetMethod(this Type type, string methodName)
        {
            MethodInfo result = null;
            while (type != null)
            {
                result = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (result != null)
                    return result;

                type = type.BaseType;
            }

            return result;
        }

        public static FieldInfo DeepGetField(this Type type, string fieldName)
        {
            FieldInfo result = null;
            while (type != null)
            {
                result = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (result != null)
                    return result;

                type = type.BaseType;
            }

            return result;
        }

        public static PropertyInfo DeepGetProperty(this Type type, string propertyName)
        {
            PropertyInfo result = null;
            while (type != null)
            {
                result = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (result != null)
                    return result;

                type = type.BaseType;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SetValue(this MemberInfo self, object target, object value)
        {
            if (self is FieldInfo)
                ((FieldInfo)self).SetValue(target, value);
            else if (self is PropertyInfo)
                ((PropertyInfo)self).SetValue(target, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static object GetValue(this MemberInfo self, object target)
        {
            if (self is FieldInfo)
                return ((FieldInfo)self).GetValue(target);
            else if (self is PropertyInfo)
                return ((PropertyInfo)self).GetValue(target);

            return null;
        }
    }

}
#endif