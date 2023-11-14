using System;
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

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                Debug.Log("Collided with: " + other.gameObject.name);
                ONHit();
            }
        }

        public void FireProjectile(Vector2 newTravelDirection, float newSpeed)
        {
            _continuousForce = newTravelDirection.normalized * newSpeed;
            _entityPhysics.AddContinuousForce(_continuousForce);
            float angle = Mathf.Atan2(newTravelDirection.x, newTravelDirection.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public void OnDisable()
        {
            _continuousForce = Vector2.zero;
        }
    }
}