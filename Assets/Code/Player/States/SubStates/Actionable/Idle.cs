using UnityEngine;

namespace Code.Player.States.SubStates.Actionable
{
    public class Idle : SuperStates.Actionable
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            m_playerPhysics.velocity = Vector2.zero;
        }

        public override void OnReceiveMovementInput(Vector2 direction)
        {
            base.OnReceiveMovementInput(direction);
            m_stateMachine.ChangeState(m_stateMachine._Running);
        }

        public override void OnReceiveButtonInput(InputButton button)
        {
            if (button == InputButton.DASH)
            {
//                m_stateMachine.ChangeState(m_stateMachine.);
            }
        }

        public Idle(PlayerData data, PlayerPhysics playerPhysics, PlayerStateMachine stateMachine) : base(data, playerPhysics, stateMachine) { }
    }
}