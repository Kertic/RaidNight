using Code.Entity.Player.StateMachines.PlayerControlStates.PlayerFaeArcherStates;
using Code.Entity.Player.Views.FaeArcher;
using Code.Entity.Player.Weapon;
using UnityEngine;

namespace Code.Entity.Player.StateMachines
{
    [RequireComponent(typeof(PlayerData), typeof(EntityPhysics), typeof(SpiralWispView))]
    public class PlayerFaeArcherStateMachine : PlayerControlsStateMachine
    {
        [Header("Passive")]
        [SerializeField]
        private TrackingWeapon mischiefWeapon;

        [SerializeField]
        private float mischiefProjectileSpeed;

        [SerializeField]
        private int maxWispCount;

        [Header("Enchanted Arrow")]
        [SerializeField]
        private FireAndForgetWeapon enchantedArrowFireAndForgetWeapon;

        [SerializeField]
        private float enchantedArrowProjectileSpeed, fireEnchantedArrowCastTime, enchantedArrowDamageMultiplier, enchantedArrowCooldown;

        [Header("Fae Assault")]
        [SerializeField]
        private float faeAssaultMaxDistance;

        [SerializeField]
        private float faeAssaultDuration, faeAssaultCooldown;

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
        public SpiralWispView _SpiralWispView { get; private set; }


        private int _currentWispCount;

        private void EnchantedArrowOnEntityHit(Entity hitEntity)
        {
            hitEntity.TakeDamage(enchantedArrowDamageMultiplier * _PlayerData._BaseAttackDamage);
            SetAutoAttackTarget(hitEntity);
            FireMischief(hitEntity);
        }

        private void FireMischief(Entity targetEntity)
        {
            TrackingProjectile projectile = mischiefWeapon.FireProjectile(targetEntity.transform, mischiefProjectileSpeed);
            _SpiralWispView.LaunchWispsToTarget(projectile);
            projectile.m_onHit += hit2Ds =>
            {
                while (_currentWispCount > 0)
                {
                    RemoveWispCharge();
                }
            };
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
            _SpiralWispView = GetComponent<SpiralWispView>();
            _Dash = _Flit = new Flit(_PlayerData, _EntityPhysics, this, flitMaxDistance, flitDuration, flitCooldown);
            _PrimaryAttack = _FireEnchantedArrow = new FireEnchantedArrow(_PlayerData, _EntityPhysics, this, castBarView, enchantedArrowCooldown);
            _SecondaryAttack = _FaeAssault = new FaeAssault(_PlayerData, _EntityPhysics, this, faeAssaultMaxDistance, faeAssaultDuration, faeAssaultCooldown);
            _Ultimate = _CommuneWithFae = new CommuneWithFae(_PlayerData, _EntityPhysics, this, communeCooldown, communeAbilityReductionTime);
        }

        protected override void Update()
        {
            base.Update();
            skillBarUIView.GetIconUIView(0).SetProgress(_FireEnchantedArrow.GetPercentCooldownCompleted(), _FireEnchantedArrow.GetTimeRemainingUntilReady());
            skillBarUIView.GetIconUIView(1).SetProgress(_FaeAssault.GetPercentCooldownCompleted(), _FaeAssault.GetTimeRemainingUntilReady());
            skillBarUIView.GetIconUIView(2).SetProgress(_Flit.GetPercentCooldownCompleted(), _Flit.GetTimeRemainingUntilReady());
            skillBarUIView.GetIconUIView(3).SetProgress(_CommuneWithFae.GetPercentCooldownCompleted(), _CommuneWithFae.GetTimeRemainingUntilReady());
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

        public override void ChangeToSecondaryAttack()
        {
            _FaeAssault.SetDashDuration(faeAssaultDuration);
            _FaeAssault.SetDashDistance(faeAssaultMaxDistance);
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

        public void AddWispCharge()
        {
            if (_currentWispCount == maxWispCount)
            {
                return;
            }

            _SpiralWispView.AddWisp();
            _currentWispCount++;
        }

        public bool RemoveWispCharge()
        {
            if (_currentWispCount == 0)
                return false;
            _SpiralWispView.RemoveWisp();
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