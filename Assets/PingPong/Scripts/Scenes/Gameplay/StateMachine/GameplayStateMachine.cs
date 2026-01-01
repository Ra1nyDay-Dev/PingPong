using System;
using System.Collections.Generic;
using PingPong.Scripts.Global.Services.CoroutineRunner;
using PingPong.Scripts.Global.Services.StaticData;
using PingPong.Scripts.Global.StateMachine;
using PingPong.Scripts.Scenes.Gameplay.Services.GameplayFactory;
using PingPong.Scripts.Scenes.Gameplay.Services.LightController;
using PingPong.Scripts.Scenes.Gameplay.Services.RoundTimer;
using PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter;
using PingPong.Scripts.Scenes.Gameplay.StateMachine.States;
using PingPong.Scripts.Scenes.Gameplay.UI;

namespace PingPong.Scripts.Scenes.Gameplay.StateMachine
{
    public class GameplayStateMachine : GameStateMachine, IGameplayStateMachine
    {
        public GameplayStateMachine(IGameplayFactory gameplayFactory, IStaticDataService staticDataService,
            IScoreCounter scoreCounter, IRoundTimer roundTimer, ILightController lightController,
            IGameplayUI gameplayUI, ICoroutineRunner coroutineRunner)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(GameStartState)] = new GameStartState(this, scoreCounter),
                [typeof(RoundStartState)] = new RoundStartState(this, gameplayFactory, staticDataService, scoreCounter,
                    roundTimer, lightController, gameplayUI, coroutineRunner),
                [typeof(GameLoopState)] = new GameLoopState(),
                [typeof(RoundEndState)] = new RoundEndState(this, gameplayFactory, staticDataService, scoreCounter,
                    roundTimer, lightController, coroutineRunner),
                [typeof(GameOverState)] = new GameOverState(gameplayFactory),
            };
        }
    }
}