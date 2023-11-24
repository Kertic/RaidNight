using System;
using TMPro;
using UnityEngine;

namespace Code.Systems.Views
{
    public class FloatingCombatTextView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro text;

        private Transform _transformToTrack;

        public TextMeshPro _Text => text;
        public Action OnAnimationEnd;

        private void Update()
        {
            transform.position = _transformToTrack.position;
        }

        public void SetColor(Color newColor)
        {
            _Text.color = newColor;
        }

        public void SetTrackedTransform(Transform newTransform)
        {
            _transformToTrack = newTransform;
        }

        public void CallOnAnimationEnd()
        {
            OnAnimationEnd?.Invoke();
        }
    }
}
