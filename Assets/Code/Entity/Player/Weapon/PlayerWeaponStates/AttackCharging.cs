using Code.Entity.Player.StateMachines;
using UnityEngine;

namespace Code.Entity.Player.Weapon.PlayerWeaponStates
{
    public class AttackCharging : PlayerWeaponState
    {
        
        private float _chargedTime;

        public AttackCharging(PlayerData data, PlayerAutoAttackStateMachine stateMachine) : base(data, stateMachine)
        {
            _chargedTime = 0;
        }

        private void HaltCharging()
        {
            m_autoAttackStateMachine.HaltAttacks();
        }

        private void ResetAutoAttack()
        {
            m_autoAttackStateMachine.BeginAttackCharging();
        }
        
        public override void OnStateEnter()
        {
            m_autoAttackStateMachine._PlayerControlsStateMachine.m_haltAutoAttack += HaltCharging;
            m_autoAttackStateMachine._PlayerControlsStateMachine.m_resetAutoAttack += ResetAutoAttack;
            _chargedTime = 0;
            m_autoAttackStateMachine.SetViewProgress(0.0f);
        }

        public override void OnStateExit()
        {
            m_autoAttackStateMachine._PlayerControlsStateMachine.m_haltAutoAttack -= HaltCharging;
            m_autoAttackStateMachine._PlayerControlsStateMachine.m_resetAutoAttack -= ResetAutoAttack;
            _chargedTime = 0;
        }

        public override void StateUpdate()
        {
            _chargedTime += Time.fixedDeltaTime;
            m_autoAttackStateMachine.SetViewProgress(_chargedTime / (1.0f / m_data._AttackSpeed));
            if (_chargedTime >= 1.0f / m_data._AttackSpeed)
            {
                // Fire Shot, Transition to Attack cooldown
                m_autoAttackStateMachine.FireAutoAttack();
            }
        }
        
    }
}