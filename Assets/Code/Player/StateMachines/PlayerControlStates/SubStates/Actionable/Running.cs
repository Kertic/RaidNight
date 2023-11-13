using UnityEngine;

namespace Code.Player.StateMachines.PlayerControlStates.SubStates.Actionable
{
    public class Running : StateMachines.PlayerControlStates.SuperStates.Actionable
    {
        private Vector2 lastMovementVector;
        public Running(PlayerData data, PlayerPhysics playerPhysics, PlayerControlsStateMachine controlsStateMachine) : base(data, playerPhysics, controlsStateMachine) { }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            lastMovementVector = Vector2.zero;
            m_controlsStateMachine.HaltAutoAttacks();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            m_playerPhysics.RemoveContinuousForce(lastMovementVector);
            m_controlsStateMachine.ResumeAutoAttacks();
        }

        public override void OnReceiveMovementInput(Vector2 direction)
        {
            OnHoldMovementInput(direction);
        }

        public override void OnHoldMovementInput(Vector2 direction)
        {
            base.OnHoldMovementInput(direction);
            Vector2 newMovementApplication = direction.normalized * m_data._MoveSpeed;
            if (lastMovementVector == newMovementApplication)
            {
                return;
            }

            m_playerPhysics.RemoveContinuousForce(lastMovementVector);
            lastMovementVector = newMovementApplication;
            m_playerPhysics.AddContinuousForce(lastMovementVector);
        }

        public override void OnReleaseMovementInput()
        {
            base.OnReleaseMovementInput();
            m_playerPhysics.RemoveContinuousForce(lastMovementVector);
            m_controlsStateMachine.ChangeToIdleState();
        }

        public override void OnReceiveButtonInput(PlayerControlsStateMachine.InputButton button)
        {
            base.OnReceiveButtonInput(button);
            if (button == PlayerControlsStateMachine.InputButton.DASH)
            {
                m_controlsStateMachine.ChangeToDashingState(lastMovementVector.normalized * m_data._MoveSpeed, m_data._DashDuration);
            }
        }
    }
}