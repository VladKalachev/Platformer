using System;
using System.Collections.Generic;
using PixelCrew.Model.Data.Properties;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Localization
{
    public class LocalizationManager
    {
        public readonly static LocalizationManager I;

        private StringObservableProperty _currentLocale = new StringObservableProperty("en", "localization/current");

        private Dictionary<string, string> _localization;
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
            var def = Resources.Load<LocaleDef>($"Locales/{localeToLoad}");
            _localization = def.GetData();
            OnLocaleChange?.Invoke();
        }

        public string Localize(string key)
        {
            if (_localization.TryGetValue(key, out string value))
            {
                return value;
            }

            return $"%%%{key}%%%%";
        }
    }
}