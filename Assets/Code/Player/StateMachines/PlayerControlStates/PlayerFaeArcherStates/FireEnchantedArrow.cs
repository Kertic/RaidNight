using Code.Camera;
using Code.Player.StateMachines.PlayerWeaponStates;
using UnityEngine;

namespace Code.Player.StateMachines.PlayerControlStates.PlayerFaeArcherStates
{
    public class FireEnchantedArrow : SuperStates.ExecuteSkill
    {
        private Vector2 _targetPos;
        private float _castTime;
        private float _castStart;
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
            _castBar.SetProgress(0.0f);
            _castBar.ChangeViewState(PlayerCastView.ViewState.CHARGING);
            _castBar.SetText("Enchanted Arrow");
            m_controlsStateMachine.HaltAutoAttacks();
            _targetPos = PlayerCam.mousePosition;
            _castStart = Time.time;
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            _castBar.SetProgress(0.0f);
            _castBar.ChangeViewState(PlayerCastView.ViewState.HIDDEN);
            _castBar.SetText("");
            m_controlsStateMachine.ResumeAutoAttacks();
            _targetPos = Vector2.zero;
        }

        public override void StateUpdate()
        {
            base.StateUpdate();
            _castBar.SetProgress((Time.time - _castStart) / _castTime);
            if (Time.time - _castStart >= _castTime)
            {
                Debug.Log("Casting Complete");
                m_controlsStateMachine.FireEnchantedArrowWeapon(_targetPos);
                m_controlsStateMachine.ChangeToIdleState();
            }
        }

        public void SetCastTime(float castTime)
        {
            _castTime = castTime;
        }
    }
}