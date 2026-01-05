using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.Paddle
{
    public class AIPaddleControlls2 : IPaddleControlls
     {
         public float MoveVectorY => CalculateNextMove();

         private const float MIN_EDGE_ATTACK_OFFSET = 0.1f;
         private const float SMOOTH_FACTOR = 0.25f; // Плавность движения ракетки к предсказанной позиции мяча (защита от дергания)
         
         private readonly GameObject _ball;
         private readonly GameObject _paddle;
         private readonly float _levelBounds;
         
         private readonly Rigidbody2D _ballRigidbody;
         private readonly float _paddleHalfHeight;
         private readonly PaddleMovement _paddleMovement;
         
         private float _smoothedTargetY = 0f;
         private float _attackPoint;
         private bool _newBounce = true;

         public AIPaddleControlls2(GameObject ball, GameObject paddle, float levelBounds)
         {
             _ball = ball;
             _paddle = paddle;
             _levelBounds = levelBounds;
             
             _ballRigidbody = ball.GetComponent<Rigidbody2D>();
             _paddleHalfHeight = paddle.GetComponent<CapsuleCollider2D>().bounds.extents.y;
             _paddleMovement = paddle.GetComponent<PaddleMovement>();
         }

        private float CalculateNextMove()
        {
            if (!IsBallMovingTowardPaddle(_ballRigidbody.linearVelocity))
            {
                _newBounce = true;
                return 0;
            }

            float timeToPaddle = CalculateTimeToPaddle();
            
            if (float.IsPositiveInfinity(timeToPaddle)) 
                return 0;
            
            float predictedBallPositionY = PredictBallYWithBounces(_ball.transform.position.y, _ballRigidbody.linearVelocityY, timeToPaddle);
            
            predictedBallPositionY = Mathf.Clamp(predictedBallPositionY,-_levelBounds + _paddleHalfHeight,_levelBounds - _paddleHalfHeight);

            if (_newBounce)
            {
                ChooseAttackPoint(predictedBallPositionY);
                _newBounce = false;
            }
            
            float strikeY = predictedBallPositionY + (_attackPoint - 0.5f) * _paddleHalfHeight * 2f;
            strikeY = Mathf.Clamp(strikeY, -_levelBounds + _paddleHalfHeight, _levelBounds - _paddleHalfHeight);
            
            float maxPaddleDistancePerFrame = _paddleMovement.Speed * Time.fixedDeltaTime;
            _smoothedTargetY = Mathf.MoveTowards(_smoothedTargetY,strikeY,maxPaddleDistancePerFrame * SMOOTH_FACTOR);
            float distanceToPredictedY = _smoothedTargetY - _paddle.transform.position.y;

            return Mathf.Clamp(distanceToPredictedY / maxPaddleDistancePerFrame, -1f, 1f);
        }

        private void ChooseAttackPoint(float predictedBallPositionY)
        {
            float wallTop = _levelBounds - _paddleHalfHeight;
            float wallBot = -_levelBounds + _paddleHalfHeight;
            
            if (predictedBallPositionY > wallTop - _paddleHalfHeight)
                _attackPoint = 1f - MIN_EDGE_ATTACK_OFFSET;
            else if (predictedBallPositionY < wallBot + _paddleHalfHeight)
                _attackPoint = MIN_EDGE_ATTACK_OFFSET;
            else
                _attackPoint = Random.Range(MIN_EDGE_ATTACK_OFFSET, 1f - MIN_EDGE_ATTACK_OFFSET);
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

        private float PredictBallYWithBounces(float ballPositionY, float ballVelocityY, float timeToPaddle)
        {
            if (Mathf.Approximately(ballVelocityY, 0f))
                return ballPositionY;
                
            float wallTop = _levelBounds;
            float wallBot = -_levelBounds;
            int maxPredictedBounces = 5;

            for (int i = 0; i < maxPredictedBounces && timeToPaddle > 0f; ++i)
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