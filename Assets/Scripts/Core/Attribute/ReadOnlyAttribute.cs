namespace Net.HungryBug.Core.Attribute
{
	using UnityEngine;
    public class ReadOnlyAttribute : PropertyAttribute { }
}


#if UNITY_EDITOR
namespace Net.HungryBug.Core.Attribute.Editor
{
	using UnityEngine;
	using UnityEditor;
	using Net.HungryBug.Core.Attribute;

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}

#endif