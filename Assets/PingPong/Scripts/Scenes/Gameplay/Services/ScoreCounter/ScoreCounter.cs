using PingPong.Scripts.Global.Data;
using TMPro;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter
{
    public class ScoreCounter : MonoBehaviour, IScoreCounter
    {
        [SerializeField] private TextMeshProUGUI _scoreLeft;
        [SerializeField] private TextMeshProUGUI _scoreRight;
        
        public int ScorePlayer1 { get; private set; } = 0;
        public int ScorePlayer2 { get; private set; } = 0;

        public void UpdateScore(PlayerId playerLosed)
        {
            if (playerLosed == PlayerId.Player1)
                ScorePlayer2++;
            else
                ScorePlayer1++;

            UpdateText();
        }
        
        public void Reset()
        {
            ScorePlayer1 = 0;
            ScorePlayer2 = 0;
            UpdateText();
        }

        public void UpdateText()
        {
            _scoreLeft.text = $"{ScorePlayer1}";
            _scoreRight.text = $"{ScorePlayer2}";
        }
    }
}