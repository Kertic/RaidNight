using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

namespace Code.Entity.Player.Weapon
{
    public class TrackingProjectile : Projectile
    {
        private Transform _trackedTarget;
        private float _firedSpeed;
        private Vector3 _startingPosition;
        
        private void Update()
        {
            Vector2 newTravelForce = (_trackedTarget.position - transform.position).normalized * _firedSpeed;
            if (m_continuousForce != newTravelForce)
            {
                m_entityPhysics.RemoveAllContinuousForces();
                FireProjectile((_trackedTarget.position - transform.position).normalized, _firedSpeed);
            }
        }

        public override void FireProjectile(Vector2 newTravelDirection, float newSpeed)
        {
            base.FireProjectile(newTravelDirection, newSpeed);
            _startingPosition = transform.position;
            _firedSpeed = newSpeed;
        }

        public void SetTarget(Transform newTarget)
        {
            _trackedTarget = newTarget;
        }

        public float GetDistanceToTarget()
        {
            return Vector3.Distance(transform.position, _trackedTarget.position);
        }

        public Vector3 GetStartingPosition()
        {
            return _startingPosition;
        }
    }
}