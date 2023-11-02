using UnityEngine;

namespace Code.Player.States.SubStates.Actionable
{
    public class Idle : SuperStates.Actionable
    {

        public override void OnReceiveMovementInput(Vector2 direction)
        {
            base.OnReceiveMovementInput(direction);
            OnHoldMovementInput(direction);
        }

        public override void OnHoldMovementInput(Vector2 direction)
        {
            base.OnHoldMovementInput(direction);
            m_stateMachine.ChangeToRunningState();
        }

        public Idle(PlayerData data, PlayerPhysics playerPhysics, PlayerStateMachine stateMachine) : base(data, playerPhysics, stateMachine) { }
    }
}