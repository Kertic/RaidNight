using System;
using UnityEngine;

namespace Code.Player.States.SuperStates
{
    public abstract class Actionable : PlayerState
    {
        public override void OnStateExit() { }

        public override void OnReceiveMovementInput(Vector2 direction) { }

        public override void OnHoldMovementInput(Vector2 direction) { }

        public override void OnReceiveButtonInput(InputButton button) { }

        public override void OnHoldButtonInput(InputButton button) { }

        public override void OnReleaseMovementInput() { }

        public override void OnReleaseButtonInput(InputButton button) { }

        public override void OnCollisionEnter2D(Collision2D collision) { }

        public override void OnCollisionExit2D(Collision2D collision) { }

        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void PhysicsUpdate() { }
        public Actionable(PlayerData data, PlayerPhysics playerPhysics, PlayerStateMachine stateMachine) : base(data, playerPhysics, stateMachine) { }
    }
}