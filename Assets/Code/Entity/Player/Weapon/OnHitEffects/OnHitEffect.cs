using System;

namespace Code.Entity.Player.Weapon.OnHitEffects
{
    public class OnHitEffect
    {
        public Action<Entity> OnApplicationEvent;
            
        public virtual void ApplyEffectToEntity(Entity hitEntity)
        {
            OnApplicationEvent?.Invoke(hitEntity);
        }
    }
}
