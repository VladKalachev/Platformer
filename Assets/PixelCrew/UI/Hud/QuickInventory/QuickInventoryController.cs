using System;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.Hud.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private GameObject _prefab;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameSession _session;
        private InventoryData.InventoryItemData[] _inventory;
        
        private void Start()
        {
           _session = FindObjectOfType<GameSession>();
           _inventory = _session.Data.Inventory.GetAll();

           Rebuild();
        }

        private void Rebuild()
        {
           
        }
    }
}