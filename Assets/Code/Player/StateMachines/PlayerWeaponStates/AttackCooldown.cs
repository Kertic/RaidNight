using Code.Player.States;
using UnityEngine;

namespace Code.Player.StateMachines.PlayerWeaponStates
{
    public class AttackCooldown : PlayerWeaponState
    {
        private float _cooldownRemaining;
        private float _maxCooldown;
        private bool _isHalted;

        public AttackCooldown(PlayerData data, PlayerWeaponStateMachine stateMachine, float maxCooldown) : base(data, stateMachine)
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

        public override void OnStateEnter()
        {
            _cooldownRemaining = _maxCooldown;
            m_weaponStateMachine.SetViewProgress(1.0f);
            m_weaponStateMachine._PlayerControlsStateMachine.m_haltAutoAttack += SetHalted;
            m_weaponStateMachine._PlayerControlsStateMachine.m_resumeAutoAttack += SetUnHalted;
        }

        public override void OnStateExit()
        {
            m_weaponStateMachine._PlayerControlsStateMachine.m_haltAutoAttack -= SetHalted;
            m_weaponStateMachine._PlayerControlsStateMachine.m_resumeAutoAttack -= SetUnHalted;
        }

        public override void StateUpdate()
        {
            _cooldownRemaining -= Time.fixedDeltaTime;
            m_weaponStateMachine.SetViewProgress(_cooldownRemaining / _maxCooldown);

            if (_cooldownRemaining > 0)
            {
                return;
            }

            if (_isHalted)
            {
                m_weaponStateMachine.HaltAttacks();
                return;
            }

            m_weaponStateMachine.BeginAttackCharging();
        }
    }
}