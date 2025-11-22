using System;
using System.Collections;
using PixelCrew.Components;
using PixelCrew.Creatures;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace  PixelCrew
{
    public class Hero : Creature
    {
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private LayoutCheck _wallCheck;
        
        [SerializeField] private float _slamDownVelocity;
        [SerializeField] private float _ineractionRadius;
        [SerializeField] private float _damageVelocity;

        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;
        
        [Space] [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;
        
        private Collider2D[] _interactionResult = new Collider2D[1];
        private bool _allowDoubleJump;
        private bool _isOnWall;
        
        private GameSession _session;
        private float _defaultGravityScale;

        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = _rigidbody.gravityScale;
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
        
        
        protected override void Update()
        {
            base.Update();
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
        
        protected override float CalculateYVelocity()
        {
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded || _isOnWall)
            {
                _allowDoubleJump = true;
            }

            if (!isJumpPressing && _isOnWall)
            {
                return 0f;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!_isGrounded && _allowDoubleJump)
            {
                _particles.Spawn("Jump");
                _allowDoubleJump = false;
                return _jumpSpeed;
            }
            
            return base.CalculateJumpVelocity(yVelocity);
        }
        
        public void AddCoins(int coins)
        {
            _session.Data.Coins += coins;
            Debug.Log($"{coins} coins added. total coins: {_session.Data.Coins}");
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
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
                _interactionResult, 
                _interactionLayer
                );

            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
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
                    _particles.Spawn("SlamDown");
                }

                if (contact.relativeVelocity.y >= _damageVelocity)
                {
                    GetComponent<HealtComponent>().ModifyHealth(-1);
                }
            }
        }

        public override void Attack()
        {
            if (!_session.Data.IsArmed) return;
            
            base.Attack();
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