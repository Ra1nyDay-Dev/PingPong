using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace PingPong.Scripts.Scenes.Gameplay.Services.LightController
{
    public class LightController : MonoBehaviour, ILightController
    {
        [SerializeField] private GameObject[] _leftGateLights;
        [SerializeField] private GameObject[] _rightGateLights;
        [SerializeField] private Color _goalColor = new Color(255,0,30,1);
        [SerializeField] private float _goalIntensity = 5f;
        [SerializeField] private float _goalOuterRadius = 12f;

        private Color _startLightColor;
        private float _startIntensity;
        private float _startOuterRadius;

        private void Awake()
        {
            _startLightColor = _leftGateLights.First().GetComponent<Light2D>().color;
            _startIntensity = _leftGateLights.First().GetComponent<Light2D>().intensity;
            _startOuterRadius = _leftGateLights.First().GetComponent<Light2D>().pointLightOuterRadius;
        }

        public void HighlightLeftGates()
        {
            foreach (var light in _leftGateLights)
            {
                light.GetComponent<Light2D>().color = _goalColor;
                light.GetComponent<Light2D>().intensity = _goalIntensity;
                light.GetComponent<Light2D>().pointLightOuterRadius = _goalOuterRadius;
            }
        }
        public void HighlightRightGates()
        {
            foreach (var light in _rightGateLights)
            {
                light.GetComponent<Light2D>().color = _goalColor;
                light.GetComponent<Light2D>().intensity = _goalIntensity;
                light.GetComponent<Light2D>().pointLightOuterRadius = _goalOuterRadius;
            }
        }

        public void ResetLights()
        {
            foreach (var light in _leftGateLights)
            {
                light.GetComponent<Light2D>().color = _startLightColor;
                light.GetComponent<Light2D>().intensity = _startIntensity;
                light.GetComponent<Light2D>().pointLightOuterRadius = _startOuterRadius;
            }

            foreach (var light in _rightGateLights)
            {
                light.GetComponent<Light2D>().color = _startLightColor;
                light.GetComponent<Light2D>().intensity = _startIntensity;
                light.GetComponent<Light2D>().pointLightOuterRadius = _startOuterRadius;
            }
        }
    
    }
}
