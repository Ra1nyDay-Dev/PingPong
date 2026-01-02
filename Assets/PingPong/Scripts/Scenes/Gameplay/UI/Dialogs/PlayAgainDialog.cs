using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.SceneLoader;
using PingPong.Scripts.Global.UI;
using PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter;
using PingPong.Scripts.Scenes.Gameplay.StateMachine;
using PingPong.Scripts.Scenes.Gameplay.StateMachine.States;
using TMPro;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.UI.Dialogs
{
    public class PlayAgainDialog : SceneDialog
    {
        [SerializeField] private TextMeshProUGUI _scoreLeft;
        [SerializeField] private TextMeshProUGUI _scoreRight;
        [SerializeField] private Color _winColor = new Color32(106, 173, 137, 255);
        [SerializeField] private Color _loseColor = new Color32(221, 66, 107, 255);
        
        private ISceneLoader _sceneLoader;
        private IGameplayStateMachine _gameplayStateMachine;
        private IScoreCounter _scoreCounter;

        private void Awake()
        {
            _sceneLoader = ProjectServices.Container.Get<ISceneLoader>();
            _gameplayStateMachine = SceneServices.Container.Get<IGameplayStateMachine>();
            _scoreCounter = SceneServices.Container.Get<IScoreCounter>();
            
            GetScore();
        }

        private void GetScore()
        {
            _scoreLeft.text = $"{_scoreCounter.ScorePlayer1}";
            _scoreRight.text = $"{_scoreCounter.ScorePlayer2}";
            _scoreLeft.color = _scoreCounter.ScorePlayer1 >  _scoreCounter.ScorePlayer2 ? _winColor : _loseColor;
            _scoreRight.color = _scoreCounter.ScorePlayer2 >  _scoreCounter.ScorePlayer1 ? _winColor : _loseColor;
        }

        public void OnPlayAgainButtonClicked()
        {
            _gameplayStateMachine.Enter<GameStartState>();
            _sceneUI.HideAllDialogs();
        }
        
        public void OnBackToMenuButtonClicked() => 
            _sceneLoader.Load(Global.Data.Scenes.MENU);
    }
}
