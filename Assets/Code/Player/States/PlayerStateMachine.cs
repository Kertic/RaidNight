using System.Collections.Generic;
using System.Data.Common;
using Code.Player.States.SubStates.Actionable;
using Code.Player.States.SubStates.UseSkill;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Player.States
{
    [RequireComponent(typeof(PlayerData), typeof(PlayerPhysics))]
    public class PlayerStateMachine : MonoBehaviour
    {
        private static Controls _controls;
        private static Dictionary<InputAction, PlayerState.InputButton> _inputCallbacks;

        private PlayerState _currentState;
        public PlayerData _PlayerData { get; private set; }
        public PlayerPhysics _PlayerPhysics { get; private set; }
        public Idle _Idle { get; private set; }
        public Running _Running { get; private set; }
        public Dash _Dash { get; private set; }

        private void Awake()
        {
            _PlayerData = GetComponent<PlayerData>();
            _PlayerPhysics = GetComponent<PlayerPhysics>();
            _currentState = _Idle = new Idle(_PlayerData, _PlayerPhysics, this);
            _Running = new Running(_PlayerData, _PlayerPhysics, this);
            _Dash = new Dash(_PlayerData, _PlayerPhysics, this);
            _controls = new Controls();
            _inputCallbacks = new Dictionary<InputAction, PlayerState.InputButton>
            {
                { _controls.Gameplay.PrimaryFire, PlayerState.InputButton.PRIMARY },
                { _controls.Gameplay.SecondaryFire, PlayerState.InputButton.PRIMARY },
                { _controls.Gameplay.Dash, PlayerState.InputButton.DASH },
                { _controls.Gameplay.Ultimate, PlayerState.InputButton.ULTIMATE },
            };
            _controls.Gameplay.Movement.performed += OnMovementInput;
            _controls.Gameplay.Movement.canceled += OnMovementInputEnd;

            foreach (KeyValuePair<InputAction, PlayerState.InputButton> action in _inputCallbacks)
            {
                action.Key.performed += (InputAction.CallbackContext context) => { _currentState.OnReceiveButtonInput(action.Value); };
                action.Key.canceled += (InputAction.CallbackContext context) => { _currentState.OnReleaseButtonInput(action.Value); };
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
            _currentState.OnStateEnter();
        }


        private void OnMovementInput(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            _currentState.OnReceiveMovementInput(direction);
        }

        private void OnMovementInputEnd(InputAction.CallbackContext context)
        {
            _currentState.OnReleaseMovementInput();
        }

        private void Update()
        {
            Vector2 movementInputVector = _controls.Gameplay.Movement.ReadValue<Vector2>();
            if (_controls.Gameplay.Movement.IsPressed())
                _currentState.OnHoldMovementInput(movementInputVector);
            foreach (KeyValuePair<InputAction, PlayerState.InputButton> action in _inputCallbacks)
            {
                if (action.Key.IsPressed())
                    _currentState.OnHoldButtonInput(action.Value);
            }
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            _currentState.OnCollisionEnter2D(other);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            _currentState.OnCollisionExit2D(other);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            _currentState.OnCollisionStay2D(other);
        }

        private void FixedUpdate()
        {
            _currentState.PhysicsUpdate();
        }

        protected void ChangeState(PlayerState newState)
        {
            _currentState.OnStateExit();
            _currentState = newState;
            _currentState.OnStateEnter();
        }

        public void ChangeToRunningState()
        {
            ChangeState(_Running);
        }

        public void ChangeToIdleState()
        {
            ChangeState(_Idle);
        }

        public void ChangeToDashingState()
        {
            ChangeState(_Dash);
        }
    }
}