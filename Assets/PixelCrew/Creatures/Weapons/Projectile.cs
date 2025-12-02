using System;
using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class Projectile: MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Rigidbody2D _rigidbody;
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var position = _rigidbody.position;
            position.x += _speed;
            _rigidbody.MovePosition(position);
        }
    }
}