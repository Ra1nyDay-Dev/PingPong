using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.SceneLoader;
using PingPong.Scripts.Scenes.Gameplay;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Menu.UI.Dialogs
{
    public class SingleplayerMenuDialog : MonoBehaviour
    {
        private IMenuUI _menuUI;
        private ISceneLoader _sceneLoader;

        private void Awake()
        {
            _menuUI = SceneServices.Container.Get<IMenuUI>();
            _sceneLoader = ProjectServices.Container.Get<ISceneLoader>();
        }

        public void OnEasyButtonClicked()
        {
            var sceneParams = new GameplayEntryParams(GameVersusMode.PlayerVsAI);
            _sceneLoader.Load(Global.Data.Scenes.GAMEPLAY, sceneParams);
        }

        public void OnBackButtonClicked() => 
            _menuUI.ShowDialog(MenuDialogsPath.MAIN_MENU);
    }
}
