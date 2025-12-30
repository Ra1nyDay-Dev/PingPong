using System;
using System.Collections;
using PingPong.Scripts.Global.Services.CoroutineRunner;
using PingPong.Scripts.Global.Services.GameMusicPlayer;
using PingPong.Scripts.Global.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace PingPong.Scripts.Global.Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly GameUI _gameUI;

        public SceneLoader(ICoroutineRunner coroutineRunner, GameUI gameUI)
        {
            _coroutineRunner = coroutineRunner;
            _gameUI = gameUI;
        }
        
        public void Load(string name, Action onLoaded = null) => 
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        public void Load<TSceneParams>(string name, TSceneParams sceneParams, Action onLoaded = null) where TSceneParams : class, ISceneParams => 
            _coroutineRunner.StartCoroutine(LoadScene(name, sceneParams, onLoaded));

        private IEnumerator LoadScene(string name, Action onLoaded = null)
        {
            _gameUI.ShowLoadingScreen();
            ProjectServices.Container.Get<IGameMusicPlayer>().Stop();
            
            SceneManager.LoadScene(Data.Scenes.BOOT);
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(name);

            while (!waitNextScene.isDone)
                yield return null;

            if (name != Data.Scenes.BOOT)
            {
                var sceneEntryPoint = Object.FindFirstObjectByType<SceneEntryPoint>();
                sceneEntryPoint.Run(_gameUI);
            }

            _gameUI.HideLoadingScreen();
            onLoaded?.Invoke();
        }
        
        private IEnumerator LoadScene<TSceneParams>(string name, TSceneParams sceneParams, Action onLoaded = null) where TSceneParams : class, ISceneParams
        {
            _gameUI.ShowLoadingScreen();
            ProjectServices.Container.Get<IGameMusicPlayer>().Stop();
            
            SceneManager.LoadScene(Data.Scenes.BOOT);
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(name);

            while (!waitNextScene.isDone)
                yield return null;

            if (name != Data.Scenes.BOOT)
            {
                var sceneEntryPoint = Object.FindFirstObjectByType<SceneEntryPoint<TSceneParams>>();
                sceneEntryPoint.Run(_gameUI, sceneParams);
            }

            _gameUI.HideLoadingScreen();
            onLoaded?.Invoke();
        }
    }
}

    