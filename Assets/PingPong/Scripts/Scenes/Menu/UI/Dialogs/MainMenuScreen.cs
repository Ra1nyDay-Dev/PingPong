using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.SceneLoader;
using PingPong.Scripts.Global.UI;
using PingPong.Scripts.Scenes.Gameplay;
using PingPong.Scripts.Scenes.Gameplay.StateMachine;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Menu.UI.Dialogs
{
    public class MainMenuScreen : SceneDialog
    {
        private ISceneLoader _sceneLoader;

        private void Awake()
        {
            _sceneLoader = ProjectServices.Container.Get<ISceneLoader>();
        }

        public void OnSingleplayerButtonClicked() => 
            _sceneUI.SwitchScreen<SingleplayerMenuScreen>();
        
        public void OnMultiplayerButtonClicked()
        {
            var sceneParams = new GameplayEntryParams(GameVersusMode.PlayerVsPlayer);
            _sceneLoader.Load(Global.Data.Scenes.GAMEPLAY, sceneParams);
        }
        
        public void OnSettingsButtonClicked() => 
            _sceneUI.SwitchScreen<SettingsMenuScreen>();
        
        public void OnAboutButtonClicked() => 
            _sceneUI.SwitchScreen<AboutMenuScreen>();
    }
}
