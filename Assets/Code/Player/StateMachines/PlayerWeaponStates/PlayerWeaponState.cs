using Code.Player.States;

namespace Code.Player.StateMachines.PlayerWeaponStates
{
    public abstract class PlayerWeaponState : IState
    {
        protected PlayerData m_data;
        protected PlayerWeaponStateMachine m_weaponStateMachine;

        public PlayerWeaponState(PlayerData data, PlayerWeaponStateMachine stateMachine)
        {
            m_data = data;
            m_weaponStateMachine = stateMachine;
        }
    }
}