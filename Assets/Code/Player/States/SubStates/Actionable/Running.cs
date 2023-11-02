using Code.Player.States.SubStates.UseSkill;
using UnityEngine;

namespace Code.Player.States.SubStates.Actionable
{
    public class Running : SuperStates.Actionable
    {
        private Vector2 lastMovementVector;
        public Running(PlayerData data, PlayerPhysics playerPhysics, PlayerStateMachine stateMachine) : base(data, playerPhysics, stateMachine) { }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            lastMovementVector = Vector2.zero;
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            m_playerPhysics.RemoveContinuousForce(lastMovementVector);
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
            m_stateMachine.ChangeToIdleState();
        }

        public override void OnReceiveButtonInput(InputButton button)
        {
            base.OnReceiveButtonInput(button);
            if (button == InputButton.DASH)
            {
                m_stateMachine.ChangeToDashingState(lastMovementVector.normalized * m_data._MoveSpeed, m_data._DashDuration);
            }
        }
    }
}