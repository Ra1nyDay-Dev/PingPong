using System.Collections;
using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.CoroutineRunner;
using PingPong.Scripts.Scenes.Gameplay.Ball;
using PingPong.Scripts.Scenes.Gameplay.Paddle;
using PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter;
using PingPong.Scripts.Scenes.Gameplay.StaticData;
using PingPong.Scripts.Scenes.Gameplay.UI;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class RoundStartState : IGameplayState
    {
        private const string LEVEL_SETTINGS = "StaticData/Gameplay/GameplayLevelSettings";
        
        private GameObject _ball;
        private GameObject[] _paddles;

        public void Enter()
        {
            _paddles = GameObject.FindGameObjectsWithTag("Paddle");
            _ball = GameObject.FindWithTag("Ball");
            
            var settings = Resources.Load<GameplayLevelSettings>(LEVEL_SETTINGS);
            var gameplayUI = SceneServices.Container.Get<IGameplayUI>();
            var coroutineRunner = ProjectServices.Container.Get<ICoroutineRunner>();
            var stateMachine = SceneServices.Container.Get<IGameplayStateMachine>();
            var scoreCounter = SceneServices.Container.Get<IScoreCounter>(); 

            _ball.SetActive(false);
            _ball.transform.position = settings.BallStartPosition;

            foreach (GameObject paddle in _paddles)
            {
                paddle.GetComponent<PaddleMovement>().enabled = false;
                paddle.transform.position = paddle.GetComponent<PaddleID>().PlayerId == PlayerId.Player1 
                    ? settings.Player1PaddleStartPosition 
                    : settings.Player2PaddleStartPosition;
            }

            var isFirstRound = scoreCounter.ScorePlayer1 == 0 && scoreCounter.ScorePlayer1 == 0;
            var countdownTime = isFirstRound ? settings.FirstRoundCountdownTime : settings.RoundCountdownTime;
            
            coroutineRunner.StartCoroutine(RoundCountdown(countdownTime, stateMachine));
            gameplayUI.StartRoundCountdown(countdownTime);
        }

        private IEnumerator RoundCountdown(float time, IGameplayStateMachine stateMachine)
        {
            yield return new WaitForSeconds(time);
            stateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
            _ball.SetActive(true);
            
            var paddles = GameObject.FindGameObjectsWithTag("Paddle");
            
            foreach (var paddle in paddles)
                paddle.GetComponent<PaddleMovement>().enabled = true;
            
            _ball.GetComponent<BallMovement>().LaunchBall();
        }
    }
}