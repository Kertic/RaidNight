namespace Code.Entity.Player.StateMachines.PlayerControlStates.PlayerFaeArcherStates
{
    public class CommuneWithFae : ExecuteFaeSkill
    {
        private float m_cooldownReductionTime;

        public CommuneWithFae(PlayerData data, EntityPhysics entityPhysics, PlayerFaeArcherStateMachine controlsStateMachine, float cooldown, float cooldownReductionTime) : base(data, entityPhysics, controlsStateMachine, cooldown)
        {
            SetCooldownReductionTime(cooldownReductionTime);
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            m_controlsStateMachine.ReduceNonUltimateSkillCooldown(m_cooldownReductionTime);
            m_controlsStateMachine.ChangeToIdleState();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            m_controlsStateMachine.AddWispCharge();
        }

        public void SetCooldownReductionTime(float newCDReductionTime)
        {
            m_cooldownReductionTime = newCDReductionTime;
        }
    }
}