using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Entity
{
    public class TrainingDummy : Entity
    {
        [SerializeField]
        private TrainingDummyEntityView dummyEntityView;

        [SerializeField]
        private float cyclingTime;

        [SerializeField]
        private Vector2[] cycleLocations = new Vector2[2];


        private Vector2 _cycleStart, _cycleEnd;
        private int _currentCycleStartLocationIndex = 0;
        private float _remainingCycleTime;

        private void OnValidate()
        {
            if (cycleLocations.Length < 2)
            {
                Array.Resize(ref cycleLocations, 2);
            }

            _cycleStart = cycleLocations[0];
            _cycleEnd = cycleLocations[1];
            _remainingCycleTime = cyclingTime;
        }

        void Start()
        {
            m_view = dummyEntityView;
            _cycleStart = cycleLocations[0];
            _cycleEnd = cycleLocations[1];
            _remainingCycleTime = cyclingTime;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            _remainingCycleTime -= Time.fixedDeltaTime;

            if (_remainingCycleTime <= 0)
            {
                IncrementCycleIndex();
            }

            transform.position = Vector3.Lerp(_cycleStart, _cycleEnd, 1.0f - (_remainingCycleTime / cyclingTime));
        }

        private void IncrementCycleIndex()
        {
            _currentCycleStartLocationIndex = (_currentCycleStartLocationIndex + 1) % cycleLocations.Length;
            _cycleStart = cycleLocations[_currentCycleStartLocationIndex];
            _cycleEnd = cycleLocations[(_currentCycleStartLocationIndex + 1) % cycleLocations.Length];
            _remainingCycleTime = cyclingTime;
        }

        protected override void OnDeath()
        {
            m_view.SetHealthbarText("I have died.");
        }

        public override void TakeDamage(float damage, Color colorOfDamage)
        {
            if (m_currentHealth <= 0)
            {
                damage = -maxHealth;
                m_view.SetHealthbarText("");
            }

            base.TakeDamage(damage, colorOfDamage);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < cycleLocations.Length; i++)
            {
                int test = (i + 1) % cycleLocations.Length;
                Gizmos.DrawLine(cycleLocations[i], cycleLocations[(i + 1) % cycleLocations.Length]);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_cycleStart, 0.05f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_cycleEnd, 0.05f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_cycleEnd, _cycleStart);
        }
    }
}