using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Player.Weapon
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {

        private float _speed;
        private Vector2 _travelDirection;
        private Rigidbody2D _rigidbody2D;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + (_travelDirection.normalized * _speed));
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("Collided with: " + other.gameObject.name);
        }

        public void FireProjectile(Vector2 newTravelDirection, float newSpeed)
        {
            _travelDirection = newTravelDirection.normalized;
            _speed = newSpeed;
        }
    }
}
