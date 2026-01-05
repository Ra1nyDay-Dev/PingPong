using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services.GameMusicPlayer;
using PingPong.Scripts.Scenes.Gameplay.StaticData;
using UnityEngine;

namespace PingPong.Scripts.Global.Services.StaticData
{
    public interface IStaticDataService : IProjectService
    {
        MusicTrack GetTrack(string trackFileName);
        MusicPlaylist GetPlaylist(string trackFileName);
        LevelSettings GetSettings(string sceneName, string settingsName);
        AIDifficultyConfig GetAIConfig(AIDifficulty difficulty);
    }
}