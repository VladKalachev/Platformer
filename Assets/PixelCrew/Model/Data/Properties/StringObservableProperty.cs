using System;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    [Serializable]
    public class StringObservableProperty : PrefsPersistentProperty<string>
    {
        public StringObservableProperty(string defaultValue, string key) : base(defaultValue, key)
        {
            Init();
        }

        protected override void Write(string value)
        {
            PlayerPrefs.SetString(Key, value);
        }

        protected override string Read(string defaultValue)
        {
            return PlayerPrefs.GetString(Key, defaultValue);
        }
    }
}