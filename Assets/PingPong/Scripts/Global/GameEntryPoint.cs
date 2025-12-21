using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.CoroutineRunner;
using PingPong.Scripts.Global.Services.Input;
using PingPong.Scripts.Global.Services.SceneLoader;
using PingPong.Scripts.Global.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PingPong.Scripts.Global
{
    public class GameEntryPoint
    {
        private const string FIRST_SCENE = Data.Scenes.GAMEPLAY;
        private const string UI_ROOT_PATH = "UI/Root/UIRoot";

        private static GameEntryPoint _instance;
        private ISceneLoader _sceneLoader;
        private readonly CoroutineRunner _coroutineRunner;
        private readonly UIRootView _uiRoot;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnApplicationRun()
        {
            _instance = new GameEntryPoint();
            _instance.RunGame();
        }
        
        private GameEntryPoint()
        {
            _coroutineRunner = new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
            Object.DontDestroyOnLoad(_coroutineRunner.gameObject);
            
            var prefabUIRoot = Resources.Load<UIRootView>(UI_ROOT_PATH);
            _uiRoot = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(_uiRoot.gameObject);
            
            _sceneLoader = new SceneLoader(_coroutineRunner, _uiRoot);
        }
        
        
        private void RunGame()
        {
            _sceneLoader.Load(Data.Scenes.BOOT);
            RegisterProjectServices();
            _sceneLoader.Load(FIRST_SCENE);
        }

        private void RegisterProjectServices()
        {
            ProjectServices.Container.Register<ISceneLoader>(_sceneLoader);
            ProjectServices.Container.Register<IInputService>($"{PlayerId.Player1}", new UnityPlayerInput(PlayerId.Player1));
            ProjectServices.Container.Register<IInputService>($"{PlayerId.Player2}", new UnityPlayerInput(PlayerId.Player2));
            ProjectServices.Container.Register<ICoroutineRunner>(_coroutineRunner);
        }
    }
}
