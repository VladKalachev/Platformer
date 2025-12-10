using System.Collections;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.Health;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEditor.Animations;
using UnityEngine;

namespace PixelCrew.Creatures.Hero
{
    public class Hero : Creature
    {
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private LayoutCheck _wallCheck;
        
        [SerializeField] private float _slamDownVelocity;
        [SerializeField] private Cooldown  _throwCooldown;
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;
        
        [Space] [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;
        
        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWallKey = Animator.StringToHash("is-on-wall");

        private bool _allowDoubleJump;
        private bool _isOnWall;
        
        private GameSession _session;
        private float _defaultGravityScale;
        
        private int CoinsCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count("Sword");

        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale;
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
            var moveToSameDirection = Direction.x * transform.localScale.x > 0;
            if (_wallCheck.IsTouchingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }
            
            Animator.SetBool(IsOnWallKey, _isOnWall);
        }
        
        protected override float CalculateYVelocity()
        {
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded || _isOnWall)
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
            if (!IsGrounded && _allowDoubleJump && !_isOnWall)
            {
                _particles.Spawn("Jump");
                _allowDoubleJump = false;
                return _jumpSpeed;
            }
            
            return base.CalculateJumpVelocity(yVelocity);
        }

        public void AddInInventory(string id, int value)
        {
            
        }
        

        public override void TakeDamage()
        {
            base.TakeDamage();
            if (CoinsCount > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(CoinsCount, 5);
            _session.Data.Inventory.Remove("Coin", numCoinsToDispose);

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
            _interactionCheck.Check();
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
            if (SwordCount <= 0) return;
            
            base.Attack();
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _disarmed;
        }

        public void OnDoThrow()
        {
            _particles.Spawn("Throw");
        }

        public void Throw()
        {
            if (_throwCooldown.IsReady)
            {
                Animator.SetTrigger(ThrowKey);
                _throwCooldown.Reset();
            }
        }
    }
};