using System.Collections;
using PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter;
using TMPro;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.UI
{
    public class GameplayUI : MonoBehaviour, IGameplayUI
    {
        [SerializeField] private TextMeshProUGUI _scoreLeft;
        [SerializeField] private TextMeshProUGUI _scoreRight;
        [SerializeField] private TextMeshProUGUI _prepareRoundCounter;
        [SerializeField] private ScoreCounter _scoreCounter;

        private const float COUNTDOWN_TICKS = 3;

        private float _countdownTicksLeft;

        private void Awake()
        {
            _scoreLeft.text = "2";
        }

        public void UpdateUI()
        {
            _scoreLeft.text = $"{_scoreCounter.ScorePlayer1}";
            _scoreRight.text = $"{_scoreCounter.ScorePlayer2}";
        }

        public void Reset()
        {
            _scoreLeft.text = "0";
            _scoreRight.text = "0";
        }

        public void StartRoundCountdown(float time) => 
            StartCoroutine(RoundCountdown(time));

        private IEnumerator RoundCountdown(float time)
        {
            _prepareRoundCounter.gameObject.SetActive(true);
            _countdownTicksLeft = COUNTDOWN_TICKS;
            var timePerTick = time / COUNTDOWN_TICKS;
            _prepareRoundCounter.text = $"{_countdownTicksLeft}";

            while (_countdownTicksLeft > 0)
            {
                yield return new WaitForSeconds(timePerTick);
                _countdownTicksLeft -= 1;
                _prepareRoundCounter.text = $"{_countdownTicksLeft}";
            }
            
            _prepareRoundCounter.gameObject.SetActive(false);
        }
    }
}