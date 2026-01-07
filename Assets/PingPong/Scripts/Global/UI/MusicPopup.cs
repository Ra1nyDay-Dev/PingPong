using System;
using System.Collections;
using PingPong.Scripts.Global.Services.GameMusicPlayer;
using TMPro;
using UnityEngine;

namespace PingPong.Scripts.Global.UI
{
    public class MusicPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        
        private readonly int _showTrigger = Animator.StringToHash("Show");
        private readonly int _hideTrigger = Animator.StringToHash("Hide");

        private Animator _animator;

        private void Awake() => 
            _animator = GetComponent<Animator>();

        public void ShowPopUp(MusicTrack track)
        {
            _titleText.text = track.Artist;
            _titleText.text += !string.IsNullOrEmpty(track.TrackName) ? $" - {track.TrackName}" : "";
            _animator.SetTrigger(_showTrigger);
            StartCoroutine(Hide(3f));
        }

        private IEnumerator Hide(float delay = 0f, float destroyTime = 3f)
        {
            yield return new WaitForSeconds(delay);
            _animator.SetTrigger(_hideTrigger);
            
            yield return new WaitForSeconds(destroyTime);
            Destroy(gameObject);
        }
    }
}
