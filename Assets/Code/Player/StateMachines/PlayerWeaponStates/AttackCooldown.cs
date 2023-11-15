using Unity.VisualScripting;
using UnityEngine;

namespace Code.Player.StateMachines.PlayerWeaponStates
{
    public class AttackCooldown : PlayerWeaponState
    {
        private float _cooldownRemaining;
        private float _maxCooldown;
        private bool _isHalted;

        public AttackCooldown(PlayerData data, PlayerAutoAttackStateMachine stateMachine, float maxCooldown) : base(data, stateMachine)
        {
            _cooldownRemaining = maxCooldown;
            _maxCooldown = maxCooldown;
            _isHalted = false;
        }

        private void SetHalted()
        {
            _isHalted = true;
        }

        private void SetUnHalted()
        {
            _isHalted = false;
        }

        private void InstantlyFinishCooldown()
        {
            m_autoAttackStateMachine.BeginAttackCharging();
        }
        
        public override void OnStateEnter()
        {
            _isHalted = false;
            _cooldownRemaining = _maxCooldown;
            m_autoAttackStateMachine.SetViewProgress(1.0f);
            m_autoAttackStateMachine._PlayerControlsStateMachine.m_haltAutoAttack += SetHalted;
            m_autoAttackStateMachine._PlayerControlsStateMachine.m_resumeAutoAttack += SetUnHalted;
            m_autoAttackStateMachine._PlayerControlsStateMachine.m_resetAutoAttack += InstantlyFinishCooldown;
        }

        public override void OnStateExit()
        {
            m_autoAttackStateMachine._PlayerControlsStateMachine.m_haltAutoAttack -= SetHalted;
            m_autoAttackStateMachine._PlayerControlsStateMachine.m_resumeAutoAttack -= SetUnHalted;
            m_autoAttackStateMachine._PlayerControlsStateMachine.m_resetAutoAttack -= InstantlyFinishCooldown;
        }

        public override void StateUpdate()
        {
            _cooldownRemaining -= Time.fixedDeltaTime;
            m_autoAttackStateMachine.SetViewProgress(_cooldownRemaining / _maxCooldown);

            if (_cooldownRemaining > 0)
            {
                return;
            }

            if (_isHalted)
            {
                m_autoAttackStateMachine.HaltAttacks();
                return;
            }

            m_autoAttackStateMachine.BeginAttackCharging();
        }

        public void SetCooldownLength(float newMaxCooldown)
        {
            _maxCooldown = newMaxCooldown;
        }
    }
}