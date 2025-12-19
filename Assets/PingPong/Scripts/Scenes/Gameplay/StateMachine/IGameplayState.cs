using PingPong.Scripts.Global;
using PingPong.Scripts.Global.StateMachine;

namespace PingPong.Scripts.Scenes.Gameplay.StateMachine
{
    public interface IGameplayState : IState {}
    public interface IPayloadedGameplayState<TPayload> : IPayloadedState<TPayload> {}
}