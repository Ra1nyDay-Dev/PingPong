using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PingPong.Scripts.Scenes.Gameplay.Ball
{
    [RequireComponent(typeof(AudioSource))]
    public class BallAudio : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _ballHitSounds;
        [SerializeField] private float _minPitch = 1f;
        [SerializeField] private float _maxPitch = 1.2f;
        
        private AudioSource _audioSource;

        private void Awake() => 
            _audioSource = gameObject.GetComponent<AudioSource>();

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Paddle") || other.gameObject.CompareTag("Wall"))
            {
                _audioSource.pitch = Random.Range(_minPitch, _maxPitch);
                var ballHitClip = _ballHitSounds[Random.Range(0, _ballHitSounds.Length)];
                _audioSource.PlayOneShot(ballHitClip);
            }
        }
    }
}