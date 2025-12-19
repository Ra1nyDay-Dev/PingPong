using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Scenes.Gameplay.Ball;
using PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter;
using PingPong.Scripts.Scenes.Gameplay.StaticData;
using PingPong.Scripts.Scenes.Gameplay.UI;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class RoundEndState : IPayloadedGameplayState<PlayerId>
    {
        private const string LEVEL_SETTINGS = "StaticData/Gameplay/GameplayLevelSettings";
        
        public void Enter(PlayerId playerLosed)
        {
            var settings = Resources.Load<GameplayLevelSettings>(LEVEL_SETTINGS);
            
            var score = SceneServices.Container.Get<IScoreCounter>();
            score.UpdateScore(playerLosed);
            
            SceneServices.Container.Get<IGameplayUI>().UpdateUI();
            GameObject.FindWithTag("Ball").GetComponent<BallMovement>().StopBall();
            
            bool isGameOver = score.ScorePlayer1 == settings.ScoreToWin || score.ScorePlayer2 == settings.ScoreToWin;
            
            if (isGameOver)
                SceneServices.Container.Get<IGameplayStateMachine>().Enter<GameOverState>();
            else
                SceneServices.Container.Get<IGameplayStateMachine>().Enter<RoundStartState>();
        }

        public void Exit()
        {
            
        }
    }
}