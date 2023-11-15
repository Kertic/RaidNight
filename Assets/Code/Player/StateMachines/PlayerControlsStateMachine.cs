using System;
using System.Collections.Generic;
using Code.Camera;
using Code.Player.StateMachines.PlayerControlStates;
using Code.Player.StateMachines.PlayerControlStates.SubStates.Actionable;
using Code.Player.StateMachines.PlayerControlStates.SuperStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Player.StateMachines
{
    [RequireComponent(typeof(PlayerData), typeof(EntityPhysics))]
    public class PlayerControlsStateMachine : MonoBehaviour
    {
        public enum InputButton
        {
            DASH,
            PRIMARY,
            SECONDARY,
            ULTIMATE,
            NUMOFINPUTBUTTONS
        }

        private static Controls _controls;
        private static Dictionary<InputAction, InputButton> _inputCallbacks;

        [SerializeField]
        private PlayerCam cam;

        private PlayerControlState _currentControlState;
        public PlayerData PlayerData { get; private set; }
        public EntityPhysics EntityPhysics { get; private set; }
        protected Idle _Idle { get; set; }
        protected Running _Running { get; set; }
        protected ExecuteSkill _PrimaryAttack { get; set; }
        protected ExecuteSkill _SecondaryAttack { get; set; }
        protected ExecuteSkill _Dash { get; set; }
        protected ExecuteSkill _Ultimate { get; set; }

        public Action m_haltAutoAttack;
        public Action m_resetAutoAttack;
        public Action m_resumeAutoAttack;
        static public Vector2 mousePos; 

        protected virtual void Awake()
        {
            PlayerData = GetComponent<PlayerData>();
            EntityPhysics = GetComponent<EntityPhysics>();
            _currentControlState = _Idle = new Idle(PlayerData, EntityPhysics, this);
            _Running = new Running(PlayerData, EntityPhysics, this);
            _controls = new Controls();
            _inputCallbacks = new Dictionary<InputAction, InputButton>
            {
                { _controls.Gameplay.PrimaryFire, InputButton.PRIMARY },
                { _controls.Gameplay.SecondaryFire, InputButton.PRIMARY },
                { _controls.Gameplay.Dash, InputButton.DASH },
                { _controls.Gameplay.Ultimate, InputButton.ULTIMATE },
            };
            _controls.Gameplay.Movement.performed += OnMovementInput;
            _controls.Gameplay.Movement.canceled += OnMovementInputEnd;
            _controls.Gameplay.Zoom.performed += cam.ZoomDistance;
            _controls.Gameplay.MousePos.performed += context =>
            {
                Vector2 mousePos2d = context.ReadValue<Vector2>();
                mousePos = PlayerCam.Instance.Camera.ScreenToWorldPoint(mousePos2d);
            };

            foreach (KeyValuePair<InputAction, InputButton> action in _inputCallbacks)
            {
                action.Key.performed += (InputAction.CallbackContext context) => { _currentControlState.OnReceiveButtonInput(action.Value); };
                action.Key.canceled += (InputAction.CallbackContext context) => { _currentControlState.OnReleaseButtonInput(action.Value); };
            }
        }

        private void OnEnable()
        {
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        private void Start()
        {
            _currentControlState.OnStateEnter();
        }


        private void OnMovementInput(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            _currentControlState.OnReceiveMovementInput(direction);
        }

        private void OnMovementInputEnd(InputAction.CallbackContext context)
        {
            _currentControlState.OnReleaseMovementInput();
        }

        private void Update()
        {
            Vector2 movementInputVector = _controls.Gameplay.Movement.ReadValue<Vector2>();
            if (_controls.Gameplay.Movement.IsPressed())
                _currentControlState.OnHoldMovementInput(movementInputVector);
            foreach (KeyValuePair<InputAction, InputButton> action in _inputCallbacks)
            {
                if (action.Key.IsPressed())
                    _currentControlState.OnHoldButtonInput(action.Value);
            }
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            _currentControlState.OnCollisionEnter2D(other);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            _currentControlState.OnCollisionExit2D(other);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            _currentControlState.OnCollisionStay2D(other);
        }

        private void FixedUpdate()
        {
            _currentControlState.StateUpdate();
        }

        private void ChangeState(PlayerControlState newControlState)
        {
            _currentControlState.OnStateExit();
            _currentControlState = newControlState;
            _currentControlState.OnStateEnter();
        }

        public void ChangeToRunningState()
        {
            ChangeState(_Running);
        }

        public void ChangeToIdleState()
        {
            ChangeState(_Idle);
        }

        public void ChangeToPrimaryAttack()
        {
            if (_PrimaryAttack != null)
                ChangeState(_PrimaryAttack);
        }

        public void ChangeToSecondaryAttack()
        {
            if (_SecondaryAttack != null)
                ChangeState(_SecondaryAttack);
        }

        public void ChangeToDash()
        {
            if (_Dash != null)
                ChangeState(_Dash);
        }

        public void ChangeToUltimate()
        {
            if (_Ultimate != null)
                ChangeState(_Ultimate);
        }

        public void HaltAutoAttacks()
        {
            m_haltAutoAttack?.Invoke();
        }

        public void ResumeAutoAttacks()
        {
            m_resumeAutoAttack?.Invoke();
        }

        public void ResetAutoTimer()
        {
            m_resetAutoAttack?.Invoke();
        }
    }
}