using PixelCrew.Components.GoBased;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class CheckPointComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private UnityEvent _setChecked;
        [SerializeField] private UnityEvent _setUnchecked;
        [SerializeField] private SpawnComponent _heroSpawn;
        
    }
}