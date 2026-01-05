using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services.GameMusicPlayer;
using PingPong.Scripts.Scenes.Gameplay.StaticData;
using UnityEngine;

namespace PingPong.Scripts.Global.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string TRACKS_PATH = "Game/StaticData/Tracks/";
        private const string PLAYLISTS_PATH = "Game/StaticData/Playlists/";
        private const string AI_CONFIGS_PATH = "Gameplay/StaticData/AIDifficultyConfigs/";

        public MusicTrack GetTrack(string trackFileName) => 
            Resources.Load<MusicTrack>($"{TRACKS_PATH}{trackFileName}");
        
        public MusicPlaylist GetPlaylist(string trackFileName) => 
            Resources.Load<MusicPlaylist>($"{PLAYLISTS_PATH}{trackFileName}");
        
        public LevelSettings GetSettings(string sceneName, string settingsName)=> 
            Resources.Load<LevelSettings>($"{sceneName}/StaticData/{settingsName}");

        public AIDifficultyConfig GetAIConfig(AIDifficulty difficulty) =>
            Resources.Load<AIDifficultyConfig>($"{AI_CONFIGS_PATH}{difficulty.ToString()}");
    }
}