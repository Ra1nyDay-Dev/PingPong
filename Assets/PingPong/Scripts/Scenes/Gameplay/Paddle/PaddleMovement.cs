using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.Paddle
{
    public class PaddleMovement : MonoBehaviour
    {
        public float Speed { get; private set; }
        
        private float _levelBounds;
        private IPaddleControlls _paddleControlls;
        
        private Rigidbody2D _rigidbody;
        private float _paddleHalfHeight;
        
        private float _movementVector;

        public void Construct(float speed, float levelBounds, IPaddleControlls paddleControlls)
        {
            Speed = speed;
            _levelBounds = levelBounds;
            _paddleControlls = paddleControlls;
        }
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _paddleHalfHeight = GetComponent<CapsuleCollider2D>().bounds.extents.y;
        }
        
        private void Update() => 
            _movementVector = _paddleControlls.MoveVectorY;
        
        private void FixedUpdate()
        {
            var newPosition = _rigidbody.position + Vector2.up * (_movementVector * Speed * Time.fixedDeltaTime);
            newPosition.y = Mathf.Clamp(newPosition.y, -_levelBounds + _paddleHalfHeight, _levelBounds - _paddleHalfHeight);
            _rigidbody.MovePosition(newPosition);
        }
    }
}
