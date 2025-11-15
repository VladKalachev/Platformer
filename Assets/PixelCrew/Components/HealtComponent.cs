using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealtComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private HealthChangeEvent _onChange;
        
        public void ModifyHealth(int healthDelta)
        {
            _health += healthDelta;
            _onChange?.Invoke(_health);
            
            if (healthDelta < 0)
            {
                _onDamage?.Invoke();       
            }
            
            if (healthDelta > 0)
            {
                _onHeal?.Invoke();       
            }
            
            if (_health <= 0)
            {
                _onDie.Invoke();
            }
        }
        
        public class HealthChangeEvent: UnityEvent<int> {}
    }    
}

