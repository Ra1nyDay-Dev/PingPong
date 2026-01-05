using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "AIDifficultyConfig", menuName = "Game/AI difficulty config")]
    public class AIDifficultyConfig : ScriptableObject
    {
        [Range(0, 2)]
        public float SpeedMultiplier = 1f;
        
        [Range(0, 1)]
        public float ReactionDelay = 0f;
        
        [Range(0, 2)]
        public float PredictionError = 0f;
        
        public bool CalculateWithBounces = false;
    }
}