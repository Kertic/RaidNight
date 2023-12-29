using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Entity.Player.Weapon
{
    public class TrackingProjectile : Projectile
    {
        public Action<Entity> m_onHitTarget;

        private Entity _trackedTarget;
        private float _firedSpeed;
        private Vector3 _startingPosition;

        private void Update()
        {
            Vector2 newTravelForce = (_trackedTarget.transform.position - transform.position).normalized * _firedSpeed;
            if (m_continuousForce != newTravelForce)
            {
                m_entityPhysics.RemoveAllContinuousForces();
                FireProjectile((_trackedTarget.transform.position - transform.position).normalized, _firedSpeed);
            }
        }

        public override void FireProjectile(Vector2 newTravelDirection, float newSpeed)
        {
            base.FireProjectile(newTravelDirection, newSpeed);
            _startingPosition = transform.position;
            _firedSpeed = newSpeed;
        }

        protected override void OnRaycastTriggersDetected(Dictionary<string, RaycastHit2D> collisions)
        {
            base.OnRaycastTriggersDetected(collisions);

            if (collisions["NonPlayerEntity"] && collisions["NonPlayerEntity"].transform.gameObject.GetComponent<Entity>() == GetTarget())
            {
                m_onHitTarget?.Invoke(_trackedTarget);
            }
        }

        public void SetTarget(Entity newTarget)
        {
            _trackedTarget = newTarget;
        }

        public Entity GetTarget()
        {
            return _trackedTarget;
        }

        public float GetDistanceToTarget()
        {
            return Vector3.Distance(transform.position, _trackedTarget.transform.position);
        }

        public Vector3 GetStartingPosition()
        {
            return _startingPosition;
        }
    }
}