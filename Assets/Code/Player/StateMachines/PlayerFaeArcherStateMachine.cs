using Code.Player.StateMachines.PlayerControlStates.PlayerFaeArcherStates;
using Code.Player.Weapon;
using UnityEngine;

namespace Code.Player.StateMachines
{
    [RequireComponent(typeof(PlayerData), typeof(EntityPhysics))]
    public class PlayerFaeArcherStateMachine : PlayerControlsStateMachine
    {
        [Header("Enchanted Arrow")]
        [SerializeField]
        private Weapon.Weapon enchantedArrowWeapon;

        [SerializeField]
        private float enchantedArrowProjectileSpeed, fireEnchantedArrowCastTime, enchantedArrowDamage;

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
            _Dash = _Flit = new Flit(PlayerData, EntityPhysics, this, flitMaxDistance, flitDuration);
            _PrimaryAttack = _FireEnchantedArrow = new FireEnchantedArrow(PlayerData, EntityPhysics, this, castBarView);
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
            Projectile enchantedArrow = enchantedArrowWeapon.FireProjectile(targetLocation - (Vector2)transform.position, enchantedArrowProjectileSpeed);
            enchantedArrow.m_onHit += hit2Ds =>
            {
                foreach (RaycastHit2D hit in hit2Ds)
                {
                    Entity.Entity entity = hit.collider.gameObject.GetComponent<Entity.Entity>();
                    if (entity != null)
                    {
                        EnchantedArrowOnEntityHit(entity);
                    }
                }
            };
        }

        private void EnchantedArrowOnEntityHit(Entity.Entity hitEntity)
        {
            hitEntity.TakeDamage(enchantedArrowDamage);
        }
    }
}