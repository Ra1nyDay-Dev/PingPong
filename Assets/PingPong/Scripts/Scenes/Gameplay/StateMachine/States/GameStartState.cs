using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.Input;
using PingPong.Scripts.Global.UI;
using PingPong.Scripts.Scenes.Gameplay.Ball;
using PingPong.Scripts.Scenes.Gameplay.Paddle;
using PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter;
using PingPong.Scripts.Scenes.Gameplay.StaticData;
using PingPong.Scripts.Scenes.Gameplay.UI;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class GameStartState : IGameplayState
    {
        private const string LEVEL_SETTINGS = "StaticData/Gameplay/GameplayLevelSettings";
        private const string PADDLE_PREFAB = "Gameplay/Paddle/Paddle";
        private const string BALL_PREFAB = "Gameplay/Ball/Ball";

        public void Enter()
        {
            PrepareLevel();
            ResetScore();
            SceneServices.Container.Get<IGameplayStateMachine>().Enter<RoundStartState>();
        }

        public void Exit()
        {
        }

        private void PrepareLevel()
        {
            GameplayLevelSettings settings = Resources.Load<GameplayLevelSettings>(LEVEL_SETTINGS);
            GameObject paddlePrefab = Resources.Load<GameObject>(PADDLE_PREFAB);
            GameObject ballPrefab = Resources.Load<GameObject>(BALL_PREFAB);
            
            CreatePaddle(paddlePrefab, settings, PlayerId.Player1, PaddleInputType.Player1);
            CreatePaddle(paddlePrefab, settings, PlayerId.Player2, PaddleInputType.Player2);
            CreateBall(ballPrefab, settings);
        }


        private static GameObject CreatePaddle(GameObject paddlePrefab,GameplayLevelSettings settings, PlayerId playerId, PaddleInputType inputType)
        {
            Vector3 position = playerId == PlayerId.Player1 ? settings.Player1PaddleStartPosition : settings.Player2PaddleStartPosition;
            IInputService inputService = ProjectServices.Container.Get<IInputService>($"{inputType}");
            GameObject playerPaddle = Object.Instantiate(paddlePrefab, position, Quaternion.identity) as GameObject;
            playerPaddle.GetComponent<PaddleID>().PlayerId = playerId;
            playerPaddle.GetComponent<PaddleMovement>().Construct(settings.PaddleSpeed, inputType, settings.LevelBoundsY, inputService);

            return playerPaddle;
        }

        private static void CreateBall(GameObject ballPrefab, GameplayLevelSettings settings)
        {
            GameObject ball = Object.Instantiate(ballPrefab, settings.BallStartPosition, Quaternion.identity);
            ball.GetComponent<BallMovement>().Construct
            ( 
                launchSpeed: settings.BallLaunchSpeed,
                maxLaunchAngle: settings.BallMaxLaunchAngle
            );
        }

        private void ResetScore()
        {
            SceneServices.Container.Get<IScoreCounter>().Reset();
            SceneServices.Container.Get<IGameplayUI>().Reset();
        }
    }
}