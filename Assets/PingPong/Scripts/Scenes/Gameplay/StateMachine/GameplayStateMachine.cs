using System;
using System.Collections.Generic;
using PingPong.Scripts.Global;
using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.StateMachine;
using PingPong.Scripts.Scenes.Gameplay.StateMachine.States;

namespace PingPong.Scripts.Scenes.Gameplay.StateMachine
{
    public class GameplayStateMachine : GameStateMachine, IGameplayStateMachine
    {
        public GameplayStateMachine()
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(GameStartState)] = new GameStartState(),
                [typeof(RoundStartState)] = new RoundStartState(),
                [typeof(GameLoopState)] = new GameLoopState(),
                [typeof(RoundEndState)] = new RoundEndState(),
                [typeof(GameOverState)] = new GameOverState(),
            };
        }
    }
}