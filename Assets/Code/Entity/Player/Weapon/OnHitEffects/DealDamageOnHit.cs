using UnityEditor.Tilemaps;
using UnityEngine;

namespace Code.Entity.Player.Weapon.OnHitEffects
{
    public class DealDamageOnHit : OnHitEffect
    {
        private float _damage;

        public DealDamageOnHit(float damageToDeal)
        {
            _damage = damageToDeal;
        }

        public override void ApplyEffectToEntity(Entity hitEntity)
        {
            base.ApplyEffectToEntity(hitEntity);
            hitEntity.TakeDamage(_damage);
        }
    }
}