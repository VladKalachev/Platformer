using System;
using System.Collections;
using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class MobAI: MonoBehaviour
    {
        [SerializeField] private LayoutCheck _vision;
        [SerializeField] private LayoutCheck _canAttack;

        [SerializeField] private float _alarmDelay = 0.5f;
        private Coroutine _current;
        private GameObject _target;

        private SpawnListComponent _particles;

        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
        }

        private void Start()
        {
            StartState(Patrolling());
        }

        public void OnHeroInVision(GameObject go)
        {
            _target = go;
            
            StartState(AgroToHero());
        }

        private IEnumerator AgroToHero()
        {
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);
            StartState(GoToHero());
        }

        private IEnumerator GoToHero()
        {
            throw new NotImplementedException();
        }

        private IEnumerator Patrolling()
        {
            yield return null;
        }

        private void StartState(IEnumerator coroutine)
        {
            if (_current != null)
            {
                StopCoroutine(_current);
            }
            
            _current = StartCoroutine(coroutine);
        }
    }
}