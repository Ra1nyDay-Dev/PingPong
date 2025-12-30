using PingPong.Scripts.Scenes.Gameplay.Camera;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PingPong.Scripts.Scenes.Gameplay.Ball
{
    public class BallMovement : MonoBehaviour
    {
        private float _launchSpeed;
        private float _maxSpeed;
        private float _speedIncreasePerHit;
        private float _maxLaunchAngle;
        private float _maxBounceAngle;
        
        private Rigidbody2D _rigidbody;
        private BallAudio _ballAudio;
        
        private float _currentSpeed;

        public void Construct(float launchSpeed, float maxSpeed, float speedIncreasePerHit, float maxLaunchAngle, float maxBounceAngle)
        {
            _launchSpeed = launchSpeed;
            _maxSpeed = maxSpeed;
            _speedIncreasePerHit = speedIncreasePerHit;
            _maxLaunchAngle = maxLaunchAngle;
            _maxBounceAngle = maxBounceAngle;
        }
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _ballAudio = GetComponent<BallAudio>();
        }

        public void LaunchBall()
        {
            _currentSpeed = _launchSpeed;
            _rigidbody.linearVelocity = GetLaunchAngle() * _launchSpeed;
            _ballAudio.PlayBallLaunch();
        }

        public void StopBall() => 
            _rigidbody.linearVelocity = Vector2.zero;

        private Vector3 GetLaunchAngle()
        {
            var angle = Random.Range(-_maxLaunchAngle, _maxLaunchAngle);
            var direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
            
            if (Random.value > 0.5f)
                direction.x *= -1;
            return direction.normalized;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Paddle"))
            {
                HandlePaddleBounce(other);
                _currentSpeed = Mathf.Min(_currentSpeed + _speedIncreasePerHit, _maxSpeed);
            }
            
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * _currentSpeed;
        }

        private void HandlePaddleBounce(Collision2D paddle)
        {
            ContactPoint2D contact = paddle.GetContact(0);
            float paddleHeight = paddle.collider.bounds.size.y;
            
            float relativeHitPosition = (contact.point.y - paddle.transform.position.y) / (paddleHeight / 2);
            relativeHitPosition = Mathf.Clamp(relativeHitPosition, -1f, 1f);
            
            float bounceAngle = relativeHitPosition * _maxBounceAngle;
            
            Vector2 baseDirection;

            if (paddle.transform.position.x < 0)
                baseDirection = Vector2.right;
            else
            {
                baseDirection = Vector2.left;
                bounceAngle = -bounceAngle;
            }
            
            Quaternion rotation = Quaternion.AngleAxis(bounceAngle, Vector3.forward);
            Vector2 newVelocity = rotation * baseDirection;
            
            _rigidbody.linearVelocity = newVelocity.normalized * _currentSpeed;
        }
    }
}
