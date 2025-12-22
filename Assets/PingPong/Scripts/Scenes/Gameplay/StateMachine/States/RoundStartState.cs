using System.Collections;
using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.CoroutineRunner;
using PingPong.Scripts.Scenes.Gameplay.Ball;
using PingPong.Scripts.Scenes.Gameplay.Paddle;
using PingPong.Scripts.Scenes.Gameplay.Services.RoundTimer;
using PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter;
using PingPong.Scripts.Scenes.Gameplay.StaticData;
using PingPong.Scripts.Scenes.Gameplay.UI;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class RoundStartState : IGameplayState
    {
        private const string LEVEL_SETTINGS = "Gameplay/StaticData/GameplayLevelSettings";
        
        private GameObject _ball;
        private GameObject[] _paddles;
        private GameplayLevelSettings _settings;
        private IGameplayUI _gameplayUI;
        private ICoroutineRunner _coroutineRunner;
        private IGameplayStateMachine _stateMachine;
        private IScoreCounter _scoreCounter;
        private IRoundTimer _roundTimer;

        public void Enter()
        {
            GetStateDependencies();
            ResetPositions();
            DisableObjects();
            _roundTimer.Reset();
            StartRoundCountdown();
        }

        public void Exit()
        {
            EnableObjects();
            _roundTimer.StartTimer();
            _ball.GetComponent<BallMovement>().LaunchBall();
        }

        private void GetStateDependencies()
        {
            _paddles = GameObject.FindGameObjectsWithTag("Paddle");
            _ball = GameObject.FindWithTag("Ball");
            _settings = Resources.Load<GameplayLevelSettings>(LEVEL_SETTINGS);
            _gameplayUI = SceneServices.Container.Get<IGameplayUI>();
            _coroutineRunner = ProjectServices.Container.Get<ICoroutineRunner>();
            _stateMachine = SceneServices.Container.Get<IGameplayStateMachine>();
            _scoreCounter = SceneServices.Container.Get<IScoreCounter>(); 
            _roundTimer = SceneServices.Container.Get<IRoundTimer>(); 
        }

        private void ResetPositions()
        {
            _ball.transform.position = _settings.BallStartPosition;

            foreach (GameObject paddle in _paddles)
            {
                paddle.transform.position = paddle.GetComponent<PaddleID>().PlayerId == PlayerId.Player1 
                    ? _settings.Player1PaddleStartPosition 
                    : _settings.Player2PaddleStartPosition;
            }
        }

        private void DisableObjects()
        {
            _ball.SetActive(false);

            foreach (var paddle in _paddles)
                paddle.GetComponent<PaddleMovement>().enabled = false;
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

        private void EnableObjects()
        {
            _ball.SetActive(true);

            foreach (var paddle in _paddles)
                paddle.GetComponent<PaddleMovement>().enabled = true;
        }
    }
}