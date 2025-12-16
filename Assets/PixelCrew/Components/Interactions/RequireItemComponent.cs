using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Interactions
{
    public class RequireItemComponent: MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private int _count;
        [SerializeField] private bool _removeAfterUse;

        [SerializeField] private UnityEvent _onSuccess;
        [SerializeField] private UnityEvent _onFail;
        
        public void Check()
        {
            var session = FindObjectOfType<GameSession>();
            var numItems = session.Data.Inventory.Count(_id);
            if (numItems >= _count)
            {
                if (_removeAfterUse)
                {
                    session.Data.Inventory.Remove(_id, _count);
                }
                
                _onSuccess?.Invoke();
            }
            else
            {
                _onFail?.Invoke();
            }
            
        }
    }
}