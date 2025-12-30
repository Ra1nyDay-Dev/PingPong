using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Game/Level Settings")]
    public class LevelSettings : ScriptableObject
    {
        public Vector3 Player1PaddleStartPosition = new Vector3(-8, 0, -1);
        public Vector3 Player1PaddleRotation = new Vector3(0, 0, -90);
        public Vector3 Player2PaddleStartPosition = new Vector3(8, 0, -1);
        public Vector3 Player2PaddleRotation = new Vector3(0, 0, 90);
        public Vector3 BallStartPosition = new Vector3(0,0,-1);
        public float PaddleSpeed = 15f;
        public float BallLaunchSpeed = 7f;
        public float BallMaxSpeed = 20f;
        public float BallSpeedIncreasePerHit = 2f;
        public float BallMaxLaunchAngle = 45f;
        public float BallMaxBounceAngle = 45f;
        public float LevelBoundsY = 4.7f;
        public float FirstRoundCountdownTime = 3f;
        public float RoundCountdownTime = 1.5f;
        public int ScoreToWin = 5;
    }
}
