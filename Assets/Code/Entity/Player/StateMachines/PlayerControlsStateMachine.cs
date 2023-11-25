using System;
using System.Collections.Generic;
using Code.Camera;
using Code.Entity.Player.StateMachines.PlayerControlStates;
using Code.Entity.Player.StateMachines.PlayerControlStates.SubStates.Actionable;
using Code.Entity.Player.StateMachines.PlayerControlStates.SuperStates;
using Code.Entity.Player.Views;
using Code.Systems.Views;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Code.Entity.Player.StateMachines
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

        public struct AttackHaltHandle
        {
            private string _id;
            private float _timestamp;

            public AttackHaltHandle(string newID, float newTimestamp)
            {
                _id = newID;
                _timestamp = newTimestamp;
            }
        }

        private static Controls _controls;
        private static Dictionary<InputAction, InputButton> _inputCallbacks;

        [SerializeField]
        protected PlayerCam cam;

        [SerializeField]
        protected TargetIndicatorView targetIndicatorView;

        [Header("Views")]
        [SerializeField]
        protected PlayerCastView castBarView;

        [SerializeField]
        protected SkillBarUIView skillBarUIView;

        private PlayerControlState _currentControlState;
        private List<AttackHaltHandle> _handles;
        public Entity _AutoAttackTarget { get; private set; }
        public PlayerData _PlayerData { get; private set; }
        public Vector2 _MovementDirection { get; private set; }
        public EntityPhysics _EntityPhysics { get; private set; }
        public bool _IsAutoAttackEnabled { get; protected set; }
        protected Idle _Idle { get; set; }
        protected Running _Running { get; set; }
        protected ExecuteSkill _PrimaryAttack { get; set; }
        protected ExecuteSkill _SecondaryAttack { get; set; }
        protected ExecuteSkill _Dash { get; set; }
        protected ExecuteSkill _Ultimate { get; set; }

        public Action m_haltAutoAttack;
        public Action m_resetAutoAttack;
        public Action m_resumeAutoAttack;
        public Action<bool> m_enableAutoAttack;

        public static bool DidEffectProc(float procChance, float luckStat)
        {
            return Random.Range(0.0f, 1.0f) <=  1.0f - Mathf.Pow((1.0f - procChance), luckStat + 1.0f);
        }

        protected virtual void Awake()
        {

            _PlayerData = GetComponent<PlayerData>();
            _IsAutoAttackEnabled = true;
            _EntityPhysics = GetComponent<EntityPhysics>();
            _handles = new List<AttackHaltHandle>();
            castBarView.ChangeViewState(PlayerCastView.ViewState.HIDDEN);
            _currentControlState = _Idle = new Idle(_PlayerData, _EntityPhysics, this);
            _Running = new Running(_PlayerData, _EntityPhysics, this);
            _controls = new Controls();
            _inputCallbacks = new Dictionary<InputAction, InputButton>
            {
                { _controls.Gameplay.PrimaryFire, InputButton.PRIMARY },
                { _controls.Gameplay.SecondaryFire, InputButton.SECONDARY },
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

        protected virtual void Start()
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

        protected virtual void Update()
        {
            Vector2 movementInputVector = _controls.Gameplay.Movement.ReadValue<Vector2>();
            _MovementDirection = movementInputVector;
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

        protected void SetAutoAttackTarget(Entity hitEntity)
        {
            _AutoAttackTarget = hitEntity;
            targetIndicatorView.SetTarget(hitEntity.transform);
        }

        public void ChangeToRunningState()
        {
            ChangeState(_Running);
        }

        public void ChangeToIdleState()
        {
            ChangeState(_Idle);
        }

        public virtual void ChangeToPrimaryAttack()
        {
            if (_PrimaryAttack != null && _PrimaryAttack.IsSkillReady())
            {
                ChangeState(_PrimaryAttack);
            }
        }

        public virtual void ChangeToSecondaryAttack()
        {
            if (_SecondaryAttack != null && _SecondaryAttack.IsSkillReady())
            {
                ChangeState(_SecondaryAttack);
            }
        }

        public virtual void ChangeToDash()
        {
            if (_Dash != null && _Dash.IsSkillReady())
            {
                ChangeState(_Dash);
            }
        }

        public virtual void ChangeToUltimate()
        {
            if (_Ultimate != null && _Ultimate.IsSkillReady())
            {
                ChangeState(_Ultimate);
            }
        }

        public AttackHaltHandle HaltAutoAttacks()
        {
            AttackHaltHandle newHandle = new(Unity.Mathematics.Random.CreateFromIndex((uint)Time.time).ToString(), Time.time);
            _handles.Add(newHandle);
            m_haltAutoAttack?.Invoke();
            return newHandle;
        }

        public void ReleaseAutoAttackHaltHandle(AttackHaltHandle handle)
        {
            _handles.Remove(handle);
            if (_handles.Count == 0)
                m_resumeAutoAttack?.Invoke();
        }

        public void ResetAutoTimer()
        {
            m_resetAutoAttack?.Invoke();
        }

        public void SetAutoAttackEnabled(bool setAttackEnabled)
        {
            _IsAutoAttackEnabled = setAttackEnabled;
            m_enableAutoAttack?.Invoke(setAttackEnabled);
            if (setAttackEnabled && _handles.Count > 0)
            {
                m_haltAutoAttack?.Invoke();
            }
        }
    }
}