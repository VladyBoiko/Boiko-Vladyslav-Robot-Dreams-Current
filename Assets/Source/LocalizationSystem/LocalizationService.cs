using System;
using System.Collections.Generic;
using Data.ScriptableObjects;
using SaveSystem;
using Services;
using UnityEngine;

namespace LocalizationSystem
{
    public class LocalizationService : GlobalMonoServiceBase
    {
        public event Action OnLanguageChanged;
        
        [SerializeField] private LocalizationData _data;
        
        private string _currentLanguage;

        private Dictionary<string, Dictionary<string, string>> _localizationLookup = new();
        private Dictionary<string, string> _currentTermsLookup;
        
        private ISaveService _saveService;
        
        public override Type Type { get; } = typeof(LocalizationService);

        protected void Start()
        {
            _saveService = ServiceLocator.Instance.GetService<ISaveService>();
            
            string savedLanguage = _saveService.SaveData.localizationData.language;
            
            if (!_data.Languages.Contains(savedLanguage))
            {
                savedLanguage = _data.Languages[0];
                _saveService.SaveData.localizationData.language = savedLanguage;
            }
            
            for (int i = 0; i < _data.LanguageEntries.Length; ++i)
            {
                LocalizationData.LanguageEntry languageEntry = _data.LanguageEntries[i];
                
                Dictionary<string, string> termLookup = new Dictionary<string, string>();
                
                for (int j = 0; j < languageEntry.termValues.Length; ++j)
                {
                    termLookup.Add(_data.Terms[j], languageEntry.termValues[j]);
                }
                
                _localizationLookup.Add(_data.Languages[i], termLookup);
            }
            
            _currentLanguage = savedLanguage;
            _currentTermsLookup = _localizationLookup[savedLanguage];
        }
        
        public string GetCurrentLanguage() => _currentLanguage;
        
        public List<string> GetSupportedLanguages() => _data.Languages;
        
        public void SetLanguage(string language)
        {
            _currentLanguage = language;
            _currentTermsLookup = _localizationLookup[language];
            _saveService.SaveData.localizationData.language = _currentLanguage;
            OnLanguageChanged?.Invoke();
        }
        
        public string GetTermValue(string term)
        {
            return _currentTermsLookup[term];
        }
    }
}