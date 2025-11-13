using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components
{
    public class ExitLevelCompoment: MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        
        public void Exit()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}