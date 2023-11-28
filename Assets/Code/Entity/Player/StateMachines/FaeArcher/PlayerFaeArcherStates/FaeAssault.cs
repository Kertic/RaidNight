using Code.Camera;
using UnityEngine;

namespace Code.Entity.Player.StateMachines.FaeArcher.PlayerFaeArcherStates
{
    public class FaeAssault : ExecuteFaeSkill
    {
        private float _maxDashDistance;
        private float _dashDuration;
        private float _testStartTime;
        private PlayerControlsStateMachine.AttackHaltHandle _haltHandle;

        public FaeAssault(PlayerData data, EntityPhysics entityPhysics, PlayerFaeArcherStateMachine controlsStateMachine, float maxDashDistance, float dashDuration, float cooldown) : base(data, entityPhysics, controlsStateMachine, cooldown)
        {
            _maxDashDistance = maxDashDistance;
            _dashDuration = dashDuration;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _testStartTime = Time.time;
            Debug.Log("Start: " + _testStartTime);

            _haltHandle = m_controlsStateMachine.HaltAutoAttacks();
            EntityPhysics.BurstForce force = new(
                m_controlsStateMachine._MovementDirection == Vector2.zero ? (PlayerCam.mousePosition - (Vector2)m_entityPhysics.transform.position) : m_controlsStateMachine._MovementDirection,
                _dashDuration); // This is bugged where the total burst is: seconds =  max distance * duration instead of seconds = duration
            force.m_movementVector = force.m_movementVector.normalized * _maxDashDistance;
            m_entityPhysics.AddBurstForce(force, () => { m_controlsStateMachine.ChangeToIdleState(); });
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            m_controlsStateMachine.ReleaseAutoAttackHaltHandle(_haltHandle);
            m_controlsStateMachine.AddWispCharge();
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