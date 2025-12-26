using PingPong.Scripts.Global.AssetManagement;
using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.Input;
using PingPong.Scripts.Global.Services.StaticData;
using PingPong.Scripts.Scenes.Gameplay.Ball;
using PingPong.Scripts.Scenes.Gameplay.Paddle;
using PingPong.Scripts.Scenes.Gameplay.Services.RoundTimer;
using PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter;
using PingPong.Scripts.Scenes.Gameplay.StaticData;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class GameStartState : IGameplayState
    {
        private GameVersusMode _currentGameVersusMode;
        private LevelSettings _settings;
        private IGameplayStateMachine _stateMachine;
        private IInputService _player1InputService;
        private IInputService _player2InputService;
        private GameObject _ball;
        private IRoundTimer _roundTimer;
        private IScoreCounter _scoreCounter;
        private IStaticDataService _staticDataService;
        private IAssetProvider _assetProvider;

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
            _staticDataService = ProjectServices.Container.Get<IStaticDataService>();
            _assetProvider = ProjectServices.Container.Get<IAssetProvider>();
            _settings = _staticDataService.GetSettings("Gameplay", SettingsNames.GAMEPLAY_SETTINGS);
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
            _ball = _assetProvider.Instantiate(AssetPath.BALL, _settings.BallStartPosition);
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
            var paddleControlls = new PlayerPaddleControlls(inputService);
            paddle.GetComponent<PaddleMovement>().Construct(_settings.PaddleSpeed, _settings.LevelBoundsY, paddleControlls);
        }

        private void CreateAIPaddle(PlayerId playerId)
        {
            var paddle = CreatePaddle(playerId);
            var paddleControlls = new AIPaddleControlls(_ball, paddle.transform, _settings.LevelBoundsY);
            paddle.GetComponent<PaddleMovement>().Construct(_settings.PaddleSpeed, _settings.LevelBoundsY, paddleControlls);
        }

        private GameObject CreatePaddle(PlayerId playerId)
        {
            Vector3 position = playerId == PlayerId.Player1 ? _settings.Player1PaddleStartPosition : _settings.Player2PaddleStartPosition;
            Vector3 rotation = playerId == PlayerId.Player1 ? _settings.Player1PaddleRotation : _settings.Player2PaddleRotation;
            GameObject paddle = _assetProvider.Instantiate(AssetPath.PADDLE, position, Quaternion.Euler(rotation));
            paddle.GetComponent<PaddleID>().PlayerId = playerId;
            
            return paddle;
        }

        private void ResetScore() => 
            _scoreCounter.Reset();
    }
}