using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PixelCrew.UI.Hud.Dialogs
{
    public class OptionItemWidget : MonoBehaviour
    {
        [SerializeField] private Text _lable;
        [SerializeField] private SelectOption _onSelect;
        
        private OptionData _data;

        public void SetData(OptionData data)
        {
            _data = data;
            _lable.text = data.Text;
        }

        public void OnSelect()
        {
            _onSelect.Invoke(_data);
        }

        [Serializable]
        public class SelectOption : UnityEvent<OptionData>
        {
            
        }
    }
}