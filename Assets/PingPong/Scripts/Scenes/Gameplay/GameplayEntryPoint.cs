using PingPong.Scripts.Global;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.UI;
using PingPong.Scripts.Scenes.Gameplay.Services.RoundTimer;
using PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter;
using PingPong.Scripts.Scenes.Gameplay.StateMachine;
using PingPong.Scripts.Scenes.Gameplay.StateMachine.States;
using PingPong.Scripts.Scenes.Gameplay.UI;

namespace PingPong.Scripts.Scenes.Gameplay
{
    public class GameplayEntryPoint : SceneEntryPoint
    {
        public override void Run(UIRootView uiRoot)
        {
            base.Run(uiRoot);
            RegisterSceneServices();
            StartGame();
        }

        private void RegisterSceneServices()
        {
            SceneServices.Container.Register<IGameplayUI>(_sceneUI.GetComponent<IGameplayUI>());
            SceneServices.Container.Register<IRoundTimer>(_sceneUI.GetComponentInChildren<IRoundTimer>());
            SceneServices.Container.Register<IScoreCounter>(_sceneUI.GetComponentInChildren<IScoreCounter>());
            SceneServices.Container.Register<IGameplayStateMachine>(new GameplayStateMachine());
        }

        private void StartGame() => 
            SceneServices.Container.Get<IGameplayStateMachine>().Enter<GameStartState>();
    }
}
