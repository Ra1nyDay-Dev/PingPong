using PingPong.Scripts.Global;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.GameMusicPlayer;
using PingPong.Scripts.Global.Services.StaticData;
using PingPong.Scripts.Global.UI;

namespace PingPong.Scripts.Scenes.Menu
{
    public class MenuEntryPoint : SceneEntryPoint
    {
        public override void Run(GameUI gameUI)
        {
            base.Run(gameUI);
            ProjectServices.Container.Get<IGameMusicPlayer>()
                .PlayPlaylist(PlaylistsNames.MENU, false, true);
        }
    }
}
