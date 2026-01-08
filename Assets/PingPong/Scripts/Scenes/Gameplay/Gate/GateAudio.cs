using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.Gate
{
    [RequireComponent(typeof(AudioSource))]
    public class GateAudio : MonoBehaviour
    {
        [SerializeField] private AudioClip _goalAlarm;
        [SerializeField] private AudioClip _goalHit;
        
        private AudioSource _audioSource;

        private void Awake() => 
            _audioSource = gameObject.GetComponent<AudioSource>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ball"))
            {
                _audioSource.PlayOneShot(_goalAlarm);
                _audioSource.PlayOneShot(_goalHit);
            }
        }
    }
}