using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LocalizationData", menuName = "Data/LocalizationData", order = 0)]
    public class LocalizationData : ScriptableObject
    {
        [Serializable]
        public class LanguageEntry
        {
            public string[] termValues;
        }
        
        [SerializeField, HideInInspector] LanguageEntry[] _languageEntries;
        
        [SerializeField, HideInInspector] private List<string> _terms;
        [SerializeField, HideInInspector] private List<string> _languages;
        
        public List<string> Terms => _terms;
        public List<string> Languages => _languages;
        public LanguageEntry[] LanguageEntries => _languageEntries;

    }
}