using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    public abstract class PersistentProperty<TPropertyType>
    {
        [SerializeField] private TPropertyType _value;
        private TPropertyType _defaultValue;
        
        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);

        public event OnPropertyChanged OnChanged;

        public PersistentProperty(TPropertyType defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public TPropertyType Value
        {
            get => _value;
            set
            {
                var isEqual = _value.Equals(value);
                if (isEqual) return;
                
                var oldValue = _value;
                Write(value);
                    _value = value;
                    
                    OnChanged?.Invoke(oldValue, value);
            }
        }

        protected void Init()
        {
            _value = Read(_defaultValue);
        }
        
        protected abstract void Write(TPropertyType value);
        protected abstract TPropertyType Read(TPropertyType defaultValue);
    }
}