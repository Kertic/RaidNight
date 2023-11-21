using Code.Camera;
using Code.Entity.Player.StateMachines.PlayerWeaponStates;
using Code.Entity.Player.Views;
using Code.Entity.Player.Weapon;
using UnityEngine;

namespace Code.Entity.Player.StateMachines
{
    public class PlayerAutoAttackStateMachine : MonoBehaviour
    {
        protected PlayerWeaponState m_currentWeaponState;
        protected AttackCooldown m_attackCooldown;
        protected AttackCharging m_attackCharging;
        protected AttackHalted m_attackHalted;
        protected AttackDisabled m_attackDisabled;


        [SerializeField]
        protected PlayerData playerData;

        [SerializeField]
        protected PlayerControlsStateMachine playerControlsStateMachine;

        [SerializeField]
        protected Weapon.TrackingWeapon weapon;

        [SerializeField]
        protected PlayerCastView autoAttackCastView;

        [SerializeField]
        protected float projectileSpeed, attackSpeedToCooldownMultiplier;

        public PlayerControlsStateMachine _PlayerControlsStateMachine => playerControlsStateMachine;

        private void Awake()
        {
            m_attackCooldown = new AttackCooldown(playerData, this, attackSpeedToCooldownMultiplier * playerData._AttackSpeed);
            m_attackCharging = new AttackCharging(playerData, this);
            m_attackHalted = new AttackHalted(playerData, this);
            m_attackDisabled = new AttackDisabled(playerData, this);
            playerControlsStateMachine.m_enableAutoAttack += isAutoAttackEnabled =>
            {
                if (isAutoAttackEnabled)
                {
                    return;
                }

                DisableAttacks();
                autoAttackCastView.ChangeViewState(PlayerCastView.ViewState.HIDDEN);
            };
            BeginAttackCharging();
        }

        private void FixedUpdate()
        {
            m_currentWeaponState.StateUpdate();
        }

        protected void ChangeState(PlayerWeaponState newWeaponState)
        {
            m_currentWeaponState?.OnStateExit();
            m_currentWeaponState = newWeaponState;
            m_currentWeaponState?.OnStateEnter();
        }

        public void DisableAttacks()
        {
            ChangeState(m_attackDisabled);
        }

        public void HaltAttacks()
        {
            ChangeState(m_attackHalted);
            autoAttackCastView.SetProgress(1.0f);
            autoAttackCastView.ChangeViewState(PlayerCastView.ViewState.IDLE);
        }

        public void BeginAttackCharging()
        {
            ChangeState(m_attackCharging);
            autoAttackCastView.SetProgress(0.0f);
            autoAttackCastView.ChangeViewState(PlayerCastView.ViewState.CHARGING);
        }

        public void BeginAttackCooldown()
        {
            m_attackCooldown.SetCooldownLength(attackSpeedToCooldownMultiplier / playerData._AttackSpeed);
            ChangeState(m_attackCooldown);
            autoAttackCastView.SetProgress(1.0f);
            autoAttackCastView.ChangeViewState(PlayerCastView.ViewState.COOLINGDOWN);
        }

        public virtual void FireAutoAttack()
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

            BeginAttackCooldown();
        }

        public void SetViewProgress(float progress)
        {
            autoAttackCastView.SetProgress(progress);
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