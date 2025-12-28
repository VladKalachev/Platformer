using PixelCrew.Components.Audio;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Params")] [SerializeField] private bool _invertScale;
        [SerializeField] private float _speed;
        [SerializeField] protected float _jumpSpeed;
        [SerializeField] protected float _damageVelocity;
        
        [Header("Checkers")]
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] private ColliderCheck _groundCheck;
        [SerializeField] private CheckCircleOverlap _attackRange;
        [SerializeField] protected SpawnListComponent _particles;
        
        protected Rigidbody2D Rigidbody;
        protected Vector2 Direction;
        protected Animator Animator;
        protected PlaySoundsComponent Sounds;
        protected bool IsGrounded;
        private bool _isJumping;
        
        private static readonly int IsGround =  Animator.StringToHash("is-ground");
        private static readonly int IsVerticalVelocity =  Animator.StringToHash("vertical-velocity");
        private static readonly int IsRunning =  Animator.StringToHash("is-running");
        private static readonly int Hit =  Animator.StringToHash("hit");
        private static readonly int AttackKey =  Animator.StringToHash("attack");

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator =  GetComponent<Animator>();
            Sounds =  GetComponent<PlaySoundsComponent>();
        }
        
        public void SetDirection(Vector2 direction)
        {
            Direction = direction;  
        }

        protected virtual void Update()
        {
            IsGrounded = _groundCheck.IsTouchingLayer;
        }
        
        private void FixedUpdate()
        {
            var xVelocity = Direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            Rigidbody.linearVelocity = new Vector2(xVelocity, yVelocity);
            
            Animator.SetBool(IsGround, IsGrounded);
            Animator.SetBool(IsRunning, Direction.x != 0);
            Animator.SetFloat(IsVerticalVelocity, Rigidbody.linearVelocity.y);
            
            UpdateSpriteDirection(Direction);
        }
        
        public void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = _invertScale ? -1 : 1;
            
            if (Direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1);
            } else if (Direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
            }
        }

        
        protected virtual float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.linearVelocity.y;
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded)
            {
                _isJumping = false;
            }

            if (isJumpPressing) 
            {
                _isJumping = true;
                
                var isFalling = Rigidbody.linearVelocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (Rigidbody.linearVelocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded)
            {
                yVelocity += _jumpSpeed;
                DoJumpVfx();
            } 
            
            return yVelocity;
        }

        protected void DoJumpVfx()
        {
            _particles.Spawn("Jump");
            Sounds.Play("Jump");
        }
        
        public virtual void TakeDamage()
        {
            _isJumping = false;
            Animator.SetTrigger(Hit);
            Rigidbody.linearVelocity = new Vector2(Rigidbody.linearVelocity.x, _damageVelocity);

        }
        
        public virtual void Attack()
        {
            Animator.SetTrigger(AttackKey);
            Sounds.Play("Melee");
        }
        
        public void OnDoAttack()
        {
            _attackRange.Check();
            _particles.Spawn("Slash");
        }

    }
}