using System.Collections.Generic;
using UnityEngine;

namespace PingPong.Scripts.Global.Services.GameMusicPlayer
{
    [CreateAssetMenu(fileName = "MusicPlaylist", menuName = "Audio/Music Playlist")]
    public class MusicPlaylist : ScriptableObject
    {
        public MusicTrack[] Tracks;
    }
}