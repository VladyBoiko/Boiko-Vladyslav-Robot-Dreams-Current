using Data.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Source.Editor.EditorSettings.Scene
{
    [FilePath("Data/EditorSceneSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class EditorSceneSettings : ScriptableSingleton<EditorSceneSettings>
    {
        [SerializeField] private SceneData _sceneData;

        public void Save()
        {
            Save(true);
        }

        public string[] SceneNames()
        {
            return _sceneData?.SceneNames ?? new []{"None"};
        }

        public string BootScene()
        {
            return _sceneData?.BootSceneName ?? "";
        }
    }
}