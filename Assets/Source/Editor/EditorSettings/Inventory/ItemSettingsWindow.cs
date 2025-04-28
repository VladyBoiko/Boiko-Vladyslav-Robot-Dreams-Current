using Data.ScriptableObjects.Inventory;
using UnityEditor;
using UnityEngine;

namespace Source.Editor.EditorSettings.Inventory
{
    public class ItemSettingsWindow : EditorWindow
    {
        [MenuItem("Tools/Editor Settings/Item Settings")]
        public static void ShowWindow()
        { 
            ItemSettingsWindow window = GetWindow<ItemSettingsWindow>();
            window.titleContent = new GUIContent("Item Settings");
            window.Show();
        }
        
        private void OnGUI()
        {
            ItemLibrary itemLibrary = EditorItemSettings.instance.ItemLibrary;
            EditorGUI.BeginChangeCheck();
            itemLibrary = EditorGUILayout.ObjectField(itemLibrary, typeof(ItemLibrary), false) as ItemLibrary;
            if (EditorGUI.EndChangeCheck())
                EditorItemSettings.instance.ItemLibrary = itemLibrary;
        }
    }
}