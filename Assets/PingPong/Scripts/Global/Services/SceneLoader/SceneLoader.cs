using System;
using System.Collections;
using PingPong.Scripts.Global.Services.CoroutineRunner;
using PingPong.Scripts.Global.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace PingPong.Scripts.Global.Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly UIRootView _uiRoot;

        public SceneLoader(ICoroutineRunner coroutineRunner, UIRootView uiRoot)
        {
            _coroutineRunner = coroutineRunner;
            _uiRoot = uiRoot;
        }

        public void Load(string name, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        private IEnumerator LoadScene(string name, Action onLoaded = null)
        {
            _uiRoot.ShowLoadingScreen();
            
            SceneManager.LoadScene(Data.Scenes.BOOT);
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(name);

            while (!waitNextScene.isDone)
                yield return null;

            if (name != Data.Scenes.BOOT)
            {
                var sceneEntryPoint = Object.FindFirstObjectByType<SceneEntryPoint>();
                sceneEntryPoint.Run(_uiRoot);
            }

            _uiRoot.HideLoadingScreen();
            onLoaded?.Invoke();
        }
    }
}

    