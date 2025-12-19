using PingPong.Scripts.Global.Data;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter
{
    public class ScoreCounter : MonoBehaviour, IScoreCounter
    {
        public int ScorePlayer1 { get; private set; } = 0;
        public int ScorePlayer2 { get; private set; } = 0;

        public void UpdateScore(PlayerId playerLosed)
        {
            if (playerLosed == PlayerId.Player1)
                ScorePlayer2++;
            else
                ScorePlayer1++;
        }
        
        public void Reset()
        {
            ScorePlayer1 = 0;
            ScorePlayer2 = 0;
        }
    }
}