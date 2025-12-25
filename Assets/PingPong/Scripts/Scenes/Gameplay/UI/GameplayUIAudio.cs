using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class GameplayUIAudio :  MonoBehaviour
    {
        [SerializeField] private AudioClip _countDownTick;
        [SerializeField] private AudioClip _countDownLastTick;
        
        private AudioSource _audioSource;
        
        private void Awake() => 
            _audioSource =  GetComponent<AudioSource>();
        
        public void PlayCountdownTick() => _audioSource.PlayOneShot(_countDownTick);
        
        public void PlayCountdownLastTick() => _audioSource.PlayOneShot(_countDownLastTick);
    }
}