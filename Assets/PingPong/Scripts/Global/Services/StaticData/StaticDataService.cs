using PingPong.Scripts.Global.Services.GameMusicPlayer;
using PingPong.Scripts.Scenes.Gameplay.StaticData;
using UnityEngine;

namespace PingPong.Scripts.Global.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string TRACKS_PATH = "Game/StaticData/Tracks";
        private const string PLAYLISTS_PATH = "Game/StaticData/Playlists";

        public MusicTrack GetTrack(string trackFileName) => 
            Resources.Load<MusicTrack>($"{TRACKS_PATH}/{trackFileName}");
        
        public MusicPlaylist GetPlaylist(string trackFileName) => 
            Resources.Load<MusicPlaylist>($"{PLAYLISTS_PATH}/{trackFileName}");
        
        public LevelSettings GetSettings(string sceneName, string settingsName)=> 
            Resources.Load<LevelSettings>($"{sceneName}/StaticData/{settingsName}");
    }
}