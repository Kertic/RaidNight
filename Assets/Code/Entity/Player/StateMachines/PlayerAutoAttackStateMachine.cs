using System;
using System.Collections.Generic;
using Code.Camera;
using Code.Entity.Player.StateMachines.BaseStates.PlayerControlStates.SubStates.ExecuteSkill;
using Code.Entity.Player.Views;
using Code.Entity.Player.Weapon;
using Code.Entity.Player.Weapon.PlayerWeaponStates;
using UnityEngine;

namespace Code.Entity.Player.StateMachines
{
    [RequireComponent(typeof(TrackingWeapon))]
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
        protected TrackingWeapon weapon;

        [SerializeField]
        protected PlayerCastView autoAttackCastView;

        [SerializeField]
        protected TargetIndicatorView targetIndicatorView, targetSelectionIndicatorView;

        [SerializeField]
        protected float projectileSpeed, attackSpeedToCooldownMultiplier, targetingDistanceThreshold;

        public PlayerControlsStateMachine _PlayerControlsStateMachine => playerControlsStateMachine;

        public Entity _AutoAttackHightlightedTarget { get; private set; }
        public Entity _AutoAttackTarget { get; private set; }

        private SetAutoAttackTarget _setAttackTargetSkill;
        private PlayerControlsStateMachine.AttackHaltHandle? _noTargetHandle;

        private void Awake()
        {
            m_attackCooldown = new AttackCooldown(playerData, this, attackSpeedToCooldownMultiplier * playerData._AttackSpeed);
            m_attackCharging = new AttackCharging(playerData, this);
            m_attackHalted = new AttackHalted(playerData, this);
            m_attackDisabled = new AttackDisabled(playerData, this);
            _setAttackTargetSkill = new SetAutoAttackTarget(this, playerData, _PlayerControlsStateMachine._EntityPhysics, _PlayerControlsStateMachine, 0.0f);
            playerControlsStateMachine.SetPrimaryAttackAction(_setAttackTargetSkill);
            playerControlsStateMachine.m_enableAutoAttack += isAutoAttackEnabled =>
            {
                if (isAutoAttackEnabled)
                {
                    return;
                }

                DisableAttacks();
                autoAttackCastView.ChangeViewState(PlayerCastView.ViewState.HIDDEN);
            };
            HaltAttacks();
        }

        private void Start()
        {
            _noTargetHandle = playerControlsStateMachine.HaltAutoAttacks();
        }

        private void FixedUpdate()
        {
            m_currentWeaponState.StateUpdate();
        }

        private void Update()
        {
            GameObject[] selectableObjects = GameObject.FindGameObjectsWithTag("NonPlayerEntity");
            targetSelectionIndicatorView.SetVisible(false);
            _AutoAttackHightlightedTarget = null;
            foreach (GameObject selectableObject in selectableObjects)
            {
                if (Vector2.Distance(PlayerCam.mousePosition, selectableObject.transform.position) <= targetingDistanceThreshold)
                {
                    SetAutoAttackSelection(selectableObject.transform);
                    break;
                }
            }
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

        protected void SetAutoAttackSelection(Transform targetedEntity)
        {
            _AutoAttackHightlightedTarget = targetedEntity.GetComponent<Entity>();
            targetSelectionIndicatorView.SetTarget(targetedEntity.transform);
            targetSelectionIndicatorView.SetVisible( _AutoAttackHightlightedTarget != _AutoAttackTarget );
        }

        private void SetAutoAttackTargetToHighlightedTarget()
        {
            _AutoAttackTarget = _AutoAttackHightlightedTarget;
            targetIndicatorView.SetTarget(_AutoAttackHightlightedTarget.transform);
            targetIndicatorView.SetVisible(true);

            if (_noTargetHandle != null)
            {
                playerControlsStateMachine.ReleaseAutoAttackHaltHandle(_noTargetHandle.Value);
            }

            _noTargetHandle = null;
        }

        private void ClearAutoAttackTarget()
        {
            _AutoAttackTarget = null;
            targetIndicatorView.SetVisible(false);
            if (_noTargetHandle == null)
            {
                _noTargetHandle = playerControlsStateMachine.HaltAutoAttacks();
            }
        }

        public void SetTarget()
        {
            if (_AutoAttackHightlightedTarget == null)
            {
                ClearAutoAttackTarget();
                return;
            }

            SetAutoAttackTargetToHighlightedTarget();
        }

        public virtual void FireAutoAttack()
        {
            if (_AutoAttackTarget.transform == null)
            {
                HaltAttacks();
                return;
            }

            TrackingProjectile projectile = weapon.FireProjectile(_AutoAttackTarget, projectileSpeed);
            projectile.m_onHitTarget += entity =>
            {
                entity.TakeDamage(playerData._BaseAttackDamage, Color.white);
                playerControlsStateMachine.ApplyOnHitEffects(entity);
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
                Gizmos.DrawWireCube(mousePos, Vector3.one);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(mousePos, transform.position);
            }
        }
    }
}