using System;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.Windows.MainMenu
{
    public class MainMenuWindow : AnimatedWindow
    {
        private Action _closeAction;
        
        public void OnShowSettings()
        {
           var window = Resources.Load<GameObject>("UI/SettingsWindow");
           var canvas = FindObjectOfType<Canvas>();
           Instantiate(window, canvas.transform);
        }
        
        public void OnStartGame()
        {
            _closeAction = () =>
            {
                SceneManager.LoadScene("Level1");
            };
            Close();
        }

        public void OnLanguages()
        {
            WindowUtils.CreateWindow("UI/LocalizationWindow");
        }
        
        public void OnExit()
        {
            _closeAction = () =>
            {
                Application.Quit();
            
#if  UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };
            Close();
           
        }

        public override void OnCloseAnimationComplete()
        {
            _closeAction?.Invoke();
            base.OnCloseAnimationComplete();
        }
    }
}