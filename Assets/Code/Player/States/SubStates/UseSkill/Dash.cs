using System;
using UnityEngine;

namespace Code.Player.States.SubStates.UseSkill
{
    [Serializable]
    public class Dash : SuperStates.UseSkill
    {
        [SerializeField]
        protected float dashDuration;

        [SerializeField]
        protected Vector2 dashOffset;

        private float _dashStartTime;
        public Dash(PlayerData data, PlayerPhysics playerPhysics, PlayerStateMachine stateMachine) : base(data, playerPhysics, stateMachine) { }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _dashStartTime = Time.time;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
        }
    }
}