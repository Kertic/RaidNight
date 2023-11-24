using System;
using Code.Camera;
using Code.Entity.Player.Views;
using UnityEngine;

namespace Code.Entity.Player.StateMachines.PlayerControlStates.PlayerFaeArcherStates
{
    public class FireEnchantedArrow : SuperStates.ExecuteSkill
    {
        private float _castTime;
        private float _castStart;
        private PlayerControlsStateMachine.AttackHaltHandle _haltHandle;
        private PlayerCastView _castBar;
        private new PlayerFaeArcherStateMachine m_controlsStateMachine;

        public FireEnchantedArrow(PlayerData data, EntityPhysics entityPhysics, PlayerFaeArcherStateMachine controlsStateMachine, PlayerCastView castView, float cooldown) : base(data, entityPhysics, controlsStateMachine, cooldown)
        {
            m_controlsStateMachine = controlsStateMachine;
            _castBar = castView;
        }

        private void FireArrow()
        {
            Debug.Log("Casting Complete");
            m_controlsStateMachine.FireEnchantedArrowWeapon(PlayerCam.mousePosition);
        }

        private float GetCastingProgress()
        {
            return (Time.time - _castStart) / _castTime;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _castBar.SetProgress(0.0f); //TODO This shouldn't be in the state likely.
            _castBar.ChangeViewState(PlayerCastView.ViewState.CHARGING);
            _castBar.SetText("Enchanted Arrow");
            _haltHandle = m_controlsStateMachine.HaltAutoAttacks();
            _castStart = Time.time;
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            _castBar.SetProgress(0.0f);
            _castBar.ChangeViewState(PlayerCastView.ViewState.HIDDEN);
            _castBar.SetText("");
            m_controlsStateMachine.ReleaseAutoAttackHaltHandle(_haltHandle);
        }

        public override void StateUpdate()
        {
            base.StateUpdate();
            _castBar.SetProgress(GetCastingProgress());
        }

        public override void OnReleaseButtonInput(PlayerControlsStateMachine.InputButton button)
        {
            base.OnReleaseButtonInput(button);

            switch (button)
            {
                case PlayerControlsStateMachine.InputButton.PRIMARY:
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