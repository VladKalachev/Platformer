using System;
using PixelCrew.Components;
using UnityEngine;

namespace  PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _damageJumpSpeed;
        [SerializeField] private LayoutCheck _groundLayer;
        [SerializeField] private float _ineractionRadius;
        [SerializeField] private LayerMask _interactionLayer;
        
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;
        
        private Collider2D[] _ineractionResult = new Collider2D[1];
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Animator _animator;
        private SpriteRenderer _sprite;
        private bool _isGrounded;
        private bool _allowDoubleJump;
        
        private static readonly int IsGround =  Animator.StringToHash("is-ground");
        private static readonly int IsVerticalVelocity =  Animator.StringToHash("vertical-velocity");
        private static readonly int IsRunning =  Animator.StringToHash("is-running");
        private static readonly int Hit =  Animator.StringToHash("hit");
        
        private int _coins;
        
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
            return _groundLayer.IsTouchingLayer;
        }

        public void AddCoins(int coins)
        {
            _coins += coins;
            Debug.Log($"{coins} coins added. total coins: {_coins}");
        }

        public void TakeDamage()
        {
            _animator.SetTrigger(Hit);
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _damageJumpSpeed);
        }

        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position, 
                _ineractionRadius, 
                _ineractionResult, 
                _interactionLayer
                );

            for (int i = 0; i < size; i++)
            {
                var interactable = _ineractionResult[i].GetComponent<InteractableComponent>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }
};