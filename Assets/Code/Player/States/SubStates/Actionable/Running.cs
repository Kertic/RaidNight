using UnityEngine;

namespace Code.Player.States.SubStates.Actionable
{
    public class Running : SuperStates.Actionable
    {
        public Running(PlayerData data, PlayerPhysics playerPhysics, PlayerStateMachine stateMachine) : base(data, playerPhysics, stateMachine) { }

        public override void OnReceiveMovementInput(Vector2 direction)
        {
            OnHoldMovementInput(direction);
        }

        public override void OnHoldMovementInput(Vector2 direction)
        {
            m_playerPhysics.velocity = (direction * Vector2.right /* Keeps X value, removes Y value */).normalized * m_data._MoveSpeed;
        }

        public override void OnReceiveButtonInput(InputButton button)
        {
            base.OnReceiveButtonInput(button);
      //      if (button == InputButton.DASH) m_stateMachine.ChangeState(m_stateMachine._Jumping);
        }

        public override void OnReleaseMovementInput()
        {
            base.OnReleaseMovementInput();
            m_stateMachine.ChangeState(m_stateMachine._Idle);
        }
    }
}