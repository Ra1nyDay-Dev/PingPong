using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.Services.RoundTimer
{
    public class RoundTimer : MonoBehaviour, IRoundTimer
    {
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private float _updateTextInterval = 0.01f;
        
        public float RoundTime { get; private set; } = 0f;
        public bool IsRunning { get; private set; } = false;
        
        private Coroutine _timerCoroutine;
        
        public void StartTimer() => 
            _timerCoroutine = StartCoroutine(TimerRoutine());

        public void StopTimer()
        {
            if (_timerCoroutine != null) 
                StopCoroutine(_timerCoroutine);

            UpdateText();
            IsRunning = false;
        }

        private IEnumerator TimerRoutine()
        {
            IsRunning = true;
            
            while (IsRunning)
            {
                yield return new WaitForSeconds(_updateTextInterval);
                RoundTime += _updateTextInterval;
                UpdateText();
            }
        }

        private void UpdateText()
        {
            _timerText.text = FormatTime(RoundTime);
        }

        private string FormatTime(float timeInSeconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeInSeconds);
        
            if (timeSpan.TotalHours < 1)
            {
                return string.Format("Время раунда: {0:00}:{1:00}:{2:00}", 
                    timeSpan.Minutes, 
                    timeSpan.Seconds, 
                    Mathf.FloorToInt(timeSpan.Milliseconds / 10f));
            }
            else
            {
                return string.Format("Время раунда: {0:00}:{1:00}:{2:00}:{3:00}", 
                    timeSpan.Hours, 
                    timeSpan.Minutes, 
                    timeSpan.Seconds, 
                    Mathf.FloorToInt(timeSpan.Milliseconds / 10f));
            }
        }

        public void Reset()
        {
            StopTimer();
            RoundTime = 0f;
            UpdateText();
        }
    }
}
