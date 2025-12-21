using System;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.Paddle
{
    public class AIPaddleControlls : IPaddleControlls
    {
        public float MoveVectorY => CalculateNextMove();

        private const float PREDICTION_TIME = 0.1f;
        private const float POSITION_DIFFERENCE_TO_FOLLOW = 0.3f;
        private const float SMOOTH_FACTOR = 4f;
        
        private readonly GameObject _ball;
        private readonly Transform _paddle;
        private readonly float _levelBounds;
        
        private float _lastDirection;

        public AIPaddleControlls(GameObject ball, Transform paddle, float levelBounds)
        {
            _ball = ball;
            _paddle = paddle;
            _levelBounds = levelBounds;
        }
        
        private float CalculateNextMove()
        {
            var ballVelocity = _ball.GetComponent<Rigidbody2D>().linearVelocity;
            
            if (!IsBallMovingTowardPaddle(ballVelocity))
                return 0;
            
            var predictedPosition = _ball.transform.position + ((Vector3)ballVelocity * PREDICTION_TIME);

            var positionDifference = predictedPosition.y - _paddle.position.y;
            if (Mathf.Abs(positionDifference) > POSITION_DIFFERENCE_TO_FOLLOW)
            {
                var targetDirection = Mathf.Sign(positionDifference);
                _lastDirection = Mathf.Lerp(_lastDirection, targetDirection, Time.deltaTime * SMOOTH_FACTOR);
                return _lastDirection;
            }
            
            _lastDirection = Mathf.Lerp(_lastDirection, 0, Time.deltaTime * SMOOTH_FACTOR);
            return _lastDirection;
        }

        private bool IsBallMovingTowardPaddle(Vector2 ballVelocity)
        {
            Vector2 toPaddle = _paddle.position - _ball.transform.position;
            float angle = Vector2.Angle(ballVelocity, toPaddle);
    
            return angle < 90f;
        }
    }
}