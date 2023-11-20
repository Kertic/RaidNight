namespace Code.Entity.Player.StateMachines.PlayerWeaponStates
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