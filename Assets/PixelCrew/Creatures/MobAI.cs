using System;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class MobAI: MonoBehaviour
    {
        [SerializeField] private LayoutCheck _vision;
        [SerializeField] private LayoutCheck _canAttack;

        private Coroutine _current;

        private void Start()
        {
            
        }
    }
}