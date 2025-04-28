using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Editor.EditorSettings.InputActions
{
    [FilePath("Data/EditorInputActionsDataSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class EditorInputActionsDataSettings : ScriptableSingleton<EditorInputActionsDataSettings>
    {
        [SerializeField] private InputActionAsset _inputAsset;

        public InputActionAsset InputAsset => _inputAsset;

        public string[] GetActionMapNames()
        {
            if (_inputAsset == null) return new []{"None"};
            return _inputAsset.actionMaps.Select(map => map.name).ToArray();
        }

        public string[] GetActionNames()
        {
            return _inputAsset.actionMaps
                .SelectMany(map => map.actions)
                .Select(action => action.name)
                .ToArray();
        }

        public void Save()
        {
            Save(true);
        }
    }
}