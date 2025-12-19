using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.Ball
{
    public class BallMovement : MonoBehaviour
    {
        private float _launchSpeed = 5f;
        private float _maxLaunchAngle = 45f;
        
        private Rigidbody2D _rigidbody;

        public void Construct(float launchSpeed, float maxLaunchAngle)
        {
            _launchSpeed = launchSpeed;
            _maxLaunchAngle = maxLaunchAngle;
        }
        
        private void Awake() => 
            _rigidbody = GetComponent<Rigidbody2D>();

        public void LaunchBall() => 
            _rigidbody.linearVelocity = SetLaunchAngle() * _launchSpeed;
        
        public void StopBall() => 
            _rigidbody.linearVelocity = Vector2.zero;

        private Vector3 SetLaunchAngle()
        {
            var angle = Random.Range(-_maxLaunchAngle, _maxLaunchAngle);
            var direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
            
            if (Random.value > 0.5f)
                direction.x *= -1;
            return direction.normalized;
        }
    }
}
