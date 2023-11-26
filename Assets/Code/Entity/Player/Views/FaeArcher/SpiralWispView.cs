using System;
using System.Collections.Generic;
using Code.Entity.Player.Weapon;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Code.Entity.Player.Views.FaeArcher
{
    public class SpiralWispView : MonoBehaviour
    {
        [SerializeField]
        private Transform circleCenterTransform;

        [SerializeField]
        private WispView wispTemplate, mischiefTemplate;

        [SerializeField]
        private float mischiefRadius, mischiefSpinSpeed, wispRadius, wispSpinSpeedMultiplier;

        private ObjectPool<WispView> _wispSprites;
        private List<WispView> _activeWisps;
        private WispView _mischiefWispView;

        private bool _isMischiefPresentOnCharacter;
        private bool _areWispsAttacking;


        private void Awake()
        {
            _wispSprites = new ObjectPool<WispView>(SpawnWisp, OnGetFromPool, OnReturnToPool, OnDestroyWisp);
            _activeWisps = new List<WispView>();
            _isMischiefPresentOnCharacter = true;
            _areWispsAttacking = false;
            _mischiefWispView = Instantiate(mischiefTemplate, transform);
        }

        void Update()
        {
            float rotationOffset = Time.time * mischiefSpinSpeed;
            if (_areWispsAttacking)
            {
                rotationOffset *= 5;
            }

            Vector3 newMischiefPos = GetPositionInRotatingCircle(1, 1, rotationOffset, mischiefRadius, circleCenterTransform.position);
            _mischiefWispView.transform.position = newMischiefPos;
            _mischiefWispView.transform.rotation = Quaternion.Euler(0, 0, GetRotationOfObjectOnCircle(newMischiefPos, circleCenterTransform.position));

            for (int i = 0; i < _activeWisps.Count; i++)
            {
                Vector3 centerPos = _isMischiefPresentOnCharacter ? _mischiefWispView.transform.position : circleCenterTransform.position;
                Vector3 newPos = GetPositionInRotatingCircle(
                    i,
                    _activeWisps.Count,
                    _isMischiefPresentOnCharacter ? rotationOffset * wispSpinSpeedMultiplier : rotationOffset,
                    _isMischiefPresentOnCharacter ? wispRadius : mischiefRadius,
                    centerPos);

                _activeWisps[i].transform.position = newPos;
                _activeWisps[i].transform.rotation = !_areWispsAttacking
                    ? Quaternion.Euler(
                        0,
                        0,
                        GetRotationOfObjectOnCircle(newPos, centerPos) - (wispSpinSpeedMultiplier < 0.0f && _isMischiefPresentOnCharacter ? 180.0f : 0.0f))
                    : circleCenterTransform.rotation;
            }
        }

        private float GetRotationOfObjectOnCircle(Vector2 positionOfObject, Vector2 positionOfCircleCenter)
        {
            float angle = positionOfObject.y >= positionOfCircleCenter.y
                ? Vector2.Angle(Vector2.right, (positionOfObject - positionOfCircleCenter).normalized)
                : 180.0f - Vector2.Angle(Vector2.right, (positionOfObject - positionOfCircleCenter).normalized) + 180.0f;
            return angle;
        }

        private Vector3 GetPositionInRotatingCircle(int index, int totalInCircle, float rotationOffset, float radius, Vector3 center)
        {
            /* Distance around the circle */
            float radians = 2 * MathF.PI / totalInCircle * index;

            /* Get the vector direction */
            float vertical = MathF.Sin(radians + rotationOffset);
            float horizontal = MathF.Cos(radians + rotationOffset);

            Vector3 spawnDir = new(horizontal, vertical, 0);

            /* Get the spawn position */
            Vector3 spawnPos = center + spawnDir * radius; // Radius is just the distance away from the point
            return spawnPos;
        }

        public void SetWispCount(int newWispCount)
        {
            if (_activeWisps.Count < newWispCount)
            {
                while (_activeWisps.Count < newWispCount)
                {
                    AddWisp();
                }
            }
            else if (_activeWisps.Count > newWispCount)
            {
                while (_activeWisps.Count > newWispCount)
                {
                    RemoveWisp();
                }
            }
        }

        public void AddWisp()
        {
            WispView newView = _wispSprites.Get();
        }

        public void RemoveWisp()
        {
            _wispSprites.Release(_activeWisps[^1]);
        }

        #region Wisp Pool Functions

        private WispView SpawnWisp()
        {
            return Instantiate(wispTemplate, transform);
        }

        private void OnGetFromPool(WispView wispView)
        {
            wispView.gameObject.SetActive(true);
            _activeWisps.Add(wispView);
        }

        private void OnReturnToPool(WispView wispView)
        {
            wispView.gameObject.SetActive(false);
            _activeWisps.Remove(wispView);
        }

        private void OnDestroyWisp(WispView wispView)
        {
            Destroy(wispView);
        }

        private void SetActiveWispWiggles(bool shouldWiggle)
        {
            for (int i = 0; i < _activeWisps.Count; i++)
            {
                _activeWisps[i].SetWiggle(shouldWiggle, Random.Range(0.0f, 1.0f));
            }
        }

        #endregion

        public void SendMischiefAway()
        {
            _isMischiefPresentOnCharacter = false;
            _mischiefWispView.gameObject.SetActive(false);
            SetActiveWispWiggles(true);
        }

        public void BringMischiefBack()
        {
            _isMischiefPresentOnCharacter = true;
            _mischiefWispView.gameObject.SetActive(true);
            SetActiveWispWiggles(false);
        }

        public void LaunchWispsToTarget(TrackingProjectile projectileToFollow)
        {
            Transform cachedTransform = circleCenterTransform;
            projectileToFollow.m_onHit += hit2Ds =>
            {
                circleCenterTransform = cachedTransform;
                BringMischiefBack(); // TODO: Make this happen when he slowly drifts back to you
                _areWispsAttacking = false; // TODO: This shouldn't even be in this view. There should be another view on the projectile that has wisps follow it.
            };

            SendMischiefAway();
            _areWispsAttacking = true;
            circleCenterTransform = projectileToFollow.transform;
        }
    }
}