using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Entity.Player.Views
{
    public class TargetIndicatorView : MonoBehaviour
    {
        [SerializeField]
        private Image targetingReticle;
        private Transform _trackedTransform;

        private void Awake()
        {
            targetingReticle.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        }

        void Start()
        {
            _trackedTransform = transform;
        }

        void Update()
        {
            transform.position = _trackedTransform.position;
        }

        public void SetTarget(Transform target)
        {
            _trackedTransform = target;
            targetingReticle.color = Color.white;
        }

        public void SetStaticLocation(Vector3 position)
        {
            transform.position = position;
            SetTarget(transform);
        }
    }
}
