using Code.Entity.Player.StateMachines.PlayerControlStates;

namespace Code.Entity.Player.Weapon.PlayerWeaponStates
{
    public class AttackHalted : PlayerWeaponState
    {
        public AttackHalted(PlayerData data, PlayerAutoAttackStateMachine stateMachine) : base(data, stateMachine) { }

        private void ResumeCharging()
        {
            m_autoAttackStateMachine.BeginAttackCharging();
        }

        public override void OnStateEnter()
        {
            m_autoAttackStateMachine._PlayerControlsStateMachine.m_resumeAutoAttack += ResumeCharging;
        }

        public override void OnStateExit()
        {
            m_autoAttackStateMachine._PlayerControlsStateMachine.m_resumeAutoAttack -= ResumeCharging;
        }

        public override void StateUpdate() { }
    }
}