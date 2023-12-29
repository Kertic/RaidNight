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
            hitEntity.TakeDamage(_damage, new Color(0.8f, 0.72f, 0.016f, 1.0f));
        }
    }
}