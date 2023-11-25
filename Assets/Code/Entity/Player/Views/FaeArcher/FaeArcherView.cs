using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Code.Entity.Player.Views.FaeArcher
{
    public class FaeArcherView : MonoBehaviour
    {
        [SerializeField]
        private Transform circleCenterTransform;

        [SerializeField]
        private WispView wispTemplate;

        [SerializeField]
        private float wispRadius, wispSpinSpeed = 1;

        private ObjectPool<WispView> _wispSprites;
        private List<WispView> _activeWisps;

        private void Awake()
        {
            _wispSprites = new ObjectPool<WispView>(SpawnWisp, OnGetFromPool, OnReturnToPool, OnDestroyWisp);
            _activeWisps = new List<WispView>();
        }

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update()
        {
            float rotationOffset = Time.time * wispSpinSpeed;
            for (int i = 0; i < _activeWisps.Count; i++)
            {
                /* Distance around the circle */
                float radians = 2 * MathF.PI / _activeWisps.Count * i;

                /* Get the vector direction */
                float vertical = MathF.Sin(radians + rotationOffset);
                float horizontal = MathF.Cos(radians + rotationOffset);

                Vector3 spawnDir = new(horizontal, vertical, 0);

                /* Get the spawn position */
                Vector3 spawnPos = circleCenterTransform.position + spawnDir * wispRadius; // Radius is just the distance away from the point

                _activeWisps[i].transform.position = spawnPos;
            }
        }

        public void SetWispCount(int wisps) { }

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

        #endregion
    }
}