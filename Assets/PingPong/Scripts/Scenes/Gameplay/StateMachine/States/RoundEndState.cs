using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Scenes.Gameplay.Ball;
using PingPong.Scripts.Scenes.Gameplay.Services.RoundTimer;
using PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter;
using PingPong.Scripts.Scenes.Gameplay.StaticData;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class RoundEndState : IPayloadedGameplayState<PlayerId>
    {
        private const string LEVEL_SETTINGS = "Gameplay/StaticData/GameplayLevelSettings";
        
        private GameplayLevelSettings _settings;
        private IScoreCounter _score;
        private GameObject _ball;
        private IGameplayStateMachine _stateMachine;
        private IRoundTimer _roundTimer;

        public void Enter(PlayerId playerLosed)
        {
            GetStateDependencies();
            _score.UpdateScore(playerLosed);
            _roundTimer.StopTimer();
            _ball.GetComponent<BallMovement>().StopBall();
            CheckGameOver();
        }

        public void Exit()
        {
            
        }

        private void GetStateDependencies()
        {
            _settings = Resources.Load<GameplayLevelSettings>(LEVEL_SETTINGS);
            _score = SceneServices.Container.Get<IScoreCounter>();
            _ball = GameObject.FindWithTag("Ball");
            _stateMachine = SceneServices.Container.Get<IGameplayStateMachine>();
            _roundTimer = SceneServices.Container.Get<IRoundTimer>();
        }

        private void CheckGameOver()
        {
            bool isGameOver = _score.ScorePlayer1 == _settings.ScoreToWin || _score.ScorePlayer2 == _settings.ScoreToWin;

            if (isGameOver)
                _stateMachine.Enter<GameOverState>();
            else
                _stateMachine.Enter<RoundStartState>();
        }
    }
}