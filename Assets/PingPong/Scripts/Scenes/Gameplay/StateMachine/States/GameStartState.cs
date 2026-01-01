using PingPong.Scripts.Scenes.Gameplay.Services.GameplayFactory;
using PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter;

namespace PingPong.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class GameStartState : IGameplayState
    {
        private readonly IGameplayStateMachine _stateMachine;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly IScoreCounter _scoreCounter;

        public GameStartState(IGameplayStateMachine stateMachine, IScoreCounter scoreCounter)
        {
            _stateMachine = stateMachine;
            _scoreCounter = scoreCounter;
        }
        
        public void Enter()
        {
            ResetGameScore();
            _stateMachine.Enter<RoundStartState>();
        }

        public void Exit()
        {
        }

        private void ResetGameScore() => 
            _scoreCounter.Reset();
    }
}