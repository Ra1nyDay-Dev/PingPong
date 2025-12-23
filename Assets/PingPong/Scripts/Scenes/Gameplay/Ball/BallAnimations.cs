using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PingPong.Scripts.Scenes.Gameplay.Ball
{
    public class BallAnimations : MonoBehaviour
    {
        [SerializeField] private int _minParticles = 10;
        [SerializeField] private int _maxParticles = 20;
        
        private static readonly int _hitTrigger = Animator.StringToHash("Hit");
        private Animator _animator;
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Paddle") || other.gameObject.CompareTag("Wall"))
            {
                _animator.SetTrigger(_hitTrigger);
                CreateParticles(other.GetContact(0).point);
            }
        }

        private void CreateParticles(Vector2 point)
        {
            _particleSystem.transform.position = point;
            
            var emission = _particleSystem.emission;
            int particleCount = Random.Range(_minParticles, _maxParticles + 1);
            emission.SetBursts(new ParticleSystem.Burst[] {new ParticleSystem.Burst(0f, particleCount)});
            
            _particleSystem.Play();
        }
    }
}