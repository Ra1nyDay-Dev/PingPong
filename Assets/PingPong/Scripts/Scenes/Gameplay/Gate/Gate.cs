using PingPong.Scripts.Global.Data;
using PingPong.Scripts.Global.Services;
using PingPong.Scripts.Scenes.Gameplay.StateMachine;
using PingPong.Scripts.Scenes.Gameplay.StateMachine.States;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.Gate
{
    public class Gate : MonoBehaviour
    {
        [SerializeField] private PlayerId _playerGateId;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ball")) 
                SceneServices.Container.Get<IGameplayStateMachine>().Enter<RoundEndState, PlayerId>(_playerGateId);
        }
    }
}
