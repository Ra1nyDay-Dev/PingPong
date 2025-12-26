using UnityEngine;

namespace PingPong.Scripts.Global.Services.GameMusicPlayer
{
    [CreateAssetMenu(fileName = "MusicTrack", menuName = "Audio/Music Track")]
    public class MusicTrack : ScriptableObject
    {
        public string Artist;
        public string TrackName;
        public AudioClip Clip;
        public bool ShowInPopup;
    }
}