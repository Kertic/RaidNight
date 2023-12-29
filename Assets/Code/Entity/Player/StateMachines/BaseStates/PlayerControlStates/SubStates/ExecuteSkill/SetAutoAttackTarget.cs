using UnityEngine;

namespace Code.Entity.Player.StateMachines.BaseStates.PlayerControlStates.SubStates.ExecuteSkill
{
    public class SetAutoAttackTarget : SuperStates.ExecuteSkill
    {
        private PlayerAutoAttackStateMachine _autoAttackStateMachine;
        public SetAutoAttackTarget(PlayerAutoAttackStateMachine autoAttackStateMachine ,PlayerData data, EntityPhysics entityPhysics, PlayerControlsStateMachine controlsStateMachine, float cooldown) : base(data, entityPhysics, controlsStateMachine, cooldown)
        {
            _autoAttackStateMachine = autoAttackStateMachine;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _autoAttackStateMachine.SetTarget();
            m_controlsStateMachine.ChangeToIdleState();
        }
    }
}
