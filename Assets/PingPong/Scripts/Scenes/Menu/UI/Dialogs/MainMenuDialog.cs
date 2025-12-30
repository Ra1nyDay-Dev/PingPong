using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.SceneLoader;
using PingPong.Scripts.Scenes.Gameplay;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Menu.UI.Dialogs
{
    public class MainMenuDialog : MonoBehaviour
    {
        private IMenuUI _menuUI;
        private ISceneLoader _sceneLoader;

        private void Awake()
        {
            _menuUI = SceneServices.Container.Get<IMenuUI>();
            _sceneLoader = ProjectServices.Container.Get<ISceneLoader>();
        }

        public void OnSingleplayerButtonClicked() => 
            _menuUI.ShowDialog(MenuDialogsPath.SINGLEPLAYER_MENU);
        
        public void OnMultiplayerButtonClicked()
        {
            var sceneParams = new GameplayEntryParams(GameVersusMode.PlayerVsPlayer);
            _sceneLoader.Load(Global.Data.Scenes.GAMEPLAY, sceneParams);
        }
        
        public void OnSettingsButtonClicked() => 
            _menuUI.ShowDialog(MenuDialogsPath.SETTINGS_MENU);
        
        public void OnAboutButtonClicked() => 
            _menuUI.ShowDialog(MenuDialogsPath.ABOUT_MENU);
    }
}
