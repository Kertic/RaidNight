using Code.Camera;
using Code.Entity.Player.StateMachines.PlayerControlStates.SuperStates;
using UnityEditor;
using UnityEngine;

namespace Code.Entity.Player.StateMachines.PlayerControlStates.PlayerFaeArcherStates
{
    public class FaeAssault : ExecuteSkill
    {
        private float _maxDashDistance;
        private float _dashDuration;
        private float _testStartTime;
        private bool _isEnhanced; // TODO: This should likely just be it's own state
        private PlayerControlsStateMachine.AttackHaltHandle _haltHandle;

        public FaeAssault(PlayerData data, EntityPhysics entityPhysics, PlayerControlsStateMachine controlsStateMachine, float maxDashDistance, float dashDuration, float cooldown) : base(data, entityPhysics, controlsStateMachine, cooldown)
        {
            _maxDashDistance = maxDashDistance;
            _dashDuration = dashDuration;
            _isEnhanced = false;
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
            Debug.Log("End duration: " + (Time.time - _testStartTime));
            if (_isEnhanced)
            {
                m_timeWhenAvailable = m_maxCooldown * 0.5f + Time.time;
                _isEnhanced = false;
            }

            m_controlsStateMachine.ReleaseAutoAttackHaltHandle(_haltHandle);
        }

        public void SetDashDistance(float newDashDistance)
        {
            _maxDashDistance = newDashDistance;
        }

        public void SetDashDuration(float newDashDuration)
        {
            _dashDuration = newDashDuration;
        }

        public void SetIsEnhancedFromMischiefStack(bool isEnhanced)
        {
            _isEnhanced = isEnhanced;
        }
    }
}