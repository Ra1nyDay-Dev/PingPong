using System;
using PingPong.Scripts.Global.AssetManagement;
using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services.Input;
using PingPong.Scripts.Global.Services.StaticData;
using PingPong.Scripts.Scenes.Gameplay.Ball;
using PingPong.Scripts.Scenes.Gameplay.Paddle;
using PingPong.Scripts.Scenes.Gameplay.StaticData;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.Services.GameplayFactory
{
    public class GameplayFactory : IGameplayFactory
    {
        public GameObject Ball { get; private set; }
        public GameObject Player1Paddle { get; private set; }
        public GameObject Player2Paddle { get; private set; }
        
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly LevelSettings _settings;
        private readonly IInputService _player1InputService;
        private readonly IInputService _player2InputService;
        private readonly GameplayEntryParams _sceneParams;

        public GameplayFactory(IAssetProvider assetProvider, IStaticDataService staticDataService
            , IInputService player1InputService, IInputService player2InputService, GameplayEntryParams sceneParams)
        {
           _assetProvider  = assetProvider;
           _player1InputService = player1InputService;
           _player2InputService = player2InputService;
           _staticDataService =  staticDataService;
           _sceneParams = sceneParams;
           
           _settings = _staticDataService.GetSettings("Gameplay", SettingsNames.GAMEPLAY_SETTINGS);
        }

        public void CreateLevelObjects()
        {
            CreateBall();
            CreatePaddles();
        }
        
        public void CreateBall()
        {
            Ball = _assetProvider.Instantiate(AssetPath.BALL, _settings.BallStartPosition);
            Ball.GetComponent<BallMovement>().Construct
            ( 
                launchSpeed: _settings.BallLaunchSpeed,
                maxSpeed: _settings.BallMaxSpeed,
                speedIncreasePerHit: _settings.BallSpeedIncreasePerHit,
                maxLaunchAngle: _settings.BallMaxLaunchAngle,
                maxBounceAngle: _settings.BallMaxBounceAngle
            );
        }
        
        public void CreatePaddles()
        {
            if (!Ball && (_sceneParams.GameVersusMode == GameVersusMode.PlayerVsAI ||  _sceneParams.GameVersusMode == GameVersusMode.AIvsAI))
                throw new Exception("Ball not created. AI Paddles need it to track");
                
            switch (_sceneParams.GameVersusMode)
            {
                case GameVersusMode.PlayerVsAI:
                {
                    Player1Paddle = CreatePlayerPaddle(PlayerId.Player1, _player2InputService);
                    Player2Paddle = CreateAIPaddle(PlayerId.Player2, Ball);
                    break;
                }
                case GameVersusMode.PlayerVsPlayer:
                {
                    Player1Paddle = CreatePlayerPaddle(PlayerId.Player1, _player1InputService);
                    Player2Paddle = CreatePlayerPaddle(PlayerId.Player2, _player2InputService);
                    break;
                }
                case GameVersusMode.AIvsAI:
                {
                    Player1Paddle = CreateAIPaddle(PlayerId.Player1, Ball);
                    Player2Paddle = CreateAIPaddle(PlayerId.Player2, Ball);
                    break;
                }
            }
        }

        public void ResetLevelObjectsPositions()
        {
            ResetBallPosition();
            ResetPaddlesPosition();
        }
        
        public void ResetBallPosition() => 
            Ball.transform.position = _settings.BallStartPosition;

        public void ResetPaddlesPosition()
        {
            Player1Paddle.transform.position = _settings.Player1PaddleStartPosition;
            Player2Paddle.transform.position = _settings.Player2PaddleStartPosition;
        }

        private GameObject CreatePlayerPaddle(PlayerId playerId, IInputService inputService)
        {
            var paddle = CreatePaddle(playerId);
            var paddleControlls = new PlayerPaddleControlls(inputService);
            paddle.GetComponent<PaddleMovement>().Construct(_settings.PaddleSpeed, _settings.LevelBoundsY, paddleControlls);

            return paddle;
        }

        private GameObject CreateAIPaddle(PlayerId playerId, GameObject ball)
        {
            var paddle = CreatePaddle(playerId);
            var difficulty = _staticDataService.GetAIConfig(_sceneParams.Difficulty);
            var paddleControlls = new AIPaddleControlls(
                Ball, 
                paddle, 
                _settings.LevelBoundsY,
                difficulty.ReactionDelay,
                difficulty.PredictionError,
                difficulty.CalculateWithBounces
            );
            paddle.GetComponent<PaddleMovement>().Construct(
                _settings.PaddleSpeed * difficulty.SpeedMultiplier, 
                _settings.LevelBoundsY
                , paddleControlls);

            return paddle;
        }

        private GameObject CreatePaddle(PlayerId playerId)
        {
            Vector3 position = playerId == PlayerId.Player1 ? _settings.Player1PaddleStartPosition : _settings.Player2PaddleStartPosition;
            Vector3 rotation = playerId == PlayerId.Player1 ? _settings.Player1PaddleRotation : _settings.Player2PaddleRotation;
            GameObject paddle = _assetProvider.Instantiate(AssetPath.PADDLE, position, Quaternion.Euler(rotation));
            paddle.GetComponent<PaddleID>().PlayerId = playerId;
            
            return paddle;
        }
    }
}