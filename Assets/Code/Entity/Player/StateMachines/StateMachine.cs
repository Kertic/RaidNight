using Code.Entity.Player.StateMachines.BaseStates;
using Code.Entity.Player.StateMachines.BaseStates.PlayerControlStates;
using UnityEngine;

namespace Code.Entity.Player.StateMachines
{
    public class StateMachine <TState>: MonoBehaviour where TState : State
    {
        protected TState m_currentState;
        
        protected virtual void Start()
        {
            m_currentState?.OnStateEnter();
        }
        protected virtual void FixedUpdate()
        {
            m_currentState?.StateUpdate();
        }

        protected virtual void ChangeState(TState newControlState)
        {
            m_currentState?.OnStateExit();
            m_currentState = newControlState;
            m_currentState?.OnStateEnter();
        }
    }
}