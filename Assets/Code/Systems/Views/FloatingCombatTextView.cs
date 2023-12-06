using System;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Systems.Views
{
    public class FloatingCombatTextView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro text;

        private Transform _transformToTrack;

        public TextMeshPro _Text => text;
        public Action m_onAnimationEnd;
        private float _finalHorizontalEndingOffset = 0.0f;
        private float _startTime = 0.0f;

        private void Update()
        {
            float modO = (Time.time - _startTime) % 0.7f / (_startTime % 0.7f + 0.7f);

            transform.position = _transformToTrack.position + new Vector3(
                Mathf.Lerp(
                    0.0f,
                    _finalHorizontalEndingOffset,
                    math.tanh(modO)),
                0.0f,
                0.0f
            );
        }

        public void SetColor(Color newColor)
        {
            _Text.color = newColor;
        }

        public void SetTrackedTransform(Transform newTransform)
        {
            _transformToTrack = newTransform;
            transform.position = _transformToTrack.position;
        }

        public void CallOnAnimationEnd()
        {
            m_onAnimationEnd?.Invoke();
        }

        public void SetHorizontalOffset(float newOffset)
        {
            _startTime = Time.time;
            _finalHorizontalEndingOffset = newOffset;
        }
    }
}