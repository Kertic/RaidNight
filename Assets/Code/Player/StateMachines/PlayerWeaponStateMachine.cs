using Code.Player.StateMachines.PlayerWeaponStates;
using UnityEngine;

namespace Code.Player.StateMachines
{
    [RequireComponent(typeof(PlayerData), typeof(PlayerControlsStateMachine))]
    public class PlayerWeaponStateMachine : MonoBehaviour
    {
        public PlayerData _PlayerData { get; private set; }
        public PlayerControlsStateMachine _PlayerControlsStateMachine { get; private set; }

        private PlayerWeaponState _currentWeaponState;
        private AttackCooldown _attackCooldown;
        private AttackCharging _attackCharging;
        private AttackHalted _attackHalted;

        [SerializeField]
        private PlayerAutoAttackView autoAttackView;

        private void Awake()
        {
            _PlayerData = GetComponent<PlayerData>();
            _PlayerControlsStateMachine = GetComponent<PlayerControlsStateMachine>();
            _attackCooldown = new AttackCooldown(_PlayerData, this, 1.5f); // Hardcoded cooldown between shots
            _attackCharging = new AttackCharging(_PlayerData, this);
            _attackHalted = new AttackHalted(_PlayerData, this);
            ChangeState(_attackCharging);
        }

        private void Start()
        {
            _currentWeaponState.OnStateEnter();
        }

        private void FixedUpdate()
        {
            _currentWeaponState.StateUpdate();
        }

        private void ChangeState(PlayerWeaponState newWeaponState)
        {
            _currentWeaponState?.OnStateExit();
            _currentWeaponState = newWeaponState;
            _currentWeaponState?.OnStateEnter();
        }

        public void HaltAttacks()
        {
            ChangeState(_attackHalted);
            autoAttackView.SetProgress(1.0f);
            autoAttackView.ChangeViewState(PlayerAutoAttackView.ViewState.IDLE);
        }

        public void BeginAttackCharging()
        {
            ChangeState(_attackCharging);
            autoAttackView.SetProgress(0.0f);
            autoAttackView.ChangeViewState(PlayerAutoAttackView.ViewState.CHARGING);
        }

        public void FireAutoAttack()
        {
            //Fire Attack, apply on hit effects?
            ChangeState(_attackCooldown);
            autoAttackView.SetProgress(0.0f);
            autoAttackView.ChangeViewState(PlayerAutoAttackView.ViewState.COOLINGDOWN);
        }

        public void SetViewProgress(float progress)
        {
            autoAttackView.SetProgress(progress);
        }
    }
}