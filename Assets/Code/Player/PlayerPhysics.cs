using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Code.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(PlayerData))]
    public class PlayerPhysics : MonoBehaviour
    {
        public BoxCollider2D _HitboxCollider2D { get; private set; }
        public Rigidbody2D _Rigidbody2D { get; private set; }
        public ContactFilter2D _contactFilter2D;
        public float collisionBuffer;

        private PlayerData m_playerData;

        private Vector2 continuousVelocity;
        private Vector2 burstVelocity;
        private List<Vector2> continuousForces;

        private List<RaycastHit2D> _raycastHits = new();

        private List<KeyValuePair<Vector2, Vector2>> hitLocations = new();

        private void Awake()
        {
            _HitboxCollider2D = GetComponent<BoxCollider2D>();
            _Rigidbody2D = GetComponent<Rigidbody2D>();
            m_playerData = GetComponent<PlayerData>();
            continuousVelocity = Vector2.zero;
            continuousForces = new List<Vector2>();
        }

        private bool MovePlayer(Vector2 movementVector)
        {
            int collisions = _Rigidbody2D.Cast(movementVector, _contactFilter2D, _raycastHits, Time.fixedDeltaTime + collisionBuffer);
            if (collisions == 0)
            {
                _Rigidbody2D.MovePosition(_Rigidbody2D.position + movementVector);
                return true;
            }

            if (hitLocations.Count == 0)
            {
                foreach (RaycastHit2D hit2D in _raycastHits)
                {
                    hitLocations.Add(new KeyValuePair<Vector2, Vector2>(_Rigidbody2D.ClosestPoint(hit2D.point), hit2D.point));
                }
            }

            return false;
        }

        private void FixedUpdate()
        {
            continuousVelocity = Vector2.zero;
            foreach (Vector2 continuousForce in continuousForces)
            {
                continuousVelocity += continuousForce;
            }

            Vector2 movementVector = continuousVelocity * Time.fixedDeltaTime;

            hitLocations.Clear();
            if (continuousVelocity != Vector2.zero &&
                !MovePlayer(movementVector))
            {
                if (!MovePlayer(movementVector * Vector2.right))
                {
                    if (!MovePlayer(movementVector * Vector2.up))
                    {
                        Debug.Log("Collision detected, not moving");
                    }
                }
            }
        }

        public void AddContinuousForce(Vector2 continuousForce)
        {
            continuousForces.Add(continuousForce);
        }

        public void RemoveContinuousForce(Vector2 continuousForce)
        {
            continuousForces.Remove(continuousForce);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            foreach (KeyValuePair<Vector2, Vector2> hitLocation in hitLocations)
            {
                Gizmos.DrawLine(hitLocation.Key, hitLocation.Value);
            }
        }
    }
}