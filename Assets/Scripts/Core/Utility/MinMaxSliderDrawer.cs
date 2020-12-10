#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
class MinMaxSliderDrawer : PropertyDrawer
{
    const string kVectorMinName = "x";
    const string kVectorMaxName = "y";
    const float kFloatFieldWidth = 65f;
    const float padding = 0f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.Vector2)
        {
            Vector2 range = property.vector2Value;
            float min = range.x;
            float max = range.y;

            MinMaxSliderAttribute attr = attribute as MinMaxSliderAttribute;

            EditorGUI.PrefixLabel(position, label);

            Rect sliderPos = position;
            sliderPos.x += EditorGUIUtility.labelWidth + kFloatFieldWidth + padding + 5;
            sliderPos.width -= EditorGUIUtility.labelWidth + kFloatFieldWidth * 2 + padding + 10;

            EditorGUI.BeginChangeCheck();
            EditorGUI.MinMaxSlider(sliderPos, ref min, ref max, attr.min, attr.max);
            if (EditorGUI.EndChangeCheck())
            {
                if (attr.isInt)
                {
                    min = (int)min;
                    max = (int)max;
                }

                range.x = (float)System.Math.Floor(min * 1000) / 1000f;
                range.y = (float)System.Math.Floor(max * 1000) / 1000f; ;
                property.vector2Value = range;
            }

            Rect minPos = position;
            minPos.x += EditorGUIUtility.labelWidth + padding;
            minPos.width = kFloatFieldWidth;

            EditorGUI.showMixedValue = property.FindPropertyRelative(kVectorMinName).hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();
            min = EditorGUI.FloatField(minPos, min);
            if (EditorGUI.EndChangeCheck())
            {
                if (attr.isInt)
                {
                    min = (int)min;
                    max = (int)max;
                }

                min = Mathf.Clamp(min, attr.min, max);
                range.x = min;
                property.vector2Value = range;
            }

            Rect maxPos = position;
            maxPos.x += maxPos.width - kFloatFieldWidth;
            maxPos.width = kFloatFieldWidth;

            EditorGUI.showMixedValue = property.FindPropertyRelative(kVectorMaxName).hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();
            max = EditorGUI.FloatField(maxPos, max);
            if (EditorGUI.EndChangeCheck())
            {
                if (attr.isInt)
                {
                    min = (int)min;
                    max = (int)max;
                }

                max = Mathf.Clamp(max, min, attr.max);
                range.y = max;
                property.vector2Value = range;
            }

            EditorGUI.showMixedValue = false;
        }
        else
        {
            EditorGUI.LabelField(position, label, new GUIContent("Vector2 support only"));
        }
    }
}

#endif
