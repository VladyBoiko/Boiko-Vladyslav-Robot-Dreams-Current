using System.Collections.Generic;
using Attributes;
using Source.Editor.EditorSettings.Localization;
using UnityEditor;
using UnityEngine;

namespace Source.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(LanguageKeyAttribute))]
    public class LanguageKeyAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                List<string> options = LocalizationEditorSettings.instance.LocalizationData.Languages;
                
                int selectedIndex = Mathf.Max(0, options.IndexOf(property.stringValue));
                
                selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, options.ToArray());
                property.stringValue = options[selectedIndex];
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}