using UnityEngine;

namespace PixelCrew.Components
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _destroyObject;
        
        public void DestroySelf()
        {
            Destroy(_destroyObject);
        }
    }
    
}
