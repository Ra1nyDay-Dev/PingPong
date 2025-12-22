using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.Input;
using PingPong.Scripts.Scenes.Gameplay.Ball;
using PingPong.Scripts.Scenes.Gameplay.Paddle;
using PingPong.Scripts.Scenes.Gameplay.Services.RoundTimer;
using PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter;
using PingPong.Scripts.Scenes.Gameplay.StaticData;
using PingPong.Scripts.Scenes.Gameplay.UI;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class GameStartState : IGameplayState
    {
        private const string LEVEL_SETTINGS = "Gameplay/StaticData/GameplayLevelSettings";
        private const string PADDLE_PREFAB = "Gameplay/Paddle/Paddle";
        private const string BALL_PREFAB = "Gameplay/Ball/Ball";
        
        private GameVersusMode _currentGameVersusMode;
        private GameplayLevelSettings _settings;
        private GameObject _ballPrefab;
        private IGameplayStateMachine _stateMachine;
        private GameObject _paddlePrefab;
        private IInputService _player1InputService;
        private IInputService _player2InputService;
        private GameObject _ball;
        private IRoundTimer _roundTimer;
        private IScoreCounter _scoreCounter;

        public void Enter()
        {
            GetStateDependencies();
            PrepareLevel();
            _roundTimer.Reset();
            ResetScore();
            _stateMachine.Enter<RoundStartState>();
        }

        public void Exit()
        {
        }

        private void GetStateDependencies()
        {
            _currentGameVersusMode =  GameVersusMode.PlayerVsAI;
            _settings = Resources.Load<GameplayLevelSettings>(LEVEL_SETTINGS);
            _ballPrefab = Resources.Load<GameObject>(BALL_PREFAB);
            _paddlePrefab = Resources.Load<GameObject>(PADDLE_PREFAB);
            _stateMachine = SceneServices.Container.Get<IGameplayStateMachine>();
            _player1InputService = ProjectServices.Container.Get<IInputService>($"{PlayerId.Player1}");
            _player2InputService = ProjectServices.Container.Get<IInputService>($"{PlayerId.Player2}");
            _scoreCounter = SceneServices.Container.Get<IScoreCounter>();
            _roundTimer = SceneServices.Container.Get<IRoundTimer>();
        }

        private void PrepareLevel()
        {
            CreateBall();
            CreatePaddles();
        }

        private void CreateBall()
        {
            _ball = Object.Instantiate(_ballPrefab, _settings.BallStartPosition, Quaternion.identity);
            _ball.GetComponent<BallMovement>().Construct
            ( 
                launchSpeed: _settings.BallLaunchSpeed,
                maxSpeed: _settings.BallMaxSpeed,
                speedIncreasePerHit: _settings.BallSpeedIncreasePerHit,
                maxLaunchAngle: _settings.BallMaxLaunchAngle
            );
        }

        private void CreatePaddles()
        {
            switch (_currentGameVersusMode)
            {
                case GameVersusMode.PlayerVsAI:
                {
                    CreatePlayerPaddle(PlayerId.Player1, _player2InputService);
                    CreateAIPaddle(PlayerId.Player2);
                    break;
                }
                case GameVersusMode.PlayerVsPlayer:
                {
                    CreatePlayerPaddle(PlayerId.Player1, _player1InputService);
                    CreatePlayerPaddle(PlayerId.Player2, _player2InputService);
                    break;
                }
                case GameVersusMode.AIvsAI:
                {
                    CreateAIPaddle(PlayerId.Player1);
                    CreateAIPaddle(PlayerId.Player2);
                    break;
                }
            }
        }

        private void CreatePlayerPaddle( PlayerId playerId, IInputService inputService )
        {
            var paddle = CreatePaddle(playerId);
            var paddleContolls = new PlayerPaddleControlls(inputService);
            paddle.GetComponent<PaddleMovement>().Construct(_settings.PaddleSpeed, _settings.LevelBoundsY, paddleContolls);
        }

        private void CreateAIPaddle(PlayerId playerId)
        {
            var paddle = CreatePaddle(playerId);
            var paddleContolls = new AIPaddleControlls(_ball, paddle.transform, _settings.LevelBoundsY);
            paddle.GetComponent<PaddleMovement>().Construct(_settings.PaddleSpeed, _settings.LevelBoundsY, paddleContolls);
        }

        private GameObject CreatePaddle(PlayerId playerId)
        {
            Vector3 position = playerId == PlayerId.Player1 ? _settings.Player1PaddleStartPosition : _settings.Player2PaddleStartPosition;
            GameObject paddle = Object.Instantiate(_paddlePrefab, position, Quaternion.identity);
            paddle.GetComponent<PaddleID>().PlayerId = playerId;
            
            return paddle;
        }

        private void ResetScore() => 
            _scoreCounter.Reset();
    }
}