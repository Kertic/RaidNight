using Code.Player.StateMachines.PlayerControlStates.SuperStates;
using UnityEngine;

namespace Code.Player.StateMachines.PlayerControlStates.SubStates.UseSkill
{
    public class Dash : PerformingAction
    {
        protected float m_dashDuration;
        protected Vector2 m_dashVector;
        private bool _active;

        public Dash(PlayerData data, PlayerPhysics playerPhysics, PlayerControlsStateMachine controlsStateMachine) : base(data, playerPhysics, controlsStateMachine) { }

        public void SetDashVectorAndDuration(Vector2 dashVector, float dashDuration)
        {
            m_dashVector = dashVector;
            m_dashDuration = dashDuration;
        }
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _active = true;
            m_playerPhysics.AddBurstForce(new PlayerPhysics.BurstForce(m_dashVector, m_dashDuration), () =>
            {
                OnDashMovementEnd();
                Debug.Log("Dash Movement Ended");
            });
            m_controlsStateMachine.HaltAutoAttacks();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            _active = false;
            m_controlsStateMachine.ResumeAutoAttacks();
        }

        private void OnDashMovementEnd()
        {
            if (_active)
            {
                Debug.Log("Changing to idle due to dash ending");
                m_controlsStateMachine.ChangeToIdleState();
            }
        }
    }
}