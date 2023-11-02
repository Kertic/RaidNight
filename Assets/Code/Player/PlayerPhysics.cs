using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(PlayerData))]
    public class PlayerPhysics : MonoBehaviour
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

        public BoxCollider2D _HitboxCollider2D { get; private set; }
        public Rigidbody2D _Rigidbody2D { get; private set; }
        public ContactFilter2D _contactFilter2D;
        public float collisionBuffer;

        private PlayerData m_playerData;

        private Vector2 burstVelocity;
        private List<Vector2> continuousForces;
        private List<KeyValuePair<UnityAction, BurstForce>> burstForces;

        private List<RaycastHit2D> _raycastHits = new();

        private List<KeyValuePair<Vector2, Vector2>> hitLocations = new();

        private void Awake()
        {
            _HitboxCollider2D = GetComponent<BoxCollider2D>();
            _Rigidbody2D = GetComponent<Rigidbody2D>();
            m_playerData = GetComponent<PlayerData>();
            continuousForces = new List<Vector2>();
            burstForces = new List<KeyValuePair<UnityAction, BurstForce>>();
        }

        private void FixedUpdate()
        {
            Vector2 movementVector = Vector2.zero;
            if (burstForces.Count != 0)
            {
                Vector2 burstMovement = Vector2.zero;
                for (int index = burstForces.Count - 1; index >= 0; index--)
                {
                    float stepSize = Time.fixedDeltaTime / burstForces[index].Value.m_duration;
                    Vector2 movement = Vector2.MoveTowards(Vector2.zero, burstForces[index].Value.m_movementVector, stepSize);
                    burstMovement += movement;
                    burstForces[index] = new KeyValuePair<UnityAction, BurstForce>(
                        burstForces[index].Key,
                        new BurstForce(
                            burstForces[index].Value.m_movementVector - movement,
                            burstForces[index].Value.m_duration
                        )
                    );

                    if (burstForces[index].Value.m_movementVector.magnitude <= float.Epsilon)
                    {
                        burstForces[index].Key();
                        burstForces.RemoveAt(index);
                    }
                }

                movementVector = burstMovement;
            }
            else
            {
                Vector2 continuousMovement = Vector2.zero;
                foreach (Vector2 continuousForce in continuousForces)
                {
                    continuousMovement += continuousForce;
                }

                movementVector = continuousMovement * Time.fixedDeltaTime;
            }


            hitLocations.Clear();
            if (movementVector == Vector2.zero || MovePlayer(movementVector)) return;
            if (MovePlayer(movementVector * Vector2.right)) return;
            if (MovePlayer(movementVector * Vector2.up)) return;
            Debug.Log("Collision detected, not moving");
        }

        private bool MovePlayer(Vector2 movementVector)
        {
            int collisions = _Rigidbody2D.Cast(movementVector.normalized, _contactFilter2D, _raycastHits, (movementVector).magnitude);
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


        public void AddContinuousForce(Vector2 continuousForce)
        {
            continuousForces.Add(continuousForce);
        }

        public void RemoveContinuousForce(Vector2 continuousForce)
        {
            continuousForces.Remove(continuousForce);
        }

        public void AddBurstForce(BurstForce burstForce, UnityAction onComplete)
        {
            burstForces.Add(new KeyValuePair<UnityAction, BurstForce>(onComplete, burstForce));
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