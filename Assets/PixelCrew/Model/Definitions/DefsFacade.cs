using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade: ScriptableObject
    {
        [SerializeField] private InventoryItemDef _items;
        [SerializeField] private PlayerDef _player;
        
        public InventoryItemDef Items => _items;
        
        public PlayerDef Player => _player;
        
        private static DefsFacade _instance;
        public static DefsFacade I => _instance == null ? LoadingDefs() : _instance;

        private static DefsFacade LoadingDefs()
        {
           return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }
    }
}