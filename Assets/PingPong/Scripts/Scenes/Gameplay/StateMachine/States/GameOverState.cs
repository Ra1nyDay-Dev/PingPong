using PingPong.Scripts.Scenes.Gameplay.Ball;
using PingPong.Scripts.Scenes.Gameplay.Paddle;
using PingPong.Scripts.Scenes.Gameplay.Services.GameplayFactory;
using PingPong.Scripts.Scenes.Gameplay.UI;
using PingPong.Scripts.Scenes.Gameplay.UI.Dialogs;

namespace PingPong.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class GameOverState : IGameplayState
    {
        private readonly IGameplayFactory _gameplayFactory;
        private readonly IGameplayUI _gameplayUI;

        public GameOverState(IGameplayFactory gameplayFactory, IGameplayUI gameplayUI)
        {
            _gameplayFactory = gameplayFactory;
            _gameplayUI =  gameplayUI;
        }

        public void Enter()
        {
            DisableMovement();
            _gameplayUI.ShowDialog<PlayAgainDialog>();
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