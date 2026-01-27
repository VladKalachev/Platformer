using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class DataGroup<TDataType, TItemType> where TItemType : MonoBehaviour, IItemRender<TDataType>
    {
        private List<TItemType> _createdItem = new List<TItemType>();
        
        private TItemType _prefab;
        private Transform _container;

        public DataGroup(TItemType prefab, Transform container)
        {
            _prefab = prefab;
            _container =  container;
        }
        
        public void SetData(IList<TDataType> data)
        {
           
            for (int i = _createdItem.Count; i < data.Count; i++)
            {
                var item = Object.Instantiate(_prefab, _container);
                _createdItem.Add(item);
            }
            
            // update data amd activete
            for (var i = 0; i < data.Count; i++)
            {
                _createdItem[i].SetData(data[i], i);
                _createdItem[i].gameObject.SetActive(true);
            }

            // hide unused items
            for (var i = data.Count; i < _createdItem.Count; i++)
            {
                _createdItem[i].gameObject.SetActive(false);
            }
        }
    }
    
    public interface IItemRender<in TDataType>
    {
        void SetData(TDataType data, int index);
    }
}