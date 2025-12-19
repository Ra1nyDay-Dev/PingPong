namespace PingPong.Scripts.Global.StateMachine
{
    public interface IStateMachine
    {
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;
        TState GetState<TState>() where TState : class, IExitableState;
    }
}