namespace Code.Player.StateMachines.PlayerWeaponStates
{
    public abstract class PlayerWeaponState : State
    {
        protected PlayerData m_data;
        protected PlayerAutoAttackStateMachine m_autoAttackStateMachine;

        public PlayerWeaponState(PlayerData data, PlayerAutoAttackStateMachine stateMachine)
        {
            m_data = data;
            m_autoAttackStateMachine = stateMachine;
        }
    }
}