using System;
using System.Collections.Generic;
using PingPong.Scripts.Scenes.Gameplay.StaticData;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace PingPong.Scripts.Scenes.Gameplay.Ball
{
    [RequireComponent(typeof(TrailRenderer))]
    public class BallTrail : MonoBehaviour
    {
        [SerializeField] Gradient _trailSpeedGradient;
        [SerializeField] float _trailMinStartWidth = 0.1f;
        [SerializeField] float _trailMaxStartWidth = 0.2f;
        
        private TrailRenderer _trailRenderer;
        private Rigidbody2D _rigidbody;
        private float _minBallSpeed;
        private float _maxBallSpeed;
        
        private Gradient _cachedGradient;
        private GradientColorKey[] _colorKeys;
        private GradientAlphaKey[] _alphaKeys;
        private SpriteRenderer _spriteRenderer;
        private Light2D _light;

        private void Awake()
        {
            _trailRenderer = GetComponent<TrailRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
            
            var settings = Resources.Load<GameplayLevelSettings>("Gameplay/StaticData/GameplayLevelSettings");
            _minBallSpeed = settings.BallLaunchSpeed;
            _maxBallSpeed = settings.BallMaxSpeed;
            _light = GetComponentInChildren<Light2D>();
            
            InitializeTrailGradient();
        }


        private void Update()
        {
            UpdateTrailBasedOnSpeed();
        }

        private void InitializeTrailGradient()
        {
            _colorKeys = new GradientColorKey[_trailRenderer.colorGradient.colorKeys.Length];
            _alphaKeys = new GradientAlphaKey[_trailRenderer.colorGradient.alphaKeys.Length];
            _cachedGradient = new Gradient();
            
            Array.Copy(_trailRenderer.colorGradient.colorKeys, _colorKeys, _colorKeys.Length);
            Array.Copy(_trailRenderer.colorGradient.alphaKeys, _alphaKeys, _colorKeys.Length);
            _cachedGradient.SetKeys(_colorKeys, _alphaKeys);
        }

        private void UpdateTrailBasedOnSpeed()
        {
            var speed = _rigidbody.linearVelocity.magnitude;
            var speedNormalized = Mathf.Clamp01((speed - _minBallSpeed) / (_maxBallSpeed - _minBallSpeed));
            var trailColor = _trailSpeedGradient.Evaluate(speedNormalized);
            
            UpdateTrailColor(trailColor);
            UpdateTrailWidth(speedNormalized);
        }

        private void UpdateTrailColor(Color color)
        {
            for (int i = 0; i < _colorKeys.Length; i++) 
                 _colorKeys[i] = new GradientColorKey(color, _colorKeys[i].time);
            
            _cachedGradient.SetKeys(_colorKeys, _alphaKeys);
            _trailRenderer.colorGradient = _cachedGradient;
        }

        private void UpdateTrailWidth(float speedNormalized) => 
            _trailRenderer.startWidth = Mathf.Lerp(_trailMinStartWidth, _trailMaxStartWidth, speedNormalized);
    }
}