using Attributes;
using Services;
using TMPro;
using UnityEngine;

namespace LocalizationSystem
{
    public class LocalizedTextMeshProUGUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        [SerializeField, LocalizationTerm] private string _term;
        
        private LocalizationService _localizationService;

        private void Start()
        {
            _localizationService = ServiceLocator.Instance.GetService<LocalizationService>();
            _textMeshProUGUI.text = _localizationService.GetTermValue(_term);
            
            _localizationService.OnLanguageChanged += LanguageHandler;
        }
        
        private void LanguageHandler()
        {
            _textMeshProUGUI.text = _localizationService.GetTermValue(_term);
        }

        private void OnDestroy()
        {
            _localizationService.OnLanguageChanged -= LanguageHandler;
        }
    }
}