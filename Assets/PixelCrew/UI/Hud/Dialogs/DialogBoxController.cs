using System;
using System.Collections;
using PixelCrew.Model.Data;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Hud.Dialogs
{
    public class DialogBoxController : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private GameObject _container;
        [SerializeField] private Animator _animator;

        [Space] [SerializeField] private float _textSpeed = 0.09f;
        
        [Header("Sounds")] [SerializeField] private AudioClip _typing;
        [SerializeField] private AudioClip _open;
        [SerializeField] private AudioClip _close;

        private static readonly int IsOpen = Animator.StringToHash("IsOpen");
        
        private DialogData _data;
        private int _currentSentence;
        private AudioSource _sfxSource;
        
        private Coroutine _typingCoroutine;

        private void Start()
        {
            _sfxSource = AudioUtils.FindSfxSound();
        }

        public void ShowDialog(DialogData data)
        {
            _data = data;
            _currentSentence = 0;
            _text.text = string.Empty;
            
            _container.SetActive(true);
            _sfxSource.PlayOneShot(_open);
            _animator.SetBool(IsOpen, true);
        }

        private IEnumerator TypeDialogText()
        {
            _text.text = string.Empty;
            var sentences = _data.Sentences[_currentSentence];
            
            foreach (var letter in sentences)
            {
                _text.text += letter;
                _sfxSource.PlayOneShot(_typing);
                yield return new WaitForSeconds(_textSpeed);
            }
            
            _typingCoroutine = null;
        }

        private void OnStartDialogAnimation()
        {
            _typingCoroutine = StartCoroutine(TypeDialogText());
        }
        
        private void OnCloseDialogAnimation()
        {
            
        }
    }
}