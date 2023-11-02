using UnityEngine;

namespace Code.Player.States.SubStates.Actionable
{
    public class Running : SuperStates.Actionable
    {
        public Running(PlayerData data, PlayerPhysics playerPhysics, PlayerStateMachine stateMachine) : base(data, playerPhysics, stateMachine) { }
        private Vector2 lastMovementApplied;

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            lastMovementApplied = Vector2.zero;
        }

        public override void OnReceiveMovementInput(Vector2 direction)
        {
            OnHoldMovementInput(direction);
        }

        public override void OnHoldMovementInput(Vector2 direction)
        {
            base.OnHoldMovementInput(direction);
            Vector2 newMovementApplication = direction.normalized * m_data._MoveSpeed;
            if (lastMovementApplied == newMovementApplication)
            {
                return;
            }

            m_playerPhysics.RemoveContinuousForce(lastMovementApplied);
            lastMovementApplied = newMovementApplication;
            m_playerPhysics.AddContinuousForce(lastMovementApplied);
        }

        public override void OnReceiveButtonInput(InputButton button)
        {
            base.OnReceiveButtonInput(button);
            if (button == InputButton.DASH && false /* Removing this logic branch for now */) m_stateMachine.ChangeToDashingState();
        }

        public override void OnReleaseMovementInput()
        {
            base.OnReleaseMovementInput();
            m_playerPhysics.RemoveContinuousForce(lastMovementApplied);
            m_stateMachine.ChangeToIdleState();
        }
    }
}