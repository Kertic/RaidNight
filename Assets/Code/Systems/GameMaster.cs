using System;
using Code.Systems.Views;
using UnityEngine;
using UnityEngine.Pool;

namespace Code.Systems
{
    public class GameMaster : MonoBehaviour
    {
        [SerializeField]
        private FloatingCombatTextView floatingCombatTextTemplate;

        private ObjectPool<FloatingCombatTextView> _floatingCombatTextPool;

        public static GameMaster m_instance;

        private void Awake()
        {
            if (m_instance == null)
                m_instance = this;
            _floatingCombatTextPool = new ObjectPool<FloatingCombatTextView>(SpawnText, OnGetFromPool, OnReturnToPool, OnDestroyCombatText);
        }

        #region Floating Combat Text Pool Functions

        private FloatingCombatTextView SpawnText()
        {
            return Instantiate(floatingCombatTextTemplate, transform);
        }

        private void OnGetFromPool(FloatingCombatTextView textView)
        {
            textView.gameObject.SetActive(true);
        }

        private void OnReturnToPool(FloatingCombatTextView textView)
        {
            textView.gameObject.SetActive(false);
            textView.OnAnimationEnd = null;
        }

        private void OnDestroyCombatText(FloatingCombatTextView textView)
        {
            Destroy(textView);
        }

        #endregion

        void Start() { }

        void Update() { }

        public void SpawnFloatingTextAtTransform(Transform transformToTrack, string textToShow, Color newColor)
        {
            FloatingCombatTextView text = _floatingCombatTextPool.Get();
            text.SetTrackedTransform(transformToTrack);
            text._Text.text = textToShow;
            text._Text.color = newColor;
            text.OnAnimationEnd += () =>
            {
                _floatingCombatTextPool.Release(text);
            };
        }
    }
}