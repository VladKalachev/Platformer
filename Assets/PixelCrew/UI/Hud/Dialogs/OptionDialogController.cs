using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PixelCrew.UI.Hud.Dialogs
{
    public class OptionDialogController : MonoBehaviour
    {
        [SerializeField] private Text _contentText;
        [SerializeField] private Transform _container;
        [SerializeField] private OptionItemWidget _prefab;
        
        public void Show(OptionDialogData data)
        {
            _contentText.text = data.DialogText;
        }
    }
    
    [Serializable]
    public class OptionDialogData
    {
        public string DialogText;
        public OptionData[] Options;
    }
    
    [Serializable]
    public class OptionData
    {
        public string Text;
        public UnityEvent OnSelect;
    }
}