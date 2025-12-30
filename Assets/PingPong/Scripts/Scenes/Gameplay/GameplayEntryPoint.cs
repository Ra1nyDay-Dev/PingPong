using PingPong.Scripts.Global;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.UI;
using PingPong.Scripts.Scenes.Gameplay.Services.LightController;
using PingPong.Scripts.Scenes.Gameplay.Services.RoundTimer;
using PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter;
using PingPong.Scripts.Scenes.Gameplay.StateMachine;
using PingPong.Scripts.Scenes.Gameplay.StateMachine.States;
using PingPong.Scripts.Scenes.Gameplay.UI;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay
{
    public class GameplayEntryPoint : SceneEntryPoint<GameplayEntryParams>
    {
        public override void Run(GameUI gameUI, GameplayEntryParams sceneParams)
        {
            base.Run(gameUI, sceneParams);
            RegisterSceneServices();
            StartGame();
        }

        private void RegisterSceneServices()
        {
            SceneServices.Container.Register<IGameplayUI>(_sceneUI.GetComponent<GameplayUI>());
            SceneServices.Container.Register<IRoundTimer>(_sceneUI.GetComponentInChildren<RoundTimer>());
            SceneServices.Container.Register<IScoreCounter>(_sceneUI.GetComponentInChildren<ScoreCounter>());
            SceneServices.Container.Register<ILightController>(GameObject.FindFirstObjectByType<LightController>());
            SceneServices.Container.Register<IGameplayStateMachine>(new GameplayStateMachine());
        }

        private void StartGame() => 
            SceneServices.Container.Get<IGameplayStateMachine>().Enter<GameStartState, GameplayEntryParams>(_sceneParams);
    }
}
