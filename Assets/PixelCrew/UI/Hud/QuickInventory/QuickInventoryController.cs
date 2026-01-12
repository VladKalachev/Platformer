using System;
using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.UI.Hud.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private GameObject _prefab;

        // private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameSession _session;
        
        private void Start()
        {
           _session = FindObjectOfType<GameSession>();
           // subscribe

           Rebuild();
        }

        private void Rebuild()
        {
           
        }
    }
}