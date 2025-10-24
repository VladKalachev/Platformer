using System;
using UnityEngine;

namespace  PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private LayerMask _groundLayer;
        
        private Rigidbody2D _rigidbody;
        
        private Vector2 _direction;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;  
        }

        private void FixedUpdate()
        {
            _rigidbody.linearVelocity = new Vector2(_direction.x * _speed, _rigidbody.linearVelocity.y);
            
            var isJumping = _direction.y > 0;
            if (isJumping)
            {
                _rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            }
        }
    }
};