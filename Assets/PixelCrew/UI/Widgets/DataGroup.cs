using System.Collections.Generic;
using PixelCrew.UI.Windows.Localization;

namespace PixelCrew.UI.Widgets
{
    public class DataGroup
    {
        public void SetData(List<LocaleInfo> composeData)
        {
           
        }
        
        public interface IItemRender<in TDataType>
        {
            void SetData(TDataType data, int index);
        }
    }
}