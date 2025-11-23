using System;
using System.Linq;
using PixelCrew.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew
{
    public class CheckCircleOverlap: MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string[] _tags;
        [SerializeField] private OnOverlapEvent _onOverlap;
        private Collider2D[] _ineractionResult = new Collider2D[10];
        
        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }

        public void Check()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position, 
                _radius, 
                _ineractionResult,
                _mask);

            for (int i = 0; i < size; i++)
            {
                var overlapResult = _ineractionResult[i];
                var isInTag = _tags.Any(tag => overlapResult.CompareTag(tag));

                if (isInTag)
                {
                    _onOverlap?.Invoke(overlapResult.gameObject);     
                }
            }
        }

        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject>
        {
            
        }
    }
}