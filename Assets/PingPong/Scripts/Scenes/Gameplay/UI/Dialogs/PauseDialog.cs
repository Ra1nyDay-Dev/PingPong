using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.SceneLoader;
using PingPong.Scripts.Global.UI;
using PingPong.Scripts.Scenes.Gameplay.StateMachine;
using PingPong.Scripts.Scenes.Gameplay.StateMachine.States;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PingPong.Scripts.Scenes.Gameplay.UI.Dialogs
{
    public class PauseDialog : SceneDialog
    {
        private ISceneLoader _sceneLoader;
        private IGameplayStateMachine _gameplayStateMachine;

        private void Awake()
        {
            Time.timeScale = 0f;
            _sceneLoader = ProjectServices.Container.Get<ISceneLoader>();
            _gameplayStateMachine = SceneServices.Container.Get<IGameplayStateMachine>();
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
    }
}