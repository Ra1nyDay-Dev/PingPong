using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.SceneLoader;
using PingPong.Scripts.Global.UI;
using PingPong.Scripts.Scenes.Gameplay;

namespace PingPong.Scripts.Scenes.Menu.UI.Dialogs
{
    public class SingleplayerMenuScreen : SceneDialog
    {
        private ISceneLoader _sceneLoader;

        private void Awake() => 
            _sceneLoader = ProjectServices.Container.Get<ISceneLoader>();

        public void OnEasyButtonClicked()
        {
            var sceneParams = new GameplayEntryParams(GameVersusMode.PlayerVsAI);
            _sceneLoader.Load(Global.Data.Scenes.GAMEPLAY, sceneParams);
        }

        public void OnBackButtonClicked() => 
            _sceneUI.SwitchScreen<MainMenuScreen>();
    }
}
