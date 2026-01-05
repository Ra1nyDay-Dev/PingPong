using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.Paddle
{
    public class AIPaddleControlls : IPaddleControlls
     {
         public float MoveVectorY => CalculateNextMove();

         private const float MIN_ATTACK_EDGE_OFFSET = 0.1f;
         
         private readonly GameObject _ball;
         private readonly GameObject _paddle;
         private readonly float _levelBounds;
         
         private readonly float _reactionDelay;
         private readonly float _predictionError; 
         private readonly bool _calculateWithBounces;
         
         private readonly Rigidbody2D _ballRigidbody;
         private readonly float _paddleHalfHeight;
         private readonly PaddleMovement _paddleMovement;
         
         private float _paddleAttackPoint = 0f;
         private bool _newBounce = true;
         private float _reactionDelayRemaining;
         private float _currentPredictionError;

         public AIPaddleControlls(GameObject ball, GameObject paddle, float levelBounds, float reactionDelay
             , float predictionError, bool calculateWithBounces)
         {
             _ball = ball;
             _paddle = paddle;
             _levelBounds = levelBounds;
             
             _reactionDelay = reactionDelay;
             _predictionError = predictionError;
             _calculateWithBounces = calculateWithBounces;

             _ballRigidbody = ball.GetComponent<Rigidbody2D>();
             _paddleHalfHeight = paddle.GetComponent<CapsuleCollider2D>().bounds.extents.y;
             _paddleMovement = paddle.GetComponent<PaddleMovement>();
         }

        private float CalculateNextMove()
        {
            if (!IsBallMovingTowardPaddle(_ballRigidbody.linearVelocity))
            {
                _newBounce = true;
                _reactionDelayRemaining = _reactionDelay;
                return 0;
            }
            
            if (_reactionDelayRemaining > 0)
            {
                _reactionDelayRemaining -= Time.deltaTime;
                return 0;
            }
                

            float timeToPaddle = CalculateTimeToPaddle();
            if (float.IsPositiveInfinity(timeToPaddle)) 
                return 0;

            if (_newBounce)
            {
                _paddleAttackPoint = Random.Range(MIN_ATTACK_EDGE_OFFSET, 1f - MIN_ATTACK_EDGE_OFFSET);
                _currentPredictionError = Random.Range(-_predictionError, _predictionError);
                _newBounce = false;
            }
            
            var predictedBallPositionY = PredictBallPositionY(timeToPaddle) + _currentPredictionError;
            
            float strikeY = predictedBallPositionY + (_paddleAttackPoint - 0.5f) * _paddleHalfHeight * 2f;
            strikeY = Mathf.Clamp(strikeY, -_levelBounds + _paddleHalfHeight, _levelBounds - _paddleHalfHeight);
            
            float distanceToPredictedY = strikeY - _paddle.transform.position.y;
            float maxPaddleDistancePerFrame = _paddleMovement.Speed * Time.fixedDeltaTime;

            return Mathf.Clamp(distanceToPredictedY / maxPaddleDistancePerFrame, -1f, 1f);
        }

        private bool IsBallMovingTowardPaddle(Vector2 ballVelocity)
        {
            Vector2 toPaddle = _paddle.transform.position - _ball.transform.position;
            float angle = Vector2.Angle(ballVelocity, toPaddle);
    
            return angle < 90f;
        }

        private float CalculateTimeToPaddle()
        {
            if (Mathf.Approximately(_ballRigidbody.linearVelocityX, 0f)) 
                return float.PositiveInfinity;

            float time = (_paddle.transform.position.x - _ball.transform.position.x) / _ballRigidbody.linearVelocityX;
            
            return time < 0f ? float.PositiveInfinity : time;
        }

        private float PredictBallPositionY(float timeToPaddle)
        {
            float predictedBallPositionY;

            if (_calculateWithBounces)
                predictedBallPositionY = PredictBallYWithBounces(_ball.transform.position.y, _ballRigidbody.linearVelocityY, timeToPaddle);
            else
                predictedBallPositionY = _ball.transform.position.y + _ballRigidbody.linearVelocityY * timeToPaddle;

            predictedBallPositionY = Mathf.Clamp(predictedBallPositionY,-_levelBounds + _paddleHalfHeight,_levelBounds - _paddleHalfHeight);
            
            return predictedBallPositionY;
        }

        private float PredictBallYWithBounces(float ballPositionY, float ballVelocityY, float timeToPaddle)
        {
            if (Mathf.Approximately(ballVelocityY, 0f))
                return ballPositionY;
                
            float wallTop = _levelBounds;
            float wallBot = -_levelBounds;
            int maxCycles = 15; // Защита от бесконечного цикла

            for (int i = 0; timeToPaddle > 0f && i < maxCycles; ++i)
            {
                float distanceToWall = ballVelocityY > 0 
                    ? (wallTop - ballPositionY) 
                    : (ballPositionY - wallBot);
                
                float timeToWall = Mathf.Abs(distanceToWall / ballVelocityY);

                if (timeToWall >= timeToPaddle)
                {
                    ballPositionY += ballVelocityY * timeToPaddle;
                    break;
                }

                ballPositionY += ballVelocityY * timeToWall; // дошли до стенки
                ballVelocityY = -ballVelocityY; // отражаем
                timeToPaddle  -= timeToWall;
            }
            
            return ballPositionY;
        }
     }
}