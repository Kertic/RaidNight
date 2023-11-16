using Code.Camera;
using Code.Player.StateMachines.PlayerControlStates.SuperStates;
using UnityEngine;

namespace Code.Player.StateMachines.PlayerControlStates.PlayerFaeArcherStates
{
    public class Flit : ExecuteSkill
    {
        private float _maxDashDistance;
        private float _dashDuration;

        public Flit(PlayerData data, EntityPhysics entityPhysics, PlayerControlsStateMachine controlsStateMachine, float maxDashDistance, float dashDuration) : base(data, entityPhysics, controlsStateMachine)
        {
            _maxDashDistance = maxDashDistance;
            _dashDuration = dashDuration;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            m_controlsStateMachine.HaltAutoAttacks();
            EntityPhysics.BurstForce force = new(
                m_controlsStateMachine.MovementDirection == Vector2.zero ? (PlayerCam.mousePosition - (Vector2)m_entityPhysics.transform.position) : m_controlsStateMachine.MovementDirection,
                _dashDuration);
            force.m_movementVector = force.m_movementVector.normalized * _maxDashDistance;
            m_entityPhysics.AddBurstForce(force, () => { m_controlsStateMachine.ChangeToIdleState(); });
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            m_controlsStateMachine.ResumeAutoAttacks();
        }

        public void SetDashDistance(float newDashDistance)
        {
            _maxDashDistance = newDashDistance;
        }

        public void SetDashDuration(float newDashDuration)
        {
            _dashDuration = newDashDuration;
        }
    }
}