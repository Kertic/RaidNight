using System;
using System.Collections.Generic;
using Code.Camera;
using Code.Player.StateMachines.PlayerControlStates;
using Code.Player.StateMachines.PlayerControlStates.SubStates.Actionable;
using Code.Player.StateMachines.PlayerControlStates.SubStates.UseSkill;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Player.StateMachines
{
    [RequireComponent(typeof(PlayerData), typeof(PlayerPhysics))]
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
        public PlayerData _PlayerData { get; private set; }
        public PlayerPhysics _PlayerPhysics { get; private set; }
        public Idle _Idle { get; private set; }
        public Running _Running { get; private set; }
        public Dash _Dash { get; private set; }

        public Action m_haltAutoAttack;
        public Action m_resumeAutoAttack;

        private void Awake()
        {
            _PlayerData = GetComponent<PlayerData>();
            _PlayerPhysics = GetComponent<PlayerPhysics>();
            _currentControlState = _Idle = new Idle(_PlayerData, _PlayerPhysics, this);
            _Running = new Running(_PlayerData, _PlayerPhysics, this);
            _Dash = new Dash(_PlayerData, _PlayerPhysics, this);
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

        public void ChangeToDashingState(Vector2 dashVector, float dashDuration)
        {
            _Dash.SetDashVectorAndDuration(dashVector, dashDuration);
            ChangeState(_Dash);
        }

        public void HaltAutoAttacks()
        {
            m_haltAutoAttack();
        }

        public void ResumeAutoAttacks()
        {
            m_resumeAutoAttack();
        }
    }
}