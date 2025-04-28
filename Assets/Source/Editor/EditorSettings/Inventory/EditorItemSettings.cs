using Data.ScriptableObjects.Inventory;
using UnityEditor;
using UnityEngine;

namespace Source.Editor.EditorSettings.Inventory
{
    [FilePath("Data/EditorItemSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class EditorItemSettings : ScriptableSingleton<EditorItemSettings>
    {
        [SerializeField] private ItemLibrary _itemLibrary;

        public ItemLibrary ItemLibrary
        {
            get => _itemLibrary;
            set
            {
                if (_itemLibrary == value)
                    return;
                _itemLibrary = value;
                EditorUtility.SetDirty(this);
                Save(true);
            }
        }
        
        public string[] ItemIds()
        {
            return _itemLibrary?.ItemIds ?? new []{"None"};
        }
    }
}