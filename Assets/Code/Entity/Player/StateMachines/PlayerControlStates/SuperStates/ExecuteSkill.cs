using System;
using UnityEngine;

namespace Code.Entity.Player.StateMachines.PlayerControlStates.SuperStates
{
    public abstract class ExecuteSkill : PlayerControlState
    {
        protected float m_maxCooldown;
        protected float m_timeWhenAvailable;

        public override void OnStateEnter() { }

        public override void OnStateExit()
        {
            m_timeWhenAvailable = m_maxCooldown + Time.time;
        }

        public override void OnReceiveMovementInput(Vector2 direction) { }
        public override void OnReceiveButtonInput(PlayerControlsStateMachine.InputButton button) { }
        public override void OnHoldMovementInput(Vector2 direction) { }
        public override void OnHoldButtonInput(PlayerControlsStateMachine.InputButton button) { }
        public override void OnReleaseMovementInput() { }
        public override void OnReleaseButtonInput(PlayerControlsStateMachine.InputButton button) { }
        public override void OnCollisionEnter2D(Collision2D collision) { }
        public override void OnCollisionExit2D(Collision2D collision) { }
        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void StateUpdate() { }

        public bool IsSkillReady()
        {
            return Time.time >= m_timeWhenAvailable;
        }

        public float GetTimeRemainingUntilReady()
        {
            return Math.Max(m_timeWhenAvailable - Time.time, 0);
        }

        public float GetPercentCooldownCompleted()
        {
            return GetTimeRemainingUntilReady() / m_maxCooldown;
        }

        public ExecuteSkill(PlayerData data, EntityPhysics entityPhysics, PlayerControlsStateMachine controlsStateMachine, float cooldown) : base(data, entityPhysics, controlsStateMachine)
        {
            m_maxCooldown = cooldown;
            m_timeWhenAvailable = 0;
        }
    }
}