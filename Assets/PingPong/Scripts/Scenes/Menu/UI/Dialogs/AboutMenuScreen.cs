using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.UI;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Menu.UI.Dialogs
{
    public class AboutMenuScreen : SceneDialog
    {
        public void OnBackButtonClicked() => 
            _sceneUI.SwitchScreen<MainMenuScreen>();
    }
}
