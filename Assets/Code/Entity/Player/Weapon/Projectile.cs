using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Entity.Player.Weapon
{
    [RequireComponent(typeof(EntityPhysics))]
    public class Projectile : MonoBehaviour
    {
        public Action<RaycastHit2D[]> m_onEntityHit;
        public Action<RaycastHit2D[]> m_onWallHit;
        public Action<RaycastHit2D[]> m_onTrigger;

        protected EntityPhysics m_entityPhysics;
        protected Vector2 m_continuousForce;

        private void Awake()
        {
            m_entityPhysics = GetComponent<EntityPhysics>();
        }

        public virtual void FireProjectile(Vector2 newTravelDirection, float newSpeed)
        {
            m_continuousForce = newTravelDirection.normalized * newSpeed;
            m_entityPhysics.AddContinuousForce(m_continuousForce);
            float angle = Mathf.Atan2(newTravelDirection.x, newTravelDirection.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
        }

        private void OnRaycastCollisionsDetected(List<RaycastHit2D> collisions)
        {
            RaycastHit2D[] entityHits = collisions.Where(col => col.transform.gameObject.CompareTag("NonPlayerEntity")).ToArray();
            RaycastHit2D[] wallHits = collisions.Where(col => col.transform.gameObject.CompareTag("Wall")).ToArray();


            if (entityHits.Length > 0)
            {
                m_onEntityHit?.Invoke(entityHits);
            }

            if (wallHits.Length > 0)
            {
                m_onWallHit?.Invoke(wallHits);
            }
        }

        private void OnRaycastTriggersDetected(Dictionary<string, RaycastHit2D> collisions)
        {
            RaycastHit2D[] hits = new RaycastHit2D[collisions.Count];
            collisions.Values.CopyTo(hits, 0);
            m_onTrigger?.Invoke(hits);
        }

        private void OnEnable()
        {
            m_entityPhysics.m_onRaycastCollisionsDetected += OnRaycastCollisionsDetected;
            m_entityPhysics.m_onRaycastTriggersDetected += OnRaycastTriggersDetected;
        }

        private void OnDisable()
        {
            m_entityPhysics.RemoveAllContinuousForces();
            m_continuousForce = Vector2.zero;
            m_entityPhysics.m_onRaycastCollisionsDetected -= OnRaycastCollisionsDetected;
            m_entityPhysics.m_onRaycastTriggersDetected -= OnRaycastTriggersDetected;
        }
    }
}