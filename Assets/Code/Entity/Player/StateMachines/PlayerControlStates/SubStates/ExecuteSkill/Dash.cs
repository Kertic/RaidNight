using UnityEngine;

namespace Code.Entity.Player.StateMachines.PlayerControlStates.SubStates.ExecuteSkill
{
    public class Dash : SuperStates.ExecuteSkill
    {
        private bool _active;
        private PlayerControlsStateMachine.AttackHaltHandle _haltHandle;

        protected float m_dashDuration;
        protected Vector2 m_dashVector;

        public Dash(PlayerData data, EntityPhysics entityPhysics, PlayerControlsStateMachine controlsStateMachine) : base(data, entityPhysics, controlsStateMachine) { }

        public void SetDashVectorAndDuration(Vector2 dashVector, float dashDuration)
        {
            m_dashVector = dashVector;
            m_dashDuration = dashDuration;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _active = true;
            m_entityPhysics.AddBurstForce(new EntityPhysics.BurstForce(m_dashVector, m_dashDuration), () =>
            {
                OnDashMovementEnd();
                Debug.Log("Dash Movement Ended");
            });
            _haltHandle = m_controlsStateMachine.HaltAutoAttacks();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            _active = false;
            m_controlsStateMachine.ReleaseAutoAttackHaltHandle(_haltHandle);
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