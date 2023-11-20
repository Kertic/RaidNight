using UnityEngine;

namespace Code.Entity.Player.StateMachines.PlayerControlStates
{
    public abstract class PlayerControlState : State
    {
        protected PlayerData m_data;
        protected EntityPhysics m_entityPhysics;
        protected PlayerControlsStateMachine m_controlsStateMachine;

        public PlayerControlState(PlayerData data, EntityPhysics entityPhysics, PlayerControlsStateMachine controlsStateMachine)
        {
            m_data = data;
            m_entityPhysics = entityPhysics;
            m_controlsStateMachine = controlsStateMachine;
        }

        public override void OnStateEnter() { }

        public abstract void OnReceiveMovementInput(Vector2 direction);
        public abstract void OnReceiveButtonInput(PlayerControlsStateMachine.InputButton button);
        public abstract void OnHoldMovementInput(Vector2 direction);
        public abstract void OnHoldButtonInput(PlayerControlsStateMachine.InputButton button);
        public abstract void OnReleaseMovementInput();
        public abstract void OnReleaseButtonInput(PlayerControlsStateMachine.InputButton button);

        public abstract void OnCollisionEnter2D(Collision2D collision);
        public abstract void OnCollisionExit2D(Collision2D collision);
        public abstract void OnCollisionStay2D(Collision2D collision);
    }
}