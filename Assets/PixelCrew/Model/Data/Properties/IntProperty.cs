using System;

namespace PixelCrew.Model.Data.Properties
{
    [Serializable]
    public class IntProperty : ObservableProperty<int>
    {
        public IntProperty(int defaultValue) : base(defaultValue)
        {
        }

        protected override void Write(int value)
        { 
            _stored = value;
        }

        protected override int Read(int defaultValue)
        {
           return _stored;
        }
    }
}