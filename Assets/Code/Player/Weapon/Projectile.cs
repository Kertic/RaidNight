using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Player.Weapon
{
    [RequireComponent(typeof(EntityPhysics))]
    public class Projectile : MonoBehaviour
    {
        public Action ONHit;

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

        private void OnRaycastDetected(List<RaycastHit2D> collisions)
        {
            ONHit?.Invoke();
        }

        private void OnEnable()
        {
            _entityPhysics.m_onRaycastCollisionsDetected += OnRaycastDetected;
        }

        private void OnDisable()
        {
            _entityPhysics.RemoveContinuousForce(_continuousForce);
            _continuousForce = Vector2.zero;
            _entityPhysics.m_onRaycastCollisionsDetected -= OnRaycastDetected;
        }
    }
}