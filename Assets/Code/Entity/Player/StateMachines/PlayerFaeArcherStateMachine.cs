using Code.Entity.Player.StateMachines.PlayerControlStates.PlayerFaeArcherStates;
using Code.Entity.Player.Weapon;
using UnityEngine;

namespace Code.Entity.Player.StateMachines
{
    [RequireComponent(typeof(PlayerData), typeof(EntityPhysics))]
    public class PlayerFaeArcherStateMachine : PlayerControlsStateMachine
    {
        [Header("Enchanted Arrow")]
        [SerializeField]
        private Weapon.FireAndForgetWeapon enchantedArrowFireAndForgetWeapon;

        [SerializeField]
        private float enchantedArrowProjectileSpeed, fireEnchantedArrowCastTime, enchantedArrowDamageMultiplier;

        [Header("Flit")]
        [SerializeField]
        private float flitMaxDistance;

        [SerializeField]
        private float flitDuration;


        private Flit _Flit { get; set; }
        private FireEnchantedArrow _FireEnchantedArrow { get; set; }

        protected override void Awake()
        {
            base.Awake();
            _Dash = _Flit = new Flit(_PlayerData, _EntityPhysics, this, flitMaxDistance, flitDuration);
            _PrimaryAttack = _FireEnchantedArrow = new FireEnchantedArrow(_PlayerData, _EntityPhysics, this, castBarView);
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

      

        private void EnchantedArrowOnEntityHit(Entity hitEntity)
        {
            hitEntity.TakeDamage(enchantedArrowDamageMultiplier * _PlayerData._BaseAttackDamage);
            SetAutoAttackTarget(hitEntity);
        }
    }
}