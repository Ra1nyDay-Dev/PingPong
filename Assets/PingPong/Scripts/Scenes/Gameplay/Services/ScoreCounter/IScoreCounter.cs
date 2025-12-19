using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;

namespace PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter
{
    public interface IScoreCounter : ISceneService
    {
        int ScorePlayer1 { get; }
        int ScorePlayer2 { get; }

        void UpdateScore(PlayerId playerLosed);

        void Reset();
    }
}