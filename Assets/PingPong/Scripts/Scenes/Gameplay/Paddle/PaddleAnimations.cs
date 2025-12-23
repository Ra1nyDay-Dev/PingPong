using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace PingPong.Scripts.Scenes.Gameplay.Paddle
{
    public class PaddleAnimations : MonoBehaviour
    {
        [SerializeField] private Light2D _paddleLight;
        [SerializeField] private Color[] _paddleLightColors;
        
        private Animator _animator;

        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                if (_paddleLightColors.Length > 0)
                {
                    Color color = _paddleLightColors[Random.Range(0, _paddleLightColors.Length)];
                    _paddleLight.color = new Color(color.r, color.g, color.b, 1f);
                    // _paddleLight.color = Color.white;
                    _animator.SetTrigger("BallHit");
                }
            }
        }
    }
}
