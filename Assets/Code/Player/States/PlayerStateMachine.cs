using System.Data.Common;
using Code.Player.States.SubStates.Actionable;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Player.States
{
    [RequireComponent(typeof(PlayerData), typeof(PlayerPhysics))]
    public class PlayerStateMachine : MonoBehaviour
    {
        private PlayerState _currentState;
        private Controls _controls;
        public PlayerData _PlayerData { get; private set; }
        public PlayerPhysics _PlayerPhysics { get; private set; }
        public Idle _Idle { get; private set; }
        public Running _Running { get; private set; }
        public Running _Running2 { get; private set; }

        private void Awake()
        {
            _PlayerData = GetComponent<PlayerData>();
            _PlayerPhysics = GetComponent<PlayerPhysics>();
            _Idle = new Idle(_PlayerData, _PlayerPhysics, this);
            _Running = new Running(_PlayerData, _PlayerPhysics, this);
            _Running2 = new Running(_PlayerData, _PlayerPhysics, this);
            _controls = new Controls();
            _controls.Gameplay.Movement.performed += OnMovementInput;
            _controls.Gameplay.Dash.performed += PressJumpInput;
            _controls.Gameplay.PrimaryFire.performed += (InputAction.CallbackContext context) => { PressAttackInput(true, context); };
            _controls.Gameplay.SecondaryFire.performed += (InputAction.CallbackContext context) => { PressAttackInput(false, context); };

            _controls.Gameplay.Movement.canceled += OnMovementInputEnd;
            _controls.Gameplay.Dash.canceled += ReleaseJumpInput;
            _controls.Gameplay.PrimaryFire.canceled += (InputAction.CallbackContext context) => { ReleaseFireInput(true, context); };
            _controls.Gameplay.SecondaryFire.canceled += (InputAction.CallbackContext context) => { ReleaseFireInput(false, context); };
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

        private void PressAttackInput(bool isPrimary, InputAction.CallbackContext context)
        {
            if (isPrimary)
            {
                _currentState.OnReceiveButtonInput(PlayerState.InputButton.PRIMARY);
                return;
            }

            _currentState.OnReceiveButtonInput(PlayerState.InputButton.SECONDARY);
        }

        private void ReleaseFireInput(bool isPrimary, InputAction.CallbackContext context)
        {
            if (isPrimary)
            {
                _currentState.OnReleaseButtonInput(PlayerState.InputButton.PRIMARY);
                return;
            }

            _currentState.OnReleaseButtonInput(PlayerState.InputButton.SECONDARY);
        }

        private void PressJumpInput(InputAction.CallbackContext context)
        {
            _currentState.OnReceiveButtonInput(PlayerState.InputButton.DASH);
        }

        private void ReleaseJumpInput(InputAction.CallbackContext context)
        {
            _currentState.OnReleaseButtonInput(PlayerState.InputButton.DASH);
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
            if (_controls.Gameplay.PrimaryFire.IsPressed())
                _currentState.OnHoldButtonInput(PlayerState.InputButton.PRIMARY);
            if (_controls.Gameplay.SecondaryFire.IsPressed())
                _currentState.OnHoldButtonInput(PlayerState.InputButton.SECONDARY);
        }

        public void ChangeState(PlayerState newState)
        {
            _currentState.OnStateExit();
            _currentState = newState;
            _currentState.OnStateEnter();
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
    }
}