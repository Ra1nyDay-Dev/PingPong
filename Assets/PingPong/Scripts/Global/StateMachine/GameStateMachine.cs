using System;
using System.Collections.Generic;

namespace PingPong.Scripts.Global.StateMachine
{
    public abstract class GameStateMachine : IStateMachine
    {
        protected Dictionary<Type, IExitableState> _states = new();
        protected IExitableState _activeState;

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;
            
            return state;
        }

        public TState GetState<TState>() where TState : class, IExitableState => 
            _states[typeof(TState)] as TState;
    }
}