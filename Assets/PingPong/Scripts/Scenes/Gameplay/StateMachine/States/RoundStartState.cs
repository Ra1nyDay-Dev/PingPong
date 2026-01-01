using System.Collections;
using PingPong.Scripts.Global.Services.CoroutineRunner;
using PingPong.Scripts.Global.Services.StaticData;
using PingPong.Scripts.Scenes.Gameplay.Ball;
using PingPong.Scripts.Scenes.Gameplay.Paddle;
using PingPong.Scripts.Scenes.Gameplay.Services.GameplayFactory;
using PingPong.Scripts.Scenes.Gameplay.Services.LightController;
using PingPong.Scripts.Scenes.Gameplay.Services.RoundTimer;
using PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter;
using PingPong.Scripts.Scenes.Gameplay.StaticData;
using PingPong.Scripts.Scenes.Gameplay.UI;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class RoundStartState : IGameplayState
    {
        private readonly IGameplayStateMachine _stateMachine;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly IRoundTimer _roundTimer;
        private readonly ILightController _lightController;
        private readonly IScoreCounter _scoreCounter;
        private readonly IStaticDataService _staticDataService;
        private readonly IGameplayUI _gameplayUI;
        private readonly ICoroutineRunner _coroutineRunner;
        
        private readonly LevelSettings _settings;

        public RoundStartState(IGameplayStateMachine stateMachine, IGameplayFactory gameplayFactory, IStaticDataService staticDataService, 
            IScoreCounter scoreCounter, IRoundTimer roundTimer, ILightController lightController, 
            IGameplayUI gameplayUI, ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _gameplayFactory = gameplayFactory;
            _roundTimer = roundTimer;
            _lightController =  lightController;
            _scoreCounter = scoreCounter;
            _staticDataService = staticDataService;
            _gameplayUI =  gameplayUI;
            _coroutineRunner = coroutineRunner;
            
            _settings = _staticDataService.GetSettings("Gameplay", SettingsNames.GAMEPLAY_SETTINGS);
        }

        public void Enter()
        {
            _gameplayFactory.ResetLevelObjectsPositions();
            BlockLevelObjects();
            _roundTimer.Reset();
            _lightController.ResetLights();
            StartRoundCountdown();
        }

        public void Exit()
        {
            UnblockLevelObjects();
            _roundTimer.StartTimer();
            _gameplayFactory.Ball.GetComponent<BallMovement>().LaunchBall();
        }

        private void UnblockLevelObjects()
        {
            _gameplayFactory.Ball.SetActive(true);
            _gameplayFactory.Player1Paddle.GetComponent<PaddleMovement>().enabled = true;
            _gameplayFactory.Player2Paddle.GetComponent<PaddleMovement>().enabled = true;
        }

        private void BlockLevelObjects()
        {
            _gameplayFactory.Ball.SetActive(false);
            _gameplayFactory.Player1Paddle.GetComponent<PaddleMovement>().enabled = false;
            _gameplayFactory.Player2Paddle.GetComponent<PaddleMovement>().enabled = false;
        }

        private void StartRoundCountdown()
        {
            var isFirstRound = _scoreCounter.ScorePlayer1 == 0 && _scoreCounter.ScorePlayer2 == 0;
            var countdownTime = isFirstRound ? _settings.FirstRoundCountdownTime : _settings.RoundCountdownTime;
            
            _gameplayUI.StartRoundCountdown(countdownTime);
            _coroutineRunner.StartCoroutine(RoundCountdown(countdownTime));
        }

        private IEnumerator RoundCountdown(float time)
        {
            yield return new WaitForSeconds(time);
            _stateMachine.Enter<GameLoopState>();
        }
    }
}