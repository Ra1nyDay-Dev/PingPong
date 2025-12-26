using System;

namespace PingPong.Scripts.Global.Services.GameMusicPlayer
{
    public interface IGameMusicPlayer : IProjectService
    {
        MusicTrack CurrentTrack { get; }
        MusicPlaylist CurrentPlaylist { get; }
        
        event Action<MusicTrack> TrackStartPlay;
        
        void PlayTrack(string trackFileName, bool repeat);
        void PlayPlaylist(string playlistFileName, bool shuffle, bool repeat);
        void Resume();
        void Pause();
        void Stop();
        void PlayNext();
        void PlayPrevious();
        void SetVolume(float volume);
        void Clear();
    }
}