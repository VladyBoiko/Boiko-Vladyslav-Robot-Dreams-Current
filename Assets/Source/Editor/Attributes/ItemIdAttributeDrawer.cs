using Attributes;
using Source.Editor.EditorSettings.Inventory;
using UnityEditor;
using UnityEngine;

namespace Source.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(ItemIdAttribute))]
    public class ItemIdAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                string[] options = EditorItemSettings.instance.ItemIds();
                int selectedIndex = Mathf.Max(0, System.Array.IndexOf(options, property.stringValue));
            
                selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, options);
                property.stringValue = options[selectedIndex];
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}