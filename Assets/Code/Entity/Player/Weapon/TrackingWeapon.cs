using UnityEngine;

namespace Code.Entity.Player.Weapon
{
    public class TrackingWeapon : Weapon<TrackingProjectile>
    {
        public TrackingProjectile FireProjectile(Transform trackingTarget, float projectileSpeed)
        {
            Vector2 newTravelDirection = trackingTarget.position - transform.position;
            TrackingProjectile projectile = base.FireProjectile(newTravelDirection, projectileSpeed);
            projectile.SetTarget(trackingTarget);
            return projectile;
        }
    }
}