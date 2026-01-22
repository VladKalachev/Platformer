using System;
using PixelCrew.Model.Data.Properties;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Localization
{
    public class LocalizationManager
    {
        public readonly static LocalizationManager I;

        private StringObservableProperty _currentLocale = new StringObservableProperty("en", "localization/current");

        private LocaleDef _localeDef;
        public event Action OnLocaleChange;
        static LocalizationManager()
        {
            I = new LocalizationManager();
        }

        public LocalizationManager()
        {
            LoadLocale(_currentLocale.Value);
        }

        private void LoadLocale(string localeToLoad)
        {
            _localeDef = Resources.Load<LocaleDef>($"Locales/{localeToLoad}");
            OnLocaleChange?.Invoke();
        }
    }
}