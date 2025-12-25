using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PingPong.Scripts.Scenes.Gameplay.Ball
{
    [RequireComponent(typeof(AudioSource))]
    public class BallAudio : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _ballHitSounds;
        [SerializeField] private AudioClip _ballLaunch;
        
        [SerializeField] private float _minBallHitPitch = 1f;
        [SerializeField] private float _maxBallHitPitch = 1.2f;
        
        private AudioSource _audioSource;

        private void Awake() => 
            _audioSource = gameObject.GetComponent<AudioSource>();

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Paddle") || other.gameObject.CompareTag("Wall"))
                PlayBallHit();
        }
        
        public void PlayBallLaunch() => 
            _audioSource.PlayOneShot(_ballLaunch);
        
        public void PlayBallHit()
        {
            _audioSource.pitch = Random.Range(_minBallHitPitch, _maxBallHitPitch);
            var ballHitClip = _ballHitSounds[Random.Range(0, _ballHitSounds.Length)];
            _audioSource.PlayOneShot(ballHitClip);
        }
    }
}