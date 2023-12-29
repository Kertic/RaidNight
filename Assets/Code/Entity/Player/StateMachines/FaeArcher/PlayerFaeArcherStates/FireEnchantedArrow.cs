using Code.Camera;
using Code.Entity.Player.Views;
using UnityEngine;

namespace Code.Entity.Player.StateMachines.FaeArcher.PlayerFaeArcherStates
{
    public class FireEnchantedArrow : ExecuteFaeSkill
    {
        private float _castTime;
        private float _castStart;
        private PlayerCastView _castBar;

        public FireEnchantedArrow(PlayerData data, EntityPhysics entityPhysics, PlayerFaeArcherStateMachine controlsStateMachine, PlayerCastView castView, float cooldown) : base(data, entityPhysics, controlsStateMachine, cooldown)
        {
            _castBar = castView;
        }

        private void FireArrow()
        {
            m_controlsStateMachine.FireEnchantedArrowWeapon(PlayerCam.mousePosition);
        }

        private float GetCastingProgress()
        {
            return Mathf.Min(1.0f, (Time.time - _castStart) / _castTime);
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _castBar.SetProgress(0.0f); //TODO This shouldn't be in the state likely.
            _castBar.ChangeViewState(PlayerCastView.ViewState.CHARGING);
            _castBar.SetText("Enchanted Arrow");
            _castStart = Time.time;
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            _castBar.SetProgress(0.0f);
            _castBar.ChangeViewState(PlayerCastView.ViewState.HIDDEN);
            _castBar.SetText("");
            m_controlsStateMachine.AddWispCharge();
            m_controlsStateMachine.SetChargeShotProgress(0.0f);
        }

        public override void StateUpdate()
        {
            base.StateUpdate();
            _castBar.SetProgress(GetCastingProgress());
            m_controlsStateMachine.SetChargeShotProgress(GetCastingProgress());
        }

        public override void OnReleaseButtonInput(PlayerControlsStateMachine.InputButton button)
        {
            base.OnReleaseButtonInput(button);

            switch (button)
            {
                case PlayerControlsStateMachine.InputButton.SECONDARY:
                    FireArrow();
                    m_controlsStateMachine.ChangeToIdleState();
                    break;
            }
        }

        public override void OnReceiveButtonInput(PlayerControlsStateMachine.InputButton button)
        {
            base.OnReceiveButtonInput(button);

            switch (button)
            {
                case PlayerControlsStateMachine.InputButton.DASH:
                    FireArrow();
                    m_controlsStateMachine.ChangeToDash();
                    break;
            }
        }

        public void SetCastTime(float castTime)
        {
            _castTime = castTime;
        }
    }
}