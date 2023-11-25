using Code.Entity.Player.StateMachines.PlayerControlStates.PlayerFaeArcherStates;
using Code.Entity.Player.Views.FaeArcher;
using Code.Entity.Player.Weapon;
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
        private float mischiefProcChance;

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

        private int _currentWispCount;

        private Flit _Flit { get; set; }
        private FireEnchantedArrow _FireEnchantedArrow { get; set; }
        private FaeAssault _FaeAssault { get; set; }
        public FaeArcherView _FaeArcherView { get; private set; }

        private void EnchantedArrowOnEntityHit(Entity hitEntity)
        {
            hitEntity.TakeDamage(enchantedArrowDamageMultiplier * _PlayerData._BaseAttackDamage);
            SetAutoAttackTarget(hitEntity);
            if (_currentWispCount > 0)
            {
                var oldProcChance = mischiefProcChance;
                mischiefProcChance = 1.0f;
                PotentiallyAddMischiefCharge();
                mischiefProcChance = oldProcChance;
            }

            PotentiallyAddMischiefCharge();
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
            _SecondaryAttack = _FaeAssault = new FaeAssault(_PlayerData, _EntityPhysics, this, faeAssaultMaxDistance, faeAssaultDuration, faeAssaultCooldown);
        }

        protected override void Update()
        {
            base.Update();
            skillBarUIView.GetIconUIView(0).SetProgress(_FireEnchantedArrow.GetPercentCooldownCompleted(), _FireEnchantedArrow.GetTimeRemainingUntilReady());
            skillBarUIView.GetIconUIView(1).SetProgress(_FaeAssault.GetPercentCooldownCompleted(), _FaeAssault.GetTimeRemainingUntilReady());
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

        public override void ChangeToSecondaryAttack()
        {
            _FaeAssault.SetDashDuration(faeAssaultDuration);
            _FaeAssault.SetDashDistance(faeAssaultMaxDistance);

            if (_SecondaryAttack != null && _SecondaryAttack.IsSkillReady())
            {
                if (ConsumeWispCharge()) // TODO: This likely shouldn't need to be an identical check to the parents check
                {
                    _FaeAssault.SetIsEnhancedFromMischiefStack(true);
                }
            }


            base.ChangeToSecondaryAttack();
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

        public void PotentiallyAddMischiefCharge()
        {
            if (_currentWispCount == maxWispCount || mischiefProcChance == 0.0f)
            {
                return;
            }

            if (!DidEffectProc(mischiefProcChance, _PlayerData._LuckChance))
            {
                return;
            }

            _FaeArcherView.AddWisp();
            SetAutoAttackEnabled(true);
            _currentWispCount++;
        }

        public bool RemoveMischiefCharge()
        {
            if (_currentWispCount == 0)
                return false;
            _FaeArcherView.RemoveWisp();
            _currentWispCount--;
            if (_currentWispCount <= 0)
                SetAutoAttackEnabled(false);
            return true;
        }

        public bool ConsumeWispCharge()
        {
            return RemoveMischiefCharge();
            // Reduce Ultimate cooldown by one second
        }
    }
}