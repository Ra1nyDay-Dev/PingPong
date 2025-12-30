using PingPong.Scripts.Global.Services;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Menu.UI.Dialogs
{
    public class SettingsMenuDialog : MonoBehaviour
    {
        private IMenuUI _menuUI;

        private void Awake() => 
            _menuUI = SceneServices.Container.Get<IMenuUI>();

        public void OnBackButtonClicked() => 
            _menuUI.ShowDialog(MenuDialogsPath.MAIN_MENU);
    }
}
