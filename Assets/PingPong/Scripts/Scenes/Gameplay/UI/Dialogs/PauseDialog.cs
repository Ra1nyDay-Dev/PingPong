using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.GameAudioMixer;
using PingPong.Scripts.Global.Services.SceneLoader;
using PingPong.Scripts.Global.UI;
using PingPong.Scripts.Scenes.Gameplay.StateMachine;
using PingPong.Scripts.Scenes.Gameplay.StateMachine.States;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PingPong.Scripts.Scenes.Gameplay.UI.Dialogs
{
    public class PauseDialog : SceneDialog
    {
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;
        
        private ISceneLoader _sceneLoader;
        private IGameplayStateMachine _gameplayStateMachine;
        private IGameAudioMixer _audioMixer;

        private void Awake()
        {
            Time.timeScale = 0f;
            _sceneLoader = ProjectServices.Container.Get<ISceneLoader>();
            _gameplayStateMachine = SceneServices.Container.Get<IGameplayStateMachine>();
            _audioMixer = ProjectServices.Container.Get<IGameAudioMixer>();
            LoadVolumes();
        }
        
        public void OnContinueButtonClicked()
        {
            Time.timeScale = 1f;
            _sceneUI.HideDialog<PauseDialog>();
        }

        public void OnBackToMenuButtonClicked()
        {
            Time.timeScale = 1f;
            _gameplayStateMachine.Enter<GameOverState>();
            _sceneLoader.Load(Global.Data.Scenes.MENU);
        }
        
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