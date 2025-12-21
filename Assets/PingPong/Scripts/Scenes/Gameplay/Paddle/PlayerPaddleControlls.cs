using System;
using PingPong.Scripts.Global.Services.Input;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.Paddle
{
    public class PlayerPaddleControlls : IPaddleControlls
    {
        public float MoveVectorY => _inputService.VerticalAxis;

        private readonly IInputService _inputService;

        public PlayerPaddleControlls(IInputService inputService)
        {
            _inputService = inputService;
        }
    }
}