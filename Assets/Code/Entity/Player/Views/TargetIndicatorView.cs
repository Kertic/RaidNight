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
            SetVisible(false);
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
        }

        public void SetColor(Color newColor)
        {
            targetingReticle.color = newColor;
        }

        public void SetVisible(bool isVisible)
        {
            Color targetingReticleColor = targetingReticle.color;
            targetingReticleColor.a = isVisible ? 1.0f : 0.0f;
            SetColor(targetingReticleColor);
        }

        public void SetStaticLocation(Vector3 position)
        {
            transform.position = position;
            SetTarget(transform);
        }
    }
}