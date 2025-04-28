using Attributes;
using Source.Editor.EditorSettings.Scene;
using UnityEditor;
using UnityEngine;

namespace Source.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(SceneDropdownAttribute))]
    public class SceneDropdownAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                string[] options = EditorSceneSettings.instance.SceneNames();
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