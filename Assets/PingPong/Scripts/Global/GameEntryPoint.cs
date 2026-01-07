using PingPong.Scripts.Global.AssetManagement;
using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.CoroutineRunner;
using PingPong.Scripts.Global.Services.GameMusicPlayer;
using PingPong.Scripts.Global.Services.Input;
using PingPong.Scripts.Global.Services.SceneLoader;
using PingPong.Scripts.Global.Services.StaticData;
using PingPong.Scripts.Global.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace PingPong.Scripts.Global
{
    public class GameEntryPoint
    {
        private const string FIRST_SCENE = Data.Scenes.MENU;

        private static GameEntryPoint _instance;
        private ISceneLoader _sceneLoader;
        private CoroutineRunner _coroutineRunner;
        private readonly IAssetProvider _assetProvider;
        private readonly GameUI _gameUI;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnApplicationRun()
        {
            _instance = new GameEntryPoint();
            _instance.RunGame();
        }
        
        private GameEntryPoint()
        {
            SceneManager.LoadScene(Data.Scenes.BOOT);
            _assetProvider = new AssetProvider();
            
            _gameUI = _assetProvider.Instantiate(AssetPath.GAME_UI).GetComponent<GameUI>();
            Object.DontDestroyOnLoad(_gameUI.gameObject);
            
            _gameUI.ShowLoadingScreen();
        }


        private void RunGame()
        {
            RegisterProjectServices();
            _sceneLoader.Load(FIRST_SCENE);
        }

        private void RegisterProjectServices()
        {
            RegisterCoroutineRunner();
            ProjectServices.Container.Register<IAssetProvider>(_assetProvider);
            ProjectServices.Container.Register<IStaticDataService>(new StaticDataService());
            ProjectServices.Container.Register<IGameUI>(_gameUI);
            RegisterGameMusicService();
            RegisterSceneLoader();
            ProjectServices.Container.Register<IInputService>($"{PlayerId.Player1}", new UnityPlayerInput(PlayerId.Player1));
            ProjectServices.Container.Register<IInputService>($"{PlayerId.Player2}", new UnityPlayerInput(PlayerId.Player2));
        }

        private void RegisterCoroutineRunner()
        {
            _coroutineRunner = new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
            ProjectServices.Container.Register<ICoroutineRunner>(_coroutineRunner);
            Object.DontDestroyOnLoad(_coroutineRunner.gameObject);
        }

        private void RegisterSceneLoader()
        {
            _sceneLoader = new SceneLoader(_coroutineRunner, _gameUI);
            ProjectServices.Container.Register<ISceneLoader>(_sceneLoader);
        }

        private void RegisterGameMusicService()
        {
            var gameMusicPlayer = _assetProvider.Instantiate(AssetPath.GAME_MUSIC_PLAYER).GetComponent<GameMusicPlayer>();
            Object.DontDestroyOnLoad(gameMusicPlayer.gameObject);
            ProjectServices.Container.Register<IGameMusicPlayer>(gameMusicPlayer);
        }
    }
}