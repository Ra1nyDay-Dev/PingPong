using System.Collections;
using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services.CoroutineRunner;
using PingPong.Scripts.Global.Services.StaticData;
using PingPong.Scripts.Scenes.Gameplay.Ball;
using PingPong.Scripts.Scenes.Gameplay.Camera;
using PingPong.Scripts.Scenes.Gameplay.Paddle;
using PingPong.Scripts.Scenes.Gameplay.Services.GameplayFactory;
using PingPong.Scripts.Scenes.Gameplay.Services.LightController;
using PingPong.Scripts.Scenes.Gameplay.Services.RoundTimer;
using PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter;
using PingPong.Scripts.Scenes.Gameplay.StaticData;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class RoundEndState : IPayloadedGameplayState<PlayerId>
    {
        private readonly IGameplayStateMachine _stateMachine;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly IScoreCounter _scoreCounter;
        private readonly IRoundTimer _roundTimer;
        private readonly ILightController _lightController;
        private readonly ICoroutineRunner _coroutineRunner;
        
        private readonly UnityEngine.Camera _camera;
        private readonly LevelSettings _settings;
        
        public RoundEndState(GameplayStateMachine stateMachine, IGameplayFactory gameplayFactory,
            IStaticDataService staticDataService, IScoreCounter scoreCounter, IRoundTimer roundTimer, 
            ILightController lightController, ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _gameplayFactory = gameplayFactory;
            _staticDataService = staticDataService;
            _scoreCounter = scoreCounter;
            _roundTimer = roundTimer;
            _lightController = lightController;
            _coroutineRunner = coroutineRunner;
            
            _camera = UnityEngine.Camera.main;
            _settings = _staticDataService.GetSettings("Gameplay", SettingsNames.GAMEPLAY_SETTINGS);
        }

        public void Enter(PlayerId playerLosed)
        {
            DisableMovement();
            ShakeCamera();
            _scoreCounter.UpdateScore(playerLosed);
            _roundTimer.StopTimer();
            _coroutineRunner.StartCoroutine(HighlightGates(playerLosed, 1.5f));
        }

        public void Exit()
        {
            
        }

        private void DisableMovement()
        {
            _gameplayFactory.Player1Paddle.GetComponent<PaddleMovement>().enabled = false;
            _gameplayFactory.Player2Paddle.GetComponent<PaddleMovement>().enabled = false;
            _gameplayFactory.Ball.GetComponent<BallMovement>().StopBall();
        }

        private void ShakeCamera() => 
            _camera.GetComponent<CameraShake>().Shake(0.5f, magnitude: 0.2f);

        private IEnumerator HighlightGates(PlayerId playerLosed, float showTime)
        {
            if (playerLosed == PlayerId.Player1)
                _lightController.HighlightLeftGates();
            else
                _lightController.HighlightRightGates();
            
            yield return new WaitForSeconds(showTime);
            
            CheckGameOver();
        }


        private void CheckGameOver()
        {
            bool isGameOver = _scoreCounter.ScorePlayer1 == _settings.ScoreToWin || 
                              _scoreCounter.ScorePlayer2 == _settings.ScoreToWin;

            if (isGameOver)
                _stateMachine.Enter<GameOverState>();
            else
                _stateMachine.Enter<RoundStartState>();
        }
    }
}