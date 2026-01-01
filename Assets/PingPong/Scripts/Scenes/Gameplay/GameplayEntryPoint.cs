using PingPong.Scripts.Global;
using PingPong.Scripts.Global.AssetManagement;
using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.CoroutineRunner;
using PingPong.Scripts.Global.Services.Input;
using PingPong.Scripts.Global.Services.StaticData;
using PingPong.Scripts.Global.UI;
using PingPong.Scripts.Scenes.Gameplay.Services.GameplayFactory;
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
        private IAssetProvider _assetProvider;
        private IStaticDataService _staticDataService;
        private IInputService _player1InputService;
        private IInputService _player2InputService;
        private ICoroutineRunner _coroutineRunner;

        public override void Run(GameUI gameUI, GameplayEntryParams sceneParams)
        {
            base.Run(gameUI, sceneParams);
            GetProjectDependencies();
            RegisterSceneServices();
            PrepareLevel();
            StartGame();
        }

        private void GetProjectDependencies()
        {
            _assetProvider = ProjectServices.Container.Get<IAssetProvider>();
            _staticDataService = ProjectServices.Container.Get<IStaticDataService>();
            _player1InputService = ProjectServices.Container.Get<IInputService>($"{PlayerId.Player1}");
            _player2InputService = ProjectServices.Container.Get<IInputService>($"{PlayerId.Player2}");
            _coroutineRunner = ProjectServices.Container.Get<ICoroutineRunner>();
        }

        private void RegisterSceneServices()
        {
            SceneServices.Container.Register<IGameplayUI>(_sceneUI.GetComponent<GameplayUI>());
            SceneServices.Container.Register<IRoundTimer>(_sceneUI.GetComponentInChildren<RoundTimer>());
            SceneServices.Container.Register<IScoreCounter>(_sceneUI.GetComponentInChildren<ScoreCounter>());
            SceneServices.Container.Register<ILightController>(GameObject.FindFirstObjectByType<LightController>());
            SceneServices.Container.Register<IGameplayFactory>(new GameplayFactory(_assetProvider, _staticDataService, 
                _player1InputService,_player2InputService, _sceneParams.GameVersusMode));
            RegisterGameplayStateMachine();
        }

        private void RegisterGameplayStateMachine()
        {
            var gameplayFactory = SceneServices.Container.Get<IGameplayFactory>();
            var scoreCounter = SceneServices.Container.Get<IScoreCounter>();
            var roundTimer = SceneServices.Container.Get<IRoundTimer>();
            var lightController = SceneServices.Container.Get<ILightController>();
            var gameplayUI = SceneServices.Container.Get<IGameplayUI>();

            SceneServices.Container.Register<IGameplayStateMachine>(new GameplayStateMachine(gameplayFactory, _staticDataService,
                scoreCounter, roundTimer, lightController, gameplayUI, _coroutineRunner));
        }

        private void PrepareLevel() => 
            SceneServices.Container.Get<IGameplayFactory>().CreateLevelObjects();

        private void StartGame() => 
            SceneServices.Container.Get<IGameplayStateMachine>().Enter<GameStartState>();
    }
}
