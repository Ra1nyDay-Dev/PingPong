using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.Input;
using PingPong.Scripts.Scenes.Gameplay.Data;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.Paddle
{
    public class PaddleMovement : MonoBehaviour
    {
        private float _speed;
        private PaddleInputType _inputType;
        private float _maxY;
        private IInputService _inputService;
        
        private Rigidbody2D _rigidbody;
        private BoxCollider2D _collider;
        
        private float _inputVertical;
        private float _paddleHalfHeight;
        private bool _isMovementBlocked;

        public void Construct(float speed, PaddleInputType inputType, float maxY, IInputService inputService)
        {
            _speed = speed;
            _inputType = inputType;
            _maxY = maxY;
            _inputService = inputService;
        }
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();
            _paddleHalfHeight = _collider.bounds.extents.y;
        }

        private void Update() => 
            _inputVertical = _inputService.VerticalAxis;
        
        private void FixedUpdate()
        {
            Vector2 newPosition = _rigidbody.position + Vector2.up * (_inputVertical * _speed * Time.fixedDeltaTime);
            newPosition.y = Mathf.Clamp(newPosition.y, -_maxY + _paddleHalfHeight, _maxY - _paddleHalfHeight);
            _rigidbody.MovePosition(newPosition);
        }
    }
}
