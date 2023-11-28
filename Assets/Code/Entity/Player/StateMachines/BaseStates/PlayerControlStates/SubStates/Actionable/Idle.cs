using UnityEngine;

namespace Code.Entity.Player.StateMachines.BaseStates.PlayerControlStates.SubStates.Actionable
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
            m_controlsStateMachine.ChangeToRunningState();
        }

        public Idle(PlayerData data, EntityPhysics entityPhysics, PlayerControlsStateMachine controlsStateMachine) : base(data, entityPhysics, controlsStateMachine) { }
    }
}