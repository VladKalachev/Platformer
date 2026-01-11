namespace PixelCrew.Model.Data.Properties
{
    public class IntProperty : ObservableProperty<float>
    {
        public IntProperty(float defaultValue) : base(defaultValue)
        {
        }

        protected override void Write(float value)
        {
           _value = value;
        }

        protected override float Read(float defaultValue)
        {
           return _value;
        }
    }
}