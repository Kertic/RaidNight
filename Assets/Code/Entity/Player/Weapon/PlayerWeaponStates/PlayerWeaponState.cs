using Code.Entity.Player.StateMachines;
using Code.Entity.Player.StateMachines.BaseStates;

namespace Code.Entity.Player.Weapon.PlayerWeaponStates
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