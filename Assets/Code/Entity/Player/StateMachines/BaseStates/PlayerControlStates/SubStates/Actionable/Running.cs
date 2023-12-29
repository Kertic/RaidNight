using UnityEngine;

namespace Code.Entity.Player.StateMachines.BaseStates.PlayerControlStates.SubStates.Actionable
{
    public class Running : SuperStates.Actionable
    {
        protected Vector2 m_lastMovementVector;
        private PlayerControlsStateMachine.AttackHaltHandle _handle;
        public Running(PlayerData data, EntityPhysics entityPhysics, PlayerControlsStateMachine controlsStateMachine) : base(data, entityPhysics, controlsStateMachine) { }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _handle = m_controlsStateMachine.HaltAutoAttacks();
            m_lastMovementVector = Vector2.zero;
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            m_controlsStateMachine.ReleaseAutoAttackHaltHandle(_handle);
            m_entityPhysics.RemoveContinuousForce(m_lastMovementVector);
        }

        public override void OnReceiveMovementInput(Vector2 direction)
        {
            OnHoldMovementInput(direction);
        }

        public override void OnHoldMovementInput(Vector2 direction)
        {
            base.OnHoldMovementInput(direction);
            Vector2 newMovementApplication = direction.normalized * m_data._MoveSpeed;
            if (m_lastMovementVector == newMovementApplication)
            {
                return;
            }

            m_entityPhysics.RemoveContinuousForce(m_lastMovementVector);
            m_lastMovementVector = newMovementApplication;
            m_entityPhysics.AddContinuousForce(m_lastMovementVector);
        }

        public override void OnReleaseMovementInput()
        {
            base.OnReleaseMovementInput();
            m_entityPhysics.RemoveContinuousForce(m_lastMovementVector);
            m_controlsStateMachine.ChangeToIdleState();
        }
    }
}