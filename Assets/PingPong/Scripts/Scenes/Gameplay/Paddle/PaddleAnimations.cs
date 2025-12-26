using System;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.StaticData;
using PingPong.Scripts.Scenes.Gameplay.StaticData;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace PingPong.Scripts.Scenes.Gameplay.Paddle
{
    public class PaddleAnimations : MonoBehaviour
    {
        [SerializeField] private Light2D _paddleLight;
        [SerializeField] private Gradient _paddleHitGradient;
        
        private Animator _animator;
        private float _minBallSpeed;
        private float _maxBallSpeed;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            var settings = ProjectServices.Container
                .Get<IStaticDataService>()
                .GetSettings("Gameplay", SettingsNames.GAMEPLAY_SETTINGS);
            
            _minBallSpeed = settings.BallLaunchSpeed;
            _maxBallSpeed = settings.BallMaxSpeed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                var ballSpeed = other.gameObject.GetComponent<Rigidbody2D>().linearVelocity.magnitude;
                var speedNormalized = Mathf.Clamp01((ballSpeed - _minBallSpeed) / (_maxBallSpeed - _minBallSpeed));
                var paddleHitColor = _paddleHitGradient.Evaluate(speedNormalized);
                _paddleLight.color = paddleHitColor;
                _animator.SetTrigger("BallHit");
            }
        }
    }
}
