using System;
using System.Collections.Generic;
using PingPong.Scripts.Global.UI;
using PingPong.Scripts.Scenes.Menu.UI.Dialogs;

namespace PingPong.Scripts.Scenes.Menu.UI
{
    public class MenuUI : SceneUI, IMenuUI
    {
        private void Awake()
        {
            _sceneDialogsPrefabs = new Dictionary<Type, string>()
            {
                {typeof(MainMenuScreen), "Menu/UI/Dialogs/MainMenu"},
                {typeof(SingleplayerMenuScreen), "Menu/UI/Dialogs/SingleplayerMenu"},
                {typeof(SettingsMenuScreen), "Menu/UI/Dialogs/SettingsMenu"},
                {typeof(AboutMenuScreen), "Menu/UI/Dialogs/AboutMenu"},
            };
        }
    }
}
