using Code.Camera;
using Code.Entity.Player.StateMachines.PlayerWeaponStates;
using Code.Entity.Player.Views;
using Code.Entity.Player.Weapon;
using UnityEngine;

namespace Code.Entity.Player.StateMachines
{
    public class PlayerAutoAttackStateMachine : MonoBehaviour
    {
        private PlayerWeaponState _currentWeaponState;
        private AttackCooldown _attackCooldown;
        private AttackCharging _attackCharging;
        private AttackHalted _attackHalted;

        [SerializeField]
        private PlayerData playerData;

        [SerializeField]
        private PlayerControlsStateMachine playerControlsStateMachine;

        [SerializeField]
        private Weapon.TrackingWeapon weapon;

        [SerializeField]
        private PlayerCastView castView;

        [SerializeField]
        private float projectileSpeed, attackSpeedToCooldownMultiplier;

        public PlayerControlsStateMachine _PlayerControlsStateMachine => playerControlsStateMachine;

        private void Awake()
        {
            _attackCooldown = new AttackCooldown(playerData, this, attackSpeedToCooldownMultiplier * playerData._AttackSpeed); // Hardcoded cooldown between shots
            _attackCharging = new AttackCharging(playerData, this);
            _attackHalted = new AttackHalted(playerData, this);
            ChangeState(_attackCharging);
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
            castView.SetProgress(1.0f);
            castView.ChangeViewState(PlayerCastView.ViewState.IDLE);
        }

        public void BeginAttackCharging()
        {
            ChangeState(_attackCharging);
            castView.SetProgress(0.0f);
            castView.ChangeViewState(PlayerCastView.ViewState.CHARGING);
        }

        public void FireAutoAttack()
        {
            TrackingProjectile projectile = weapon.FireProjectile(playerControlsStateMachine._AutoAttackTarget.transform, projectileSpeed);
            projectile.m_onHit += hit2Ds =>
            {
                foreach (RaycastHit2D hit in hit2Ds)
                {
                    Entity entity = hit.collider.gameObject.GetComponent<Entity>();
                    if (entity != null)
                    {
                        entity.TakeDamage(playerData._BaseAttackDamage);
                    }
                }
            };
            _attackCooldown.SetCooldownLength(attackSpeedToCooldownMultiplier / playerData._AttackSpeed);
            ChangeState(_attackCooldown);
            castView.SetProgress(1.0f);
            castView.ChangeViewState(PlayerCastView.ViewState.COOLINGDOWN);
        }

        public void SetViewProgress(float progress)
        {
            castView.SetProgress(progress);
        }

        private void OnDrawGizmosSelected()
        {
            if (PlayerCam.Instance != null)
            {
                Gizmos.color = Color.blue;
                Vector3 mousePos = PlayerCam.mousePosition;
                //mousePos.z = transform.position.z;
                Gizmos.DrawWireCube(mousePos, Vector3.one);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(mousePos, transform.position);
            }
        }
    }
}