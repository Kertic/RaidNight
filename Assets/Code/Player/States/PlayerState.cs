using System;
using UnityEngine;

namespace Code.Player.States
{
    public abstract class PlayerState
    {
        public enum InputButton
        {
            DASH,
            PRIMARY,
            SECONDARY,
            INTERRUPT,
            ULTIMATE,
            NUMOFINPUTBUTTONS
        }
        protected PlayerData m_data;
        protected PlayerPhysics m_playerPhysics;
        protected PlayerStateMachine m_stateMachine;

        public PlayerState(PlayerData data, PlayerPhysics playerPhysics, PlayerStateMachine stateMachine)
        {
            m_data = data;
            m_playerPhysics = playerPhysics;
            m_stateMachine = stateMachine;
        }

        public virtual void OnStateEnter()
        {
            Debug.Log("Entered:" + this.GetType().Name);
        }
        public abstract void OnStateExit();

        public abstract void OnReceiveMovementInput(Vector2 direction);
        public abstract void OnReceiveButtonInput(InputButton button);
        public abstract void OnHoldMovementInput(Vector2 direction);
        public abstract void OnHoldButtonInput(InputButton button);
        public abstract void OnReleaseMovementInput();
        public abstract void OnReleaseButtonInput(InputButton button);

        public abstract void OnCollisionEnter2D(Collision2D collision);
        public abstract void OnCollisionExit2D(Collision2D collision);
        public abstract void OnCollisionStay2D(Collision2D collision);
        public abstract void PhysicsUpdate();
    }
}