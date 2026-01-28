using System;
using System.Collections.Generic;
using PixelCrew.Model.Data;
using PixelCrew.Utils.Disposables;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession: MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;

        private PlayerData _save;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public QuickInventoryModel QuickInventory { get; private set; }
        
        private readonly List<string> _checkpoints = new List<string>();

        private void Awake()
        {
            LoadHud();
            
            if (IsSessionExit())
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this);
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(_data);
            _trash.Retain(QuickInventory);
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }

        private bool IsSessionExit()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                {
                    return true;
                }
            }
            return false;
        }

        public void Save()
        {
            _save = _data.Clone();
        }

        public void LoadLastSave()
        {
            _data =  _save.Clone();
            _trash.Dispose();
            InitModels();
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }

        public bool IsChecked(string id)
        {
           return _checkpoints.Contains(id);
        }
    }
}