using System;
using PixelCrew.Components.GoBased;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    [RequireComponent(typeof(SpawnComponent))]
    public class CheckPointComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private UnityEvent _setChecked;
        [SerializeField] private UnityEvent _setUnchecked;

        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            if (_session.IsChecked(_id))
            {
                _setChecked?.Invoke();
            }
            else
            {
                _setUnchecked?.Invoke();
            }
        }
    }
}