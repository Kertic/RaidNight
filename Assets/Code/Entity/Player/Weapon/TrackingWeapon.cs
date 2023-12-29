using UnityEngine;

namespace Code.Entity.Player.Weapon
{
    public class TrackingWeapon : Weapon<TrackingProjectile>
    {
        public TrackingProjectile FireProjectile(Entity trackingTarget, float projectileSpeed)
        {
            Vector2 newTravelDirection = trackingTarget.transform.position - transform.position;
            TrackingProjectile projectile = base.FireProjectile(newTravelDirection, projectileSpeed);
            projectile.m_onHitTarget += (_) => m_projectilePool.Release(projectile);
            projectile.SetTarget(trackingTarget);
            return projectile;
        }

        protected override void OnReturnToPool(TrackingProjectile projectile)
        {
            base.OnReturnToPool(projectile);
            projectile.m_onHitTarget = null;
        }
    }
}