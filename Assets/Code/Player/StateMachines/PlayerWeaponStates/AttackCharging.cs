using Code.Player.States;
using UnityEngine;

namespace Code.Player.StateMachines.PlayerWeaponStates
{
    public class AttackCharging : PlayerWeaponState
    {
        
        private float _chargedTime;

        public AttackCharging(PlayerData data, PlayerWeaponStateMachine stateMachine) : base(data, stateMachine)
        {
            _chargedTime = 0;
        }

        private void HaltCharging()
        {
            m_weaponStateMachine.HaltAttacks();
        }
        
        public override void OnStateEnter()
        {
            m_weaponStateMachine._PlayerControlsStateMachine.m_haltAutoAttack += HaltCharging;
            _chargedTime = 0;
            m_weaponStateMachine.SetViewProgress(0.0f);
        }

        public override void OnStateExit()
        {
            m_weaponStateMachine._PlayerControlsStateMachine.m_haltAutoAttack -= HaltCharging;
            _chargedTime = 0;
        }

        public override void StateUpdate()
        {
            _chargedTime += Time.fixedDeltaTime;
            m_weaponStateMachine.SetViewProgress(_chargedTime / (1.0f / m_data._AttackSpeed));
            if (_chargedTime >= 1.0f / m_data._AttackSpeed)
            {
                // Fire Shot, Transition to Attack cooldown
                m_weaponStateMachine.FireAutoAttack();
            }
        }
        
    }
}