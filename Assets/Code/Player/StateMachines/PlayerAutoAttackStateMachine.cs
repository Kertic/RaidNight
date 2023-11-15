﻿using System;
using Code.Camera;
using Code.Player.StateMachines.PlayerWeaponStates;
using UnityEngine;

namespace Code.Player.StateMachines
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
        private Weapon.Weapon weapon;

        [SerializeField]
        private PlayerAutoAttackView autoAttackView;

        [SerializeField]
        private float projectileSpeed;

        public PlayerControlsStateMachine _PlayerControlsStateMachine => playerControlsStateMachine;

        private void Awake()
        {
            _attackCooldown = new AttackCooldown(playerData, this, 1.5f * playerData._AttackSpeed); // Hardcoded cooldown between shots
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
            weapon.FireProjectile(Vector2.MoveTowards(transform.position, PlayerControlsStateMachine.mousePos, 1.0f).normalized, projectileSpeed);
            _attackCooldown.SetCooldownLength(1.5f / playerData._AttackSpeed);
            ChangeState(_attackCooldown);
            autoAttackView.SetProgress(1.0f);
            autoAttackView.ChangeViewState(PlayerAutoAttackView.ViewState.COOLINGDOWN);
        }

        public void SetViewProgress(float progress)
        {
            autoAttackView.SetProgress(progress);
        }

        private void OnDrawGizmosSelected()
        {
            if (PlayerCam.Instance != null)
            {
                Gizmos.color = Color.blue;
                Vector3 mousePos = PlayerCam.Instance.Camera.ScreenToWorldPoint(PlayerControlsStateMachine.mousePos);
                mousePos.z = transform.position.z;
                Gizmos.DrawWireCube(mousePos, Vector3.one);
            }
        }
    }
}