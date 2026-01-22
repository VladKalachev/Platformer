namespace PixelCrew.Model.Definitions.Localization
{
    public class LocalizationManager
    {
        public readonly static LocalizationManager I;

        static LocalizationManager()
        {
            I = new LocalizationManager();
        }

        public LocalizationManager()
        {
            Initialize();
        }

        private void Initialize()
        {
            
        }
    }
}