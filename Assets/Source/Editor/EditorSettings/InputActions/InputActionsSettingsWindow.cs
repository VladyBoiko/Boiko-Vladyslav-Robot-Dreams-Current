using UnityEditor;
using UnityEngine;

namespace Source.Editor.EditorSettings.InputActions
{
    public class InputActionsSettingsWindow : EditorWindow
    {
        [MenuItem("Tools/Editor Settings/Input Actions Settings")]

        public static void ShowWindow()
        {
            InputActionsSettingsWindow window = GetWindow<InputActionsSettingsWindow>();
            window.titleContent = new GUIContent("Input Actions Settings");
            window.Show();
        }
        
        private UnityEditor.Editor _editor;

        private void OnEnable()
        {
            _editor = UnityEditor.Editor.CreateEditor(EditorInputActionsDataSettings.instance);
        }
        
        private void OnGUI()
        {
            if(_editor == null)
                return;
            
            EditorGUI.BeginChangeCheck();
            _editor.DrawDefaultInspector();
            
            if(EditorGUI.EndChangeCheck())
                EditorInputActionsDataSettings.instance.Save();
        }
    }
}