using Data.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Source.Editor.EditorSettings.Localization
{
    [FilePath("Data/LocalizationSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class LocalizationEditorSettings : ScriptableSingleton<LocalizationEditorSettings>
    {
        [SerializeField] private LocalizationData _localizationData;

        public LocalizationData LocalizationData
        {
            get => _localizationData;
            set
            {
                if(value == _localizationData) return;
                _localizationData = value;
                Save(true);
            }
        }
    }
}