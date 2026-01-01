using PingPong.Scripts.Global.Services;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.Services.GameplayFactory
{
    public interface IGameplayFactory : ISceneService
    {
        GameObject Ball { get; }
        GameObject Player1Paddle { get; }
        GameObject Player2Paddle { get; }
        
        void CreateLevelObjects();
        void CreateBall();
        void CreatePaddles();
        void ResetLevelObjectsPositions();
        void ResetBallPosition();
        void ResetPaddlesPosition();
    }
}