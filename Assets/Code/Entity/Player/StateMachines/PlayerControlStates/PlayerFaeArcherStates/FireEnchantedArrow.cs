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

        public FireEnchantedArrow(PlayerData data, EntityPhysics entityPhysics, PlayerFaeArcherStateMachine controlsStateMachine, PlayerCastView castView) : base(data, entityPhysics, controlsStateMachine)
        {
            m_controlsStateMachine = controlsStateMachine;
            _castBar = castView;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _castBar.SetProgress(0.0f);//TODO This shouldn't be in the state likely.
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
            _castBar.SetProgress((Time.time - _castStart) / _castTime);
            if (Time.time - _castStart >= _castTime)
            {
                Debug.Log("Casting Complete");
                m_controlsStateMachine.FireEnchantedArrowWeapon(PlayerCam.mousePosition);
                m_controlsStateMachine.ChangeToIdleState();
            }
        }

        public void SetCastTime(float castTime)
        {
            _castTime = castTime;
        }
    }
}