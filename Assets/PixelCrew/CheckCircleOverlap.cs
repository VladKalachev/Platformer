using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PixelCrew
{
    public class CheckCircleOverlap: MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        private Collider2D[] _ineractionResult = new Collider2D[5];
        
        public GameObject[] GetObjectsInRange()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _ineractionResult);

            var overlaps = new List<GameObject>();

            for (int i = 0; i < size; i++)
            {
                overlaps.Add(_ineractionResult[i].gameObject);
            }

            return overlaps.ToArray();
        }

        private void OnDrawGizmosSelected()
        {
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }
    }
}