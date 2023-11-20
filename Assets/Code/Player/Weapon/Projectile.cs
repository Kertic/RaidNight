using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Player.Weapon
{
    [RequireComponent(typeof(EntityPhysics))]
    public class Projectile : MonoBehaviour
    {
        public Action<RaycastHit2D[]> m_onHit;
        public Action<RaycastHit2D[]> m_onTrigger;

        private Weapon _firingWeapon;
        private EntityPhysics _entityPhysics;
        private Vector2 _continuousForce;

        private void Awake()
        {
            _entityPhysics = GetComponent<EntityPhysics>();
        }

        public void FireProjectile(Vector2 newTravelDirection, float newSpeed)
        {
            _continuousForce = newTravelDirection.normalized * newSpeed;
            _entityPhysics.AddContinuousForce(_continuousForce);
            float angle = Mathf.Atan2(newTravelDirection.x, newTravelDirection.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
        }

        private void OnRaycastCollisionsDetected(List<RaycastHit2D> collisions)
        {
            m_onHit?.Invoke(collisions.ToArray());
        }

        private void OnRaycastTriggersDetected(Dictionary<string, RaycastHit2D> collisions)
        {
            RaycastHit2D[] hits = new RaycastHit2D[collisions.Count];
            collisions.Values.CopyTo(hits, 0);
            m_onTrigger?.Invoke(hits);
        }

        private void OnEnable()
        {
            _entityPhysics.m_onRaycastCollisionsDetected += OnRaycastCollisionsDetected;
            _entityPhysics.m_onRaycastTriggersDetected += OnRaycastTriggersDetected;
        }

        private void OnDisable()
        {
            _entityPhysics.RemoveContinuousForce(_continuousForce);
            _continuousForce = Vector2.zero;
            _entityPhysics.m_onRaycastCollisionsDetected -= OnRaycastCollisionsDetected;
            _entityPhysics.m_onRaycastTriggersDetected -= OnRaycastTriggersDetected;
        }
    }
}