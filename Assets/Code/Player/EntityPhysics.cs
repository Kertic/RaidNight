using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class EntityPhysics : MonoBehaviour
    {
        public struct BurstForce
        {
            public Vector2 m_movementVector;
            public float m_duration;

            public BurstForce(Vector2 movement, float duration)
            {
                m_movementVector = movement;
                m_duration = duration;
            }
        }

        public Action<List<KeyValuePair<Vector2, Vector2>>> OnRaycastCollisionsDetected;
        
        private Collider2D Collider2D { get; set; }
        private Rigidbody2D Rigidbody2D { get; set; }
        public ContactFilter2D _contactFilter2D;
        public float collisionBuffer;
        
        private Vector2 _burstVelocity;
        private List<Vector2> _continuousForces;
        private List<KeyValuePair<UnityAction, BurstForce>> _burstForces;
        private List<RaycastHit2D> _raycastHits = new();
        private List<KeyValuePair<Vector2, Vector2>> hitLocations = new();

        private void Awake()
        {
            Collider2D = GetComponent<Collider2D>();
            Rigidbody2D = GetComponent<Rigidbody2D>();
            _continuousForces = new List<Vector2>();
            _burstForces = new List<KeyValuePair<UnityAction, BurstForce>>();
        }

        private void FixedUpdate()
        {
            Vector2 movementVector;
            if (_burstForces.Count != 0)
            {
                Vector2 burstMovement = Vector2.zero;
                for (int index = _burstForces.Count - 1; index >= 0; index--)
                {
                    float stepSize = Time.fixedDeltaTime / _burstForces[index].Value.m_duration;
                    Vector2 movement = Vector2.MoveTowards(Vector2.zero, _burstForces[index].Value.m_movementVector,
                        stepSize);
                    burstMovement += movement;
                    _burstForces[index] = new KeyValuePair<UnityAction, BurstForce>(
                        _burstForces[index].Key,
                        new BurstForce(
                            _burstForces[index].Value.m_movementVector - movement,
                            _burstForces[index].Value.m_duration
                        )
                    );

                    if (_burstForces[index].Value.m_movementVector.magnitude <= float.Epsilon)
                    {
                        _burstForces[index].Key();
                        _burstForces.RemoveAt(index);
                    }
                }

                movementVector = burstMovement;
            }
            else
            {
                Vector2 continuousMovement = Vector2.zero;
                foreach (Vector2 continuousForce in _continuousForces)
                {
                    continuousMovement += continuousForce;
                }

                movementVector = continuousMovement * Time.fixedDeltaTime;
            }


            hitLocations.Clear();
            if (movementVector == Vector2.zero || MoveEntity(movementVector)) return;
            if (MoveEntity(movementVector * Vector2.right)) return;
            if (MoveEntity(movementVector * Vector2.up)) return;


            OnRaycastCollisionsDetected(hitLocations);
            Debug.Log("Collision detected, not moving (" + Rigidbody2D.gameObject.name + ")");
        }

        private bool MoveEntity(Vector2 movementVector)
        {
            int collisions = Rigidbody2D.Cast(movementVector.normalized, _contactFilter2D, _raycastHits,
                (movementVector).magnitude);
            if (collisions == 0)
            {
                Rigidbody2D.MovePosition(Rigidbody2D.position + movementVector);
                return true;
            }

            if (hitLocations.Count == 0)
            {
                foreach (RaycastHit2D hit2D in _raycastHits)
                {
                    hitLocations.Add(new KeyValuePair<Vector2, Vector2>(Rigidbody2D.ClosestPoint(hit2D.point),
                        hit2D.point));
                }
            }

            return false;
        }
        public void AddContinuousForce(Vector2 continuousForce)
        {
            _continuousForces.Add(continuousForce);
        }
        public void RemoveContinuousForce(Vector2 continuousForce)
        {
            _continuousForces.Remove(continuousForce);
        }
        public void AddBurstForce(BurstForce burstForce, UnityAction onComplete)
        {
            _burstForces.Add(new KeyValuePair<UnityAction, BurstForce>(onComplete, burstForce));
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