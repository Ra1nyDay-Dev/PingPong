using PingPong.Scripts.Scenes.Gameplay.Ball;
using PingPong.Scripts.Scenes.Gameplay.Paddle;
using PingPong.Scripts.Scenes.Gameplay.Services.GameplayFactory;

namespace PingPong.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class GameOverState : IGameplayState
    {
        private readonly IGameplayFactory _gameplayFactory;

        public GameOverState(IGameplayFactory gameplayFactory)
        {
            _gameplayFactory = gameplayFactory;
        }

        public void Enter()
        {
            DisableMovement();
        }

        public void Exit()
        {
        }
        
        private void DisableMovement()
        {
            _gameplayFactory.Player1Paddle.GetComponent<PaddleMovement>().enabled = false;
            _gameplayFactory.Player2Paddle.GetComponent<PaddleMovement>().enabled = false;
            _gameplayFactory.Ball.GetComponent<BallMovement>().StopBall();
        }
    }
}