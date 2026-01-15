using System;
using System.Collections.Generic;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.Hud.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItemWidget _prefab;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameSession _session;
        private InventoryData.InventoryItemData[] _inventory;
        private List<InventoryItemWidget> _createdItem = new List<InventoryItemWidget>();
        private void Start()
        {
           _session = FindObjectOfType<GameSession>();

           Rebuild();
        }

        private void Rebuild()
        {
            _inventory = _session.Data.Inventory.GetAll();

            for (int i = _createdItem.Count; i < _inventory.Length; i++)
            {
                var item = Instantiate(_prefab, _container);
                _createdItem.Add(item);
            }
            
            // update data amd activete
            for (var i = 0; i < _inventory.Length; i++)
            {
                _createdItem[i].SetData(_inventory[i], i);
                _createdItem[i].gameObject.SetActive(true);
            }

            // hide unused items
            for (var i = _inventory.Length; i < _inventory.Length; i++)
            {
                _createdItem[i].gameObject.SetActive(false);
            }
        }
    }
}