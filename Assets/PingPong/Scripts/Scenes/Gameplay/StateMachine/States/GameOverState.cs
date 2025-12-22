using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Scenes.Gameplay.Paddle;
using PingPong.Scripts.Scenes.Gameplay.StaticData;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class GameOverState : IGameplayState
    {
        private const string LEVEL_SETTINGS = "Gameplay/StaticData/GameplayLevelSettings";
        
        public void Enter()
        {
            var paddles = GameObject.FindGameObjectsWithTag("Paddle");
            var settings = Resources.Load<GameplayLevelSettings>(LEVEL_SETTINGS);
            
            foreach (GameObject paddle in paddles)
            {
                paddle.GetComponent<PaddleMovement>().enabled = false;
                paddle.transform.position = paddle.GetComponent<PaddleID>().PlayerId == PlayerId.Player1 
                    ? settings.Player1PaddleStartPosition 
                    : settings.Player2PaddleStartPosition;
            }
        }

        public void Exit()
        {
        }
    }
}