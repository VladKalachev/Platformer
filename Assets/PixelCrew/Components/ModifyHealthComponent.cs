using UnityEngine;

namespace PixelCrew.Components
{
    public class ModifyHealthComponent : MonoBehaviour
    {
       [SerializeField] private int _hpDelta;

       public void Apply(GameObject target)
       { 
           var healthComponent = target.GetComponent<HealtComponent>();
           if (healthComponent != null)
           {
               healthComponent.ModifyHealth(_hpDelta);     
           }
          
       }
    }    
}

