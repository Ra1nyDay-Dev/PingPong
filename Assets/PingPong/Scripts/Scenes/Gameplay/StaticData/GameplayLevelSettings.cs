using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "GameplayLevelSettings", menuName = "Game/Level Settings")]
    public class GameplayLevelSettings : ScriptableObject
    {
        public Vector3 Player1PaddleStartPosition = new Vector3(-8, 0, 0);
        public Vector3 Player2PaddleStartPosition = new Vector3(8, 0, 0);
        public float PaddleSpeed = 15f;
        public Vector3 BallStartPosition = Vector3.zero;
        public float BallLaunchSpeed = 10f;
        public float BallMaxLaunchAngle = 45f;
        public float LevelBoundsY = 4.7f;
        public float FirstRoundCountdownTime = 3f;
        public float RoundCountdownTime = 1.5f;
        public int ScoreToWin = 5;
    }
}
