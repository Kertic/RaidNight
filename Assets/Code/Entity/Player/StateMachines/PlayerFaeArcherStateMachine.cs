using System;
using Code.Entity.Player.StateMachines.PlayerControlStates.PlayerFaeArcherStates;
using Code.Entity.Player.Views.FaeArcher;
using Code.Entity.Player.Weapon;
using UnityEditor;
using UnityEngine;

namespace Code.Entity.Player.StateMachines
{
    [RequireComponent(typeof(PlayerData), typeof(EntityPhysics), typeof(FaeArcherView))]
    public class PlayerFaeArcherStateMachine : PlayerControlsStateMachine
    {
        [Header("Passive")]
        [SerializeField]
        private int maxWispCount;

        [SerializeField]
        private TrackingWeapon wispLauncher;

        [SerializeField]
        private float wispLaunchSpeed;

        [Header("Enchanted Arrow")]
        [SerializeField]
        private FireAndForgetWeapon enchantedArrowFireAndForgetWeapon;

        [SerializeField]
        private float enchantedArrowProjectileSpeed, fireEnchantedArrowCastTime, enchantedArrowDamageMultiplier, enchantedArrowCooldown;

        [Header("Flit")]
        [SerializeField]
        private float flitMaxDistance;

        [SerializeField]
        private float flitDuration, flitCooldown;

        private int _currentWispCount;

        private Flit _Flit { get; set; }
        private FireEnchantedArrow _FireEnchantedArrow { get; set; }
        public FaeArcherView _FaeArcherView { get; private set; }

        private void EnchantedArrowOnEntityHit(Entity hitEntity)
        {
            hitEntity.TakeDamage(enchantedArrowDamageMultiplier * _PlayerData._BaseAttackDamage);
            SetAutoAttackTarget(hitEntity);
            AddMischiefCharge();
        }

        private void OnValidate()
        {
            if (skillBarUIView != null && skillBarUIView.GetSkillCount() != 4)
            {
                Debug.LogError("ERROR: " + gameObject.name + " has been assigned a skill bar with " + skillBarUIView.GetSkillCount() + ". " + "It must have 4 skills.");
                skillBarUIView = null;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _FaeArcherView = GetComponent<FaeArcherView>();
            _Dash = _Flit = new Flit(_PlayerData, _EntityPhysics, this, flitMaxDistance, flitDuration, flitCooldown);
            _PrimaryAttack = _FireEnchantedArrow = new FireEnchantedArrow(_PlayerData, _EntityPhysics, this, castBarView, enchantedArrowCooldown);
        }

        protected override void Update()
        {
            base.Update();
            Debug.Log("EnchArrow CD:" + _FireEnchantedArrow.GetPercentCooldownCompleted());
            skillBarUIView.GetIconUIView(0).SetProgress(_FireEnchantedArrow.GetPercentCooldownCompleted() , _FireEnchantedArrow.GetTimeRemainingUntilReady());
            skillBarUIView.GetIconUIView(2).SetProgress(_Flit.GetPercentCooldownCompleted(), _Flit.GetTimeRemainingUntilReady());
        }

        protected override void Start()
        {
            base.Start();
            SetAutoAttackEnabled(false);
        }

        public override void ChangeToPrimaryAttack()
        {
            _FireEnchantedArrow.SetCastTime(fireEnchantedArrowCastTime);
            base.ChangeToPrimaryAttack();
        }

        public override void ChangeToDash()
        {
            _Flit.SetDashDuration(flitDuration);
            _Flit.SetDashDistance(flitMaxDistance);
            base.ChangeToDash();
        }

        public void FireEnchantedArrowWeapon(Vector2 targetLocation)
        {
            Projectile enchantedArrow = enchantedArrowFireAndForgetWeapon.FireProjectile(targetLocation - (Vector2)transform.position, enchantedArrowProjectileSpeed);
            enchantedArrow.m_onHit += hit2Ds =>
            {
                foreach (RaycastHit2D hit in hit2Ds)
                {
                    Entity entity = hit.collider.gameObject.GetComponent<Entity>();
                    if (entity != null)
                    {
                        EnchantedArrowOnEntityHit(entity);
                    }
                }
            };
        }

        public void AddMischiefCharge()
        {
            if (_currentWispCount == maxWispCount)
                return;
            _FaeArcherView.AddWisp();
            SetAutoAttackEnabled(true);
            _currentWispCount++;
        }

        public void RemoveMischiefCharge()
        {
            if (_currentWispCount == 0)
                return;
            _FaeArcherView.RemoveWisp();
            _currentWispCount--;
            if (_currentWispCount <= 0)
                SetAutoAttackEnabled(false);
        }

        public void LaunchWispAttack(Transform target)
        {
            // Do ONHits here
            wispLauncher.FireProjectile(target, wispLaunchSpeed); // Animate wisp attack               
        }
    }
}