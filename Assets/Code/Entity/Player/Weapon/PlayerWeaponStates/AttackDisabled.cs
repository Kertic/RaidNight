using Code.Entity.Player.StateMachines.PlayerControlStates;

namespace Code.Entity.Player.Weapon.PlayerWeaponStates
{
    public class AttackDisabled : PlayerWeaponState
    {
        public AttackDisabled(PlayerData data, PlayerAutoAttackStateMachine stateMachine) : base(data, stateMachine) { }

        private void ResumeAttacking(bool shouldResume)
        {
            if (shouldResume)
                m_autoAttackStateMachine.BeginAttackCharging();
        }

        public override void OnStateEnter()
        {
            m_autoAttackStateMachine._PlayerControlsStateMachine.m_enableAutoAttack += ResumeAttacking;
        }

        public override void OnStateExit()
        {
            m_autoAttackStateMachine._PlayerControlsStateMachine.m_enableAutoAttack -= ResumeAttacking;
        }

        public override void StateUpdate() { }
    }
}