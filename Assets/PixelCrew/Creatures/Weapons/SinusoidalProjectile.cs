using System;
using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class SinusoidalProjectile: BaseProjectile
    {
        private float _originalY;
        
        protected override void Start()
        {
            base.Start();
        }

        private void FixedUpdate()
        {
            var position = Rigidbody.position;
            position.x = Direction * _speed;
            position.y = _originalY + Mathf.Sin(Time.time);
            Rigidbody.MovePosition(position);
        }
    }
}