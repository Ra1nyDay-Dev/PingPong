using System;
using UnityEngine;
using UnityEngine.Audio;

namespace PingPong.Scripts.Global.Services.GameAudioMixer
{
    public class GameAudioMixer : MonoBehaviour, IGameAudioMixer
    {
        [SerializeField] private AudioMixer _audioMixer;
        
        private const string MUSIC_KEY = "MusicVolume";
        private const string SFX_KEY = "SFXVolume";

        private void Start() => 
            LoadVolumes();

        public float GetMusicVolume() => 
            PlayerPrefs.GetFloat(MUSIC_KEY, 1f);

        public void SetMusicVolume(float volume)
        {
            _audioMixer.SetFloat(MUSIC_KEY, Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat(MUSIC_KEY, volume);
        }

        public float GetSFXVolume() => 
            PlayerPrefs.GetFloat(SFX_KEY, 1f);

        public void SetSFXVolume(float volume)
        {
            _audioMixer.SetFloat(SFX_KEY, Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat(SFX_KEY, volume);
        }

        private void LoadVolumes()
        {
            float music = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
            float sfx = PlayerPrefs.GetFloat(SFX_KEY, 1f);

            SetMusicVolume(music);
            SetSFXVolume(sfx);
        }
    }
}
