using Data.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Source.Editor.EditorSettings.Localization
{
    public class LocalizationSettingsWindow : EditorWindow
    {
        [MenuItem("Tools/Localization Settings")]
        private static void ShowWindow()
        {
            var window = GetWindow<LocalizationSettingsWindow>();
            window.titleContent = new GUIContent("Localization Settings");
            window.Show();
        }

        private void OnGUI()
        {
            LocalizationData localizationData = LocalizationEditorSettings.instance.LocalizationData;
            EditorGUI.BeginChangeCheck();
            
            localizationData = EditorGUILayout.ObjectField(localizationData, typeof(LocalizationData), 
                false) as LocalizationData;
            
            if(EditorGUI.EndChangeCheck())
                LocalizationEditorSettings.instance.LocalizationData = localizationData;
        }
    }
}