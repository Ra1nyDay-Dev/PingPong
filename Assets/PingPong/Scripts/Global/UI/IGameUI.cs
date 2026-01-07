using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.GameMusicPlayer;
using UnityEngine;

namespace PingPong.Scripts.Global.UI
{
    public interface IGameUI : IProjectService
    {
        void AttachSceneUI(GameObject sceneUI);
        void ShowMusicPopUp(MusicTrack track);
    }
}