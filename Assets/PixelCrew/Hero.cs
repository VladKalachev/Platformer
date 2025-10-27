using System;
using UnityEngine;

namespace  PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        
        [SerializeField] private LayoutCheck _groundCheck;
        
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        
        private Animator _animator;
        private SpriteRenderer _sprite;
        
        private static readonly int IsGround =  Animator.StringToHash("is-ground");
        private static readonly int IsVerticalVelocity =  Animator.StringToHash("vertical-velocity");
        private static readonly int IsRunning =  Animator.StringToHash("is-running");

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator =  GetComponent<Animator>();
            _sprite =  GetComponent<SpriteRenderer>();
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;  
        }

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                _sprite.flipX = false;
            } else if (_direction.x < 0)
            {
                _sprite.flipX = true;
            }
        }

        private void FixedUpdate()
        {
            _rigidbody.linearVelocity = new Vector2(_direction.x * _speed, _rigidbody.linearVelocity.y);
            
            var isJumping = _direction.y > 0;
            var isGrounded = IsGrounded();
            if (isJumping)
            {
                if (isGrounded && _rigidbody.linearVelocity.y <= 0)
                {
                    _rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);    
                }
            } else if (_rigidbody.linearVelocity.y > 0)
            {
                _rigidbody.linearVelocity =
                    new Vector2(_rigidbody.linearVelocity.x, _rigidbody.linearVelocity.y * 0.5f);
            }
            
            _animator.SetBool(IsGround, isGrounded);
            _animator.SetBool(IsRunning, _direction.x != 0);
            _animator.SetFloat(IsVerticalVelocity, _rigidbody.linearVelocity.y);
            
            UpdateSpriteDirection();
        }

        private bool IsGrounded()
        {
            return _groundCheck.IsTouchingLayer;
        }
        
    }
};