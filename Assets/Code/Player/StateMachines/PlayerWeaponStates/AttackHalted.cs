using Code.Player.States;
using UnityEngine.Networking;

namespace Code.Player.StateMachines.PlayerWeaponStates
{
    public class AttackHalted : PlayerWeaponState
    {
        public AttackHalted(PlayerData data, PlayerWeaponStateMachine stateMachine) : base(data, stateMachine)
        {
            
        }

        private void ResumeCharging()
        {
            m_weaponStateMachine.BeginAttackCharging();
        }

        public override void OnStateEnter()
        {
            m_weaponStateMachine._PlayerControlsStateMachine.m_resumeAutoAttack += ResumeCharging;
        }

        public override void OnStateExit()
        {
            m_weaponStateMachine._PlayerControlsStateMachine.m_resumeAutoAttack -= ResumeCharging;
        }

        public override void StateUpdate() { }
    }
}