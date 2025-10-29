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
        private bool _isGrounded;
        private bool _allowDoubleJump;
        
        private static readonly int IsGround =  Animator.StringToHash("is-ground");
        private static readonly int IsVerticalVelocity =  Animator.StringToHash("vertical-velocity");
        private static readonly int IsRunning =  Animator.StringToHash("is-running");

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator =  GetComponent<Animator>();
            _sprite =  GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            _isGrounded = IsGrounded();
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
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            _rigidbody.linearVelocity = new Vector2(xVelocity, yVelocity);
            
            _animator.SetBool(IsGround, _isGrounded);
            _animator.SetBool(IsRunning, _direction.x != 0);
            _animator.SetFloat(IsVerticalVelocity, _rigidbody.linearVelocity.y);
            
            UpdateSpriteDirection();
        }

        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.linearVelocity.y;
            var isJumpPressing = _direction.y > 0;
           
            if (_isGrounded) _allowDoubleJump = true;
            
            if (isJumpPressing) 
            {
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.linearVelocity.y > 0)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.linearVelocity.y <= 0.001f;
            
            if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                yVelocity += _jumpSpeed;
            } else if (_allowDoubleJump)
            {
                yVelocity = _jumpSpeed;
                _allowDoubleJump = false;
            }
            
            return yVelocity;
        }

        private bool IsGrounded()
        {
            return _groundCheck.IsTouchingLayer;
        }
        
    }
};