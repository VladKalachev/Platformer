using System;
using System.Collections;
using PixelCrew.Components;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace  PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _damageJumpSpeed;
        [SerializeField] private float _slamDownVelocity;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _ineractionRadius;
        [SerializeField] private int _damage;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private float _damageVelocity;
        [SerializeField] private LayoutCheck _wallCheck;
        
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;

        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;
        
        [SerializeField] private CheckCircleOverlap _attackRange;
        
        [Space] [Header("Particles")]
        [SerializeField] private SpawnComponent _footStepParticles;
        [SerializeField] private SpawnComponent _jumpParticles;
        [SerializeField] private SpawnComponent _slamDownParticles;
        [SerializeField] private ParticleSystem _hitParticles;
        
        private Collider2D[] _ineractionResult = new Collider2D[1];
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Animator _animator;
        private bool _isGrounded;
        private bool _allowDoubleJump;
        private bool _isJumping;
        private bool _isOnWall;
        
        private static readonly int IsGround =  Animator.StringToHash("is-ground");
        private static readonly int IsVerticalVelocity =  Animator.StringToHash("vertical-velocity");
        private static readonly int IsRunning =  Animator.StringToHash("is-running");
        private static readonly int Hit =  Animator.StringToHash("hit");
        private static readonly int AttackKey =  Animator.StringToHash("attack");

        private GameSession _session;
        private float _defaultGravityScale;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator =  GetComponent<Animator>();
            _defaultGravityScale = _rigidbody.gravityScale;
        }

        private void Update()
        {
            _isGrounded = IsGrounded();

            if (_wallCheck.IsTouchingLayer && _direction.x == transform.localScale.x)
            {
                _isOnWall = true;
                _rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                _rigidbody.gravityScale = _defaultGravityScale;
            }
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var health = GetComponent<HealtComponent>();
            health.SetHealth(_session.Data.Hp);
            UpdateHeroWeapon();
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;  
        }

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                transform.localScale = Vector3.one;
            } else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
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

            if (_isGrounded)
            {
                _allowDoubleJump = true;
                _isJumping = false;
            }

            if (_isOnWall)
            {
                _allowDoubleJump = true;
            }
            
            if (isJumpPressing) 
            {
                _isJumping = true;
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_isOnWall)
            {
                yVelocity = 0f;
            }
            else if (_rigidbody.linearVelocity.y > 0 && _isJumping)
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
                _jumpParticles.Spawn();
            } else if (_allowDoubleJump)
            {
                yVelocity = _jumpSpeed;
                _jumpParticles.Spawn();
                _allowDoubleJump = false;
            }
            
            return yVelocity;
        }

        private bool IsGrounded()
        {
            var hit = Physics2D.CircleCast(transform.position + _groundCheckPositionDelta, _groundCheckRadius,
                Vector2.down, 0, _groundLayer);
            return hit.collider != null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = IsGrounded() ? HandlesUtils.TransparentGreen : HandlesUtils.TransparentRed;
            Handles.DrawWireDisc(transform.position + _groundCheckPositionDelta, Vector3.forward, _groundCheckRadius);
        } 
#endif
       
        public void AddCoins(int coins)
        {
            _session.Data.Coins += coins;
            Debug.Log($"{coins} coins added. total coins: {_session.Data.Coins}");
        }

        public void TakeDamage()
        {
            _isJumping = false;
            _animator.SetTrigger(Hit);
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _damageJumpSpeed);

            if (_session.Data.Coins > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_session.Data.Coins, 5);
            _session.Data.Coins -= numCoinsToDispose;

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);
            
            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
            
            StartCoroutine(DisableParticlesAfterPlay());
        }
        
        private IEnumerator DisableParticlesAfterPlay()
        {
            yield return new WaitWhile(() => _hitParticles.isPlaying);
            _hitParticles.gameObject.SetActive(false);
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

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    _slamDownParticles.Spawn();
                }

                if (contact.relativeVelocity.y >= _damageVelocity)
                {
                    GetComponent<HealtComponent>().ModifyHealth(-1);
                }
            }
        }

        public void SpawnFootDust()
        {
            _footStepParticles.Spawn();
        }

        public void Attack()
        {
            if (!_session.Data.IsArmed) return;
            
            _animator.SetTrigger(AttackKey);
        }

        public void OnDoAttack()
        {
            var gos =  _attackRange.GetObjectsInRange();
            foreach (var go in gos)
            {
                var hp = go.GetComponent<HealtComponent>();
                if (hp != null && go.CompareTag("Enemy"))
                {
                    hp.ModifyHealth(-_damage);
                }
            }  
        }

        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();
        }

        private void UpdateHeroWeapon()
        {
            _animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _disarmed;
        }
    }
};