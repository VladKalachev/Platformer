using System;
using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class Creature : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] protected float _jumpSpeed;
        [SerializeField] private float _damageVelocity;
        [SerializeField] private int _damage;
        [SerializeField] private LayerMask _groundLayer;
        
        [SerializeField] private LayoutCheck _groundCheck;
        [SerializeField] private CheckCircleOverlap _attackRange;

        [SerializeField] protected SpawnListComponent _particles;
        
        protected Rigidbody2D _rigidbody;
        protected Vector2 _direction;
        private Animator _animator;
        protected bool _isGrounded;
        private bool _isJumping;
        
        private static readonly int IsGround =  Animator.StringToHash("is-ground");
        private static readonly int IsVerticalVelocity =  Animator.StringToHash("vertical-velocity");
        private static readonly int IsRunning =  Animator.StringToHash("is-running");
        private static readonly int Hit =  Animator.StringToHash("hit");
        private static readonly int AttackKey =  Animator.StringToHash("attack");

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator =  GetComponent<Animator>();
        }
        
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;  
        }

        protected virtual void Update()
        {
            _isGrounded = _groundCheck.IsTouchingLayer;
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
        
        protected virtual float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.linearVelocity.y;
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded)
            {
                _isJumping = false;
            }

            if (isJumpPressing) 
            {
                _isJumping = true;
                
                var isFalling = _rigidbody.linearVelocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (_rigidbody.linearVelocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (_isGrounded)
            {
                yVelocity += _jumpSpeed;
                _particles.Spawn("Jump");
            } 
            
            return yVelocity;
        }
    }
}