using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.Paddle
{
    public class PaddleMovement : MonoBehaviour
    {
        private float _speed;
        private float _maxY;
        private IPaddleControlls _paddleControlls;
        
        private Rigidbody2D _rigidbody;
        
        private float _movementVector;
        private float _paddleHalfHeight;
        private bool _isMovementBlocked;

        public void Construct(float speed, float maxY, IPaddleControlls paddleControlls)
        {
            _speed = speed;
            _maxY = maxY;
            _paddleControlls = paddleControlls;
        }
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _paddleHalfHeight = GetComponent<BoxCollider2D>().bounds.extents.y;
        }
        
        private void Update() => 
            _movementVector = _paddleControlls.MoveVectorY;
        
        private void FixedUpdate()
        {
            var newPosition = _rigidbody.position + Vector2.up * (_movementVector * _speed * Time.fixedDeltaTime);
            newPosition.y = Mathf.Clamp(newPosition.y, -_maxY + _paddleHalfHeight, _maxY - _paddleHalfHeight);
            _rigidbody.MovePosition(newPosition);
        }
    }
}
