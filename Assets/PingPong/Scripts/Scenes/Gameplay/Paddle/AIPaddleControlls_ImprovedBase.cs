using System;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.Paddle
{
    public class AIPaddleControlls_ImprovedBase : IPaddleControlls
    {
        public float MoveVectorY => CalculateNextMove();

        private const float PREDICTION_TIME = 1f;
        private const float POSITION_DIFFERENCE_TO_FOLLOW = 0.3f;
        private const float SMOOTH_FACTOR = 4f;
        
        private readonly GameObject _ball;
        private readonly GameObject _paddle;
        private readonly float _levelBounds;
        private readonly float _paddleHalfHeight;
        
        private float _lastDirection;

        public AIPaddleControlls_ImprovedBase(GameObject ball, GameObject paddle, float levelBounds)
        {
            _ball = ball;
            _paddle = paddle;
            _levelBounds = levelBounds;
            _paddleHalfHeight = paddle.GetComponent<BoxCollider2D>().bounds.extents.y;
        }
        
        private float CalculateNextMove()
        {
            var ballVelocity = _ball.GetComponent<Rigidbody2D>().linearVelocity;
            
            if (!IsBallMovingTowardPaddle(ballVelocity))
                return 0;
            
            var predictedPosition = PredictBallPositionWithBounces(ballVelocity);

            var positionDifference = predictedPosition.y - _paddle.transform.position.y;
            if (Mathf.Abs(positionDifference) > POSITION_DIFFERENCE_TO_FOLLOW)
            {
                var targetDirection = Mathf.Sign(positionDifference);
                _lastDirection = Mathf.Lerp(_lastDirection, targetDirection, Time.deltaTime * SMOOTH_FACTOR);
                return _lastDirection;
            }
            
            _lastDirection = Mathf.Lerp(_lastDirection, 0, Time.deltaTime * SMOOTH_FACTOR);
            return _lastDirection;
        }

        private Vector2 PredictBallPositionWithBounces(Vector2 ballVelocity)
        {
            Vector2 currentPosition = _ball.transform.position;
            Vector2 currentVelocity = ballVelocity;
            float timeRemaining = PREDICTION_TIME;
            
            if (currentVelocity.magnitude < 0.01f)
                return currentPosition;
            
            while (timeRemaining > 0)
            {
                float timeToNextBounce = CalculateTimeToNextBounce(currentPosition, currentVelocity);
                
                if (timeToNextBounce > timeRemaining || timeToNextBounce <= 0)
                {
                    currentPosition += currentVelocity * timeRemaining;
                    break;
                }
                
                currentPosition += currentVelocity * timeToNextBounce;
                currentPosition.y = Mathf.Clamp(currentPosition.y,-_levelBounds,_levelBounds);
                currentVelocity = new Vector2(currentVelocity.x, -currentVelocity.y);
                
                timeRemaining -= timeToNextBounce;
            }
            
            return currentPosition;
        }

        private float CalculateTimeToNextBounce(Vector2 currentPosition, Vector2 currentVelocity)
        {
            if (Mathf.Abs(currentVelocity.y) < 0.01f) 
                return float.MaxValue;
            
            float timeToHit = 0f;
            
            if (currentVelocity.y > 0)
            {
                float distanceToTop = _levelBounds - currentPosition.y;
                timeToHit = distanceToTop / currentVelocity.y;
            }
            else if (currentVelocity.y < 0)
            {
                float distanceToBottom = currentPosition.y - (-_levelBounds);
                timeToHit = distanceToBottom / Mathf.Abs(currentVelocity.y);
            }
            
            return Mathf.Max(timeToHit, 0);
        }

        private bool IsBallMovingTowardPaddle(Vector2 ballVelocity)
        {
            Vector2 toPaddle = _paddle.transform.position - _ball.transform.position;
            float angle = Vector2.Angle(ballVelocity, toPaddle);
    
            return angle < 90f;
        }
    }
}