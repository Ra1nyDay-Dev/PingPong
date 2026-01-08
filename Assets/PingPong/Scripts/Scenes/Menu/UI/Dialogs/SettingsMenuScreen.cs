using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.GameAudioMixer;
using PingPong.Scripts.Global.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PingPong.Scripts.Scenes.Menu.UI.Dialogs
{
    public class SettingsMenuScreen : SceneDialog
    {
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;
        
        private IGameAudioMixer _audioMixer;

        private void Awake()
        {
            _audioMixer = ProjectServices.Container.Get<IGameAudioMixer>();
            LoadVolumes();
        }

        public void OnBackButtonClicked() => 
            _sceneUI.SwitchScreen<MainMenuScreen>();
        
        public void SetMusicVolume(float volume) => 
            _audioMixer.SetMusicVolume(volume);

        public void SetSFXVolume(float volume) => 
            _audioMixer.SetSFXVolume(volume);

        private void LoadVolumes()
        {
            float music = _audioMixer.GetMusicVolume();
            float sfx = _audioMixer.GetSFXVolume();

            _musicSlider.value = music;
            _sfxSlider.value = sfx;
        }
    }
}
