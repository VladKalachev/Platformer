using System;
using PixelCrew.Model.Definitions.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Localization
{
    [RequireComponent(typeof(Text))]
    public class _localization : MonoBehaviour
    {
        [SerializeField] private string _key;
        [SerializeField] private bool _capitalize;

        private Text _text;
        private void Awake()
        {
            _text = GetComponent<Text>();
            
            LocalizationManager.I.OnLocaleChange += OnLocaleChanged;
            Localize();
        }
        
        private void OnLocaleChanged()
        {
            Localize();
        }
        
        private void Localize()
        {
            var localized = LocalizationManager.I.Localize(_key);
            _text.text = _capitalize ? localized.ToUpper() : localized;
        }
        
    }
}