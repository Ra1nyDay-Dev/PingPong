using System;
using System.Collections;
using System.Collections.Generic;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.UI;
using PingPong.Scripts.Scenes.Gameplay.StateMachine;
using PingPong.Scripts.Scenes.Gameplay.StateMachine.States;
using PingPong.Scripts.Scenes.Gameplay.UI.Dialogs;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.UI
{
    public class GameplayUI : SceneUI, IGameplayUI
    {
        [SerializeField] private TextMeshProUGUI _prepareRoundCounter;
 
        private const float COUNTDOWN_TICKS = 3;

        private GameplayUIAudio _gameplayUIAudio;
        private float _countdownTicksLeft;

        private void Awake()
        {
            _sceneDialogsPrefabs = new Dictionary<Type, string>()
            {
                {typeof(PlayAgainDialog), "Gameplay/UI/Dialogs/PlayAgainDialog"},
                {typeof(PauseDialog), "Gameplay/UI/Dialogs/PauseDialog"},
            };
            
            _gameplayUIAudio = GetComponent<GameplayUIAudio>();
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
                PlayTickAudio();
                
                yield return new WaitForSeconds(timePerTick);
                _countdownTicksLeft -= 1;
                _prepareRoundCounter.text = $"{_countdownTicksLeft}";
            }
            
            _prepareRoundCounter.gameObject.SetActive(false);
        }

        private void PlayTickAudio()
        {
            if (_countdownTicksLeft > 1)
                _gameplayUIAudio.PlayCountdownTick();
            else 
                _gameplayUIAudio.PlayCountdownLastTick();
        }

        public void OnPauseButtonClicked() => 
            ShowDialog<PauseDialog>();
    }
}