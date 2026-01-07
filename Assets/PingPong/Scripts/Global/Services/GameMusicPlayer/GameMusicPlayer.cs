using System;
using System.Collections.Generic;
using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services.StaticData;
using PingPong.Scripts.Global.UI;
using UnityEngine;

namespace PingPong.Scripts.Global.Services.GameMusicPlayer
{
    [RequireComponent(typeof(AudioSource))]
    public class GameMusicPlayer : MonoBehaviour, IGameMusicPlayer
    {
        public MusicTrack CurrentTrack { get;  private set; }
        public MusicPlaylist CurrentPlaylist { get; private set; }

        private AudioSource _audioSource;
        
        private readonly List<MusicTrack> _playlistQueue = new();
        private int _currentIndex = -1;

        private bool _isPaused;
        private bool _repeatPlaylist;
        private Coroutine _trackEndCoroutine;
        private IStaticDataService _staticDataService;
        private bool _wasPlayingBeforeFocusLoss = false;

        public event Action<MusicTrack> TrackStartPlay;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _staticDataService = ProjectServices.Container.Get<IStaticDataService>();
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            HandleFocusChange(hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            HandleFocusChange(!pauseStatus);
        }

        private void HandleFocusChange(bool hasFocus)
        {
            if (hasFocus)
            {
                if (_wasPlayingBeforeFocusLoss) 
                    Resume();
            }
            else
            {
                _wasPlayingBeforeFocusLoss = _audioSource.isPlaying && !_isPaused;
                if (_wasPlayingBeforeFocusLoss) 
                    Pause();
            }
        }

        public void PlayTrack(string trackFileName, bool repeat)
        {
            if (string.IsNullOrEmpty(trackFileName))
                throw new ArgumentNullException(nameof(trackFileName));
            
            MusicTrack track = _staticDataService.GetTrack(trackFileName);
            
            Clear();
            _playlistQueue.Add(track);
            CurrentTrack = track;
            _repeatPlaylist = repeat;
            PlayNext();
        }

        public void PlayPlaylist(string playlistFileName, bool shuffle, bool repeat)
        {
            if (string.IsNullOrEmpty(playlistFileName))
                throw new ArgumentNullException(nameof(playlistFileName));
            
            MusicPlaylist playlist = _staticDataService.GetPlaylist(playlistFileName);
            
            if (playlist?.Tracks == null || playlist.Tracks.Length == 0) 
                throw new Exception($"No tracks found in playlist {playlistFileName}");
            
            Clear();
            _playlistQueue.AddRange(playlist.Tracks);
            _repeatPlaylist = repeat;

            if (shuffle)
                _playlistQueue.Shuffle();
            
            PlayNext();
        }

        public void Resume()
        {
            if (_audioSource.clip != null && _isPaused)
            {
                _audioSource.UnPause();
                StartTrackEndCheck();
                _isPaused = false;
            }
        }

        public void Pause()
        {
            _audioSource.Pause();
            StopTrackEndCheck();
            _isPaused = true;
        }

        public void Stop()
        {
            _audioSource.Stop();
            StopTrackEndCheck();
            _currentIndex = -1;
            CurrentTrack = null;
            _isPaused = false;
        }

        public void PlayNext()
        {
            if (_playlistQueue.Count == 0)
                return;

            if (_currentIndex + 1 < _playlistQueue.Count)
            {
                _currentIndex++;
                PlayAudioclip();
            }                
            else
            {
                if (_repeatPlaylist)
                {
                    _currentIndex = -1;
                    PlayNext();
                }
            }
        }

        public void PlayPrevious()
        {
            if (_playlistQueue.Count == 0)
                return;
            
            if (_currentIndex - 1 >= 0)
            {
                _currentIndex--;
                PlayAudioclip();
            }                
            else
            {
                if (_repeatPlaylist)
                {
                    _currentIndex = _playlistQueue.Count - 1;
                    PlayAudioclip();
                }
                else
                {
                    _currentIndex = -1;
                    PlayNext();
                }
            }
        }

        public void SetVolume(float volume) => 
            _audioSource.volume = Mathf.Clamp01(volume);

        public void Clear()
        {
            Stop();
            _playlistQueue.Clear();
            CurrentTrack = null;
            CurrentPlaylist = null;
            _currentIndex = -1;
            _isPaused = false;
        }

        private void PlayAudioclip()
        {
            CurrentTrack = _playlistQueue[_currentIndex];
            _audioSource.clip = CurrentTrack.Clip;
            _audioSource.Play();
            ProjectServices.Container.Get<IGameUI>().ShowMusicPopUp(CurrentTrack); // toDo: переписать на события
            TrackStartPlay?.Invoke(CurrentTrack);
            StartTrackEndCheck();
        }

        private void StartTrackEndCheck()
        {
            StopTrackEndCheck();
            
            if (_audioSource.clip != null && _audioSource.isPlaying)
            {
                _trackEndCoroutine = StartCoroutine(TrackEndCheckRoutine());
            }
        }
        
        private void StopTrackEndCheck()
        {
            if (_trackEndCoroutine != null)
            {
                StopCoroutine(_trackEndCoroutine);
                _trackEndCoroutine = null;
            }
        }
        
        private System.Collections.IEnumerator TrackEndCheckRoutine()
        {
            while (_audioSource.isPlaying && _audioSource.time < _audioSource.clip.length)
                yield return null;
            
            PlayNext();
        }
    }
}