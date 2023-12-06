using System;
using System.Collections.Generic;
using Code.Entity.Buffs;
using Code.Entity.Buffs.PlayerBuffs.FaeArcherBuffs;
using Code.Entity.Player.StateMachines.BaseStates.PlayerControlStates;
using Code.Entity.Player.StateMachines.FaeArcher.PlayerFaeArcherStates;
using Code.Entity.Player.Views.FaeArcher;
using Code.Entity.Player.Weapon;
using Code.Entity.Player.Weapon.OnHitEffects;
using Code.Systems.Views;
using UnityEngine;

namespace Code.Entity.Player.StateMachines.FaeArcher
{
    [RequireComponent(typeof(PlayerData), typeof(EntityPhysics), typeof(FaeArcherView))]
    public class PlayerFaeArcherStateMachine : PlayerControlsStateMachine
    {
        [Header("Passive")]
        [SerializeField]
        private Sprite mischiefDotDebuffIcon;

        [SerializeField]
        private float mischiefDotTotalDamage, mischiefDotDuration;


        [SerializeField]
        private int maxWispCount, mischiefTotalDamageTicks;

        [SerializeField]
        private MischiefStateMachine mischiefStateMachine;

        [Header("Enchanted Arrow")]
        [SerializeField]
        private FireAndForgetWeapon enchantedArrowFireAndForgetWeapon;

        [SerializeField]
        private float enchantedArrowProjectileSpeed, fireEnchantedArrowCastTime, enchantedArrowDamageMultiplier, enchantedArrowCooldown;

        [Header("Fae Assault")]
        [SerializeField]
        private float faeAssaultDuration;

        [SerializeField]
        private Sprite faeAssaultBuffIcon;

        [SerializeField]
        private float faeAssaultCooldown, faeAssaultAttackSpeedAmp;

        [Header("Flit")]
        [SerializeField]
        private float flitMaxDistance;

        [SerializeField]
        private float flitDuration, flitCooldown;

        [Header("Commune with Fae")]
        [SerializeField]
        private float communeCooldown;

        [SerializeField]
        private float communeAbilityReductionTime;


        private Flit _Flit { get; set; }
        private FireEnchantedArrow _FireEnchantedArrow { get; set; }
        private FaeAssault _FaeAssault { get; set; }
        private CommuneWithFae _CommuneWithFae { get; set; }
        public FaeArcherView _SpiralWispView { get; private set; }

        public List<OnHitEffect> _OnHitEffects => _onHitEffects;

        public Action<Entity> m_onHitEntity;

        private int _currentWispCount;

        private BuffView _faeAssaultBuffView;
        private List<OnHitEffect> _onHitEffects;

        private void EnchantedArrowOnEntityHit(Entity hitEntity)
        {
            hitEntity.TakeDamage(enchantedArrowDamageMultiplier * _PlayerData._BaseAttackDamage);
            foreach (OnHitEffect onHitEffect in _OnHitEffects)
            {
                onHitEffect.ApplyEffectToEntity(hitEntity);
            }
            m_onHitEntity?.Invoke(hitEntity);
        }

        private void MischiefOnHit(Entity hitEntity)
        {
            MischiefDamageDebuff dotDebuff = new MischiefDamageDebuff(hitEntity, mischiefDotDuration, mischiefDotDebuffIcon, mischiefDotTotalDamage, mischiefTotalDamageTicks);
            hitEntity.AddBuff(dotDebuff);
        }

        private void OnValidate()
        {
            if (_SkillBarUIView != null && _SkillBarUIView.GetSkillCount() != 4)
            {
                Debug.LogError("ERROR: " + gameObject.name + " has been assigned a skill bar with " + _SkillBarUIView.GetSkillCount() + ". " + "It must have 4 skills.");
                skillBarUIView = null;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _onHitEffects = new List<OnHitEffect>();
            _SpiralWispView = GetComponent<FaeArcherView>();
            _Dash = _Flit = new Flit(_PlayerData, _EntityPhysics, this, flitMaxDistance, flitDuration, flitCooldown);
            _PrimaryAttack = _FireEnchantedArrow = new FireEnchantedArrow(_PlayerData, _EntityPhysics, this, castBarView, enchantedArrowCooldown);
            _SecondaryAttack = _FaeAssault = new FaeAssault(_PlayerData, _EntityPhysics, this, faeAssaultDuration, faeAssaultCooldown, faeAssaultAttackSpeedAmp, faeAssaultBuffIcon);
            _Ultimate = _CommuneWithFae = new CommuneWithFae(_PlayerData, _EntityPhysics, this, communeCooldown, communeAbilityReductionTime);
            mischiefStateMachine.m_onFiredTrackingProjectile += projectile =>
            {
                _SpiralWispView.AttachWispsToProjectile(projectile);
                projectile.m_onEntityHit += hit2Ds =>
                {
                    foreach (RaycastHit2D hit in hit2Ds)
                    {
                        Entity entity = hit.collider.gameObject.GetComponent<Entity>();
                        if (entity != null)
                        {
                            MischiefOnHit(entity);
                            break;
                        }
                    }

                    while (_currentWispCount > 0)
                    {
                        RemoveWispCharge();
                    }
                };
            };
        }

        protected override void Update()
        {
            base.Update();
            _SkillBarUIView.GetIconUIView(0).SetProgress(_FireEnchantedArrow.GetPercentCooldownCompleted(), _FireEnchantedArrow.GetTimeRemainingUntilReady());
            _SkillBarUIView.GetIconUIView(1).SetProgress(_FaeAssault.GetPercentCooldownCompleted(), _FaeAssault.GetTimeRemainingUntilReady());
            _SkillBarUIView.GetIconUIView(2).SetProgress(_Flit.GetPercentCooldownCompleted(), _Flit.GetTimeRemainingUntilReady());
            _SkillBarUIView.GetIconUIView(3).SetProgress(_CommuneWithFae.GetPercentCooldownCompleted(), _CommuneWithFae.GetTimeRemainingUntilReady());
        }

        protected override void Start()
        {
            base.Start();
            SetAutoAttackEnabled(false);
        }

        public override void ChangeToPrimaryAttack()
        {
            _FireEnchantedArrow.SetCastTime(fireEnchantedArrowCastTime * (1.0f / _PlayerData._AttackSpeed));
            _FireEnchantedArrow.SetMaxCooldown(enchantedArrowCooldown * (1.0f / _PlayerData._AttackSpeed));
            base.ChangeToPrimaryAttack();
        }

        public override void ChangeToDash()
        {
            _Flit.SetDashDuration(flitDuration);
            _Flit.SetDashDistance(flitMaxDistance);
            base.ChangeToDash();
        }

        public override void ChangeToSecondaryAttack()
        {
            _FaeAssault.SetBuffDuration(faeAssaultDuration);
            base.ChangeToSecondaryAttack();
        }

        public override void ChangeToUltimate()
        {
            _CommuneWithFae.SetCooldownReductionTime(communeAbilityReductionTime);
            base.ChangeToUltimate();
        }

        public void FireEnchantedArrowWeapon(Vector2 targetLocation)
        {
            Projectile enchantedArrow = enchantedArrowFireAndForgetWeapon.FireProjectile(targetLocation - (Vector2)transform.position, enchantedArrowProjectileSpeed);
            enchantedArrow.m_onEntityHit += hit2Ds =>
            {
                foreach (RaycastHit2D hit in hit2Ds)
                {
                    Entity entity = hit.collider.gameObject.GetComponent<Entity>();
                    EnchantedArrowOnEntityHit(entity);
                }
            };
        }

        public void AddWispCharge()
        {
            if (_currentWispCount == maxWispCount)
            {
                return;
            }

            if (!mischiefStateMachine.GetCanAddWisps())
            {
                return;
            }

            _SpiralWispView.AddWispToSwirlingWisps();
            _currentWispCount++;
        }

        public bool RemoveWispCharge()
        {
            if (_currentWispCount == 0)
                return false;
            _SpiralWispView.RemoveWispFromSwirlingWisps();
            _currentWispCount--;
            return true;
        }

        public void ReduceNonUltimateSkillCooldown(float reductionTime)
        {
            _Flit.ReduceCooldown(reductionTime);
            _FaeAssault.ReduceCooldown(reductionTime);
            _FireEnchantedArrow.ReduceCooldown(reductionTime);
        }
    }
}