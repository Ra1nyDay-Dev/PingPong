using PingPong.Scripts.Global;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.GameMusicPlayer;
using PingPong.Scripts.Global.Services.SceneLoader;
using PingPong.Scripts.Global.Services.StaticData;
using PingPong.Scripts.Global.UI;
using PingPong.Scripts.Scenes.Menu.UI;

namespace PingPong.Scripts.Scenes.Menu
{
    public class MenuEntryPoint : SceneEntryPoint
    {
        public override void Run(GameUI gameUI)
        {
            base.Run(gameUI);
            RegisterSceneServices();
            SceneServices.Container.Get<IMenuUI>().ShowDialog(MenuDialogsPath.MAIN_MENU);
            // ProjectServices.Container.Get<IGameMusicPlayer>()
            //     .PlayPlaylist(PlaylistsNames.MENU, false, true);
        }

        private void RegisterSceneServices()
        {
            SceneServices.Container.Register<IMenuUI>(_sceneUI.GetComponent<MenuUI>());
        }
    }
}
