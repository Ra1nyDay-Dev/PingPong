using System.Collections;
using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Global.Services.CoroutineRunner;
using PingPong.Scripts.Scenes.Gameplay.Ball;
using PingPong.Scripts.Scenes.Gameplay.Camera;
using PingPong.Scripts.Scenes.Gameplay.Paddle;
using PingPong.Scripts.Scenes.Gameplay.Services.LightController;
using PingPong.Scripts.Scenes.Gameplay.Services.RoundTimer;
using PingPong.Scripts.Scenes.Gameplay.Services.ScoreCounter;
using PingPong.Scripts.Scenes.Gameplay.StaticData;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.StateMachine.States
{
    public class RoundEndState : IPayloadedGameplayState<PlayerId>
    {
        private const string LEVEL_SETTINGS = "Gameplay/StaticData/GameplayLevelSettings";
        
        private GameplayLevelSettings _settings;
        private IScoreCounter _score;
        private GameObject _ball;
        private IGameplayStateMachine _stateMachine;
        private IRoundTimer _roundTimer;
        private ILightController _lightController;
        private ICoroutineRunner _coroutineRunner;
        private GameObject[] _paddles;
        private UnityEngine.Camera _camera;

        public void Enter(PlayerId playerLosed)
        {
            GetStateDependencies();
            _score.UpdateScore(playerLosed);
            _roundTimer.StopTimer();
            _ball.GetComponent<BallMovement>().StopBall();
            DisableMovement();
            _camera.GetComponent<CameraShake>().Shake(0.5f, magnitude: 0.2f);
            _coroutineRunner.StartCoroutine(HighlightGates(playerLosed, 1.5f));
        }

        public void Exit()
        {
            
        }

        private void GetStateDependencies()
        {
            _settings = Resources.Load<GameplayLevelSettings>(LEVEL_SETTINGS);
            _score = SceneServices.Container.Get<IScoreCounter>();
            _ball = GameObject.FindWithTag("Ball");
            _stateMachine = SceneServices.Container.Get<IGameplayStateMachine>();
            _roundTimer = SceneServices.Container.Get<IRoundTimer>();
            _lightController = SceneServices.Container.Get<ILightController>();
            _coroutineRunner = ProjectServices.Container.Get<ICoroutineRunner>();
            _paddles = GameObject.FindGameObjectsWithTag("Paddle");
            _camera = UnityEngine.Camera.main;
        }
        
        private void DisableMovement()
        {
            foreach (var paddle in _paddles)
                paddle.GetComponent<PaddleMovement>().enabled = false;
        }
        
        private IEnumerator HighlightGates(PlayerId playerLosed, float showTime)
        {
            if (playerLosed == PlayerId.Player1)
                _lightController.HighlightLeftGates();
            else
                _lightController.HighlightRightGates();
            
            yield return new WaitForSeconds(showTime);
            
            CheckGameOver();
        }


        private void CheckGameOver()
        {
            bool isGameOver = _score.ScorePlayer1 == _settings.ScoreToWin || _score.ScorePlayer2 == _settings.ScoreToWin;

            if (isGameOver)
                _stateMachine.Enter<GameOverState>();
            else
                _stateMachine.Enter<RoundStartState>();
        }
    }
}