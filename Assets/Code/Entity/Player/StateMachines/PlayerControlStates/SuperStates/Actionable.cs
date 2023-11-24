using UnityEngine;

namespace Code.Entity.Player.StateMachines.PlayerControlStates.SuperStates
{
    public abstract class Actionable : PlayerControlState
    {
        public override void OnStateEnter() { }

        public override void OnStateExit() { }

        public override void OnReceiveMovementInput(Vector2 direction) { }

        public override void OnHoldMovementInput(Vector2 direction) { }

        public override void OnReceiveButtonInput(PlayerControlsStateMachine.InputButton button)
        {
            switch (button)
            {
                case PlayerControlsStateMachine.InputButton.DASH:
                    m_controlsStateMachine.ChangeToDash();
                    break;
                case PlayerControlsStateMachine.InputButton.PRIMARY:
                    m_controlsStateMachine.ChangeToPrimaryAttack();
                    break;
                case PlayerControlsStateMachine.InputButton.SECONDARY:
                    m_controlsStateMachine.ChangeToSecondaryAttack();
                    break;
                case PlayerControlsStateMachine.InputButton.ULTIMATE:
                    m_controlsStateMachine.ChangeToUltimate();
                    break;
            }
        }

        public override void OnHoldButtonInput(PlayerControlsStateMachine.InputButton button) { }

        public override void OnReleaseMovementInput() { }

        public override void OnReleaseButtonInput(PlayerControlsStateMachine.InputButton button) { }

        public override void OnCollisionEnter2D(Collision2D collision) { }

        public override void OnCollisionExit2D(Collision2D collision) { }

        public override void OnCollisionStay2D(Collision2D collision) { }
        public override void StateUpdate() { }
        public Actionable(PlayerData data, EntityPhysics entityPhysics, PlayerControlsStateMachine controlsStateMachine) : base(data, entityPhysics, controlsStateMachine) { }
    }
}