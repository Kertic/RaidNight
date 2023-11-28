using System.Collections.Generic;
using Code.Entity.Player.Weapon;
using Code.Systems;
using Unity.Mathematics;
using Unity.VisualScripting;
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
        private WispView wispTemplate;

        [SerializeField]
        private float wispRadius, wispSpinSpeed;

        private ObjectPool<WispView> _wispSprites;
        private List<WispView> _activeWisps;
        private TrackingProjectile _attachedProjectile;

        private void Awake()
        {
            _wispSprites = new ObjectPool<WispView>(SpawnWisp, OnGetFromPool, OnReturnToPool, OnDestroyWisp);
            _activeWisps = new List<WispView>();
            _attachedProjectile = null;
        }

        void Update()
        {
            float rotationOffset = Time.time * wispSpinSpeed;

            for (int i = 0; i < _activeWisps.Count; i++)
            {
                Vector3 newPos = Utils.Vector3.GetPositionInRotatingCircle(
                    i,
                    _activeWisps.Count,
                    rotationOffset * wispSpinSpeed,
                    wispRadius,
                    _attachedProjectile == null ? circleCenterTransform.position : _attachedProjectile.GetStartingPosition());


                if (_attachedProjectile != null)
                {
                    _activeWisps[i].transform.position = Vector3.Lerp(newPos, _attachedProjectile.transform.position,
                        1.0f - math.tanh(_attachedProjectile.GetDistanceToTarget() / 5.0f)
                    );
                    _activeWisps[i].transform.rotation = _attachedProjectile.transform.rotation;
                }
                else
                {
                    _activeWisps[i].transform.position = newPos;
                    _activeWisps[i].transform.rotation = Quaternion.Euler(
                        0,
                        0,
                        Utils.Vector2.GetRotationOfObjectOnCircle(newPos, circleCenterTransform.position) - (wispSpinSpeed < 0.0f ? 180.0f : 0.0f));
                }
            }
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
            _wispSprites.Get();
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
            _activeWisps.Add(wispView);
            wispView.gameObject.SetActive(true);
        }

        private void OnReturnToPool(WispView wispView)
        {
            _activeWisps.Remove(wispView);
            wispView.gameObject.SetActive(false);
        }

        private void OnDestroyWisp(WispView wispView)
        {
            Destroy(wispView);
        }

        private void SetActiveWispWiggles(bool shouldWiggle)
        {
            for (int i = 0; i < _activeWisps.Count; i++)
            {
                _activeWisps[i].SetWiggle(shouldWiggle,
                    shouldWiggle ? Random.Range(0.0f, 1.0f) : 0.0f);
            }
        }

        #endregion

        public void AttachWispsToProjectile(TrackingProjectile projectileToFollow)
        {
            projectileToFollow.m_onEntityHit += hit2Ds =>
            {
                _attachedProjectile = null;
                SetActiveWispWiggles(false); // TODO: Make this happen when he slowly drifts back to you
            };

            _attachedProjectile = projectileToFollow;
        }
    }
}