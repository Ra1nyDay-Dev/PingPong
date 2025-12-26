using System;
using System.Collections.Generic;
using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services.StaticData;
using UnityEngine;

namespace PingPong.Scripts.Global.Services.GameMusicPlayer
{
    [RequireComponent(typeof(AudioSource))]
    public class GameMusicPlayer : MonoBehaviour, IGameMusicPlayer
    {
        public MusicTrack CurrentTrack { get;  private set; }
        public MusicPlaylist CurrentPlaylist { get; private set; }

        private AudioSource _audioSource;
        
        private List<MusicTrack> _playlistQueue = new();
        private int _currentIndex = -1;

        private bool _isPaused;
        private bool _repeatPlaylist;
        private Coroutine _trackEndCoroutine;
        private IStaticDataService _staticDataService;

        public event Action<MusicTrack> TrackStartPlay;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _staticDataService = ProjectServices.Container.Get<IStaticDataService>();
        }

        public void PlayTrack(string trackFileName, bool repeat)
        {
            MusicTrack track = _staticDataService.GetTrack(trackFileName);
            
            Clear();
            _playlistQueue.Add(track);
            CurrentTrack = track;
            _repeatPlaylist = repeat;
            PlayNext();
        }

        public void PlayPlaylist(string playlistFileName, bool shuffle, bool repeat)
        {
            MusicPlaylist playlist = _staticDataService.GetPlaylist(playlistFileName);
            
            if (playlist?.Tracks == null || playlist.Tracks.Length == 0) 
                return;
            
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
            if (_audioSource.isPlaying && !_isPaused)
            {
                _audioSource.Pause();
                StopTrackEndCheck();
                _isPaused = true;
            }
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

            if (_currentIndex + 1 <= _playlistQueue.Count)
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

                Stop();
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

                _currentIndex = -1;
                PlayNext();
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