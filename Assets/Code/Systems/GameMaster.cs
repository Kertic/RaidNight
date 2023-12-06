using System;
using Code.Systems.Views;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Code.Systems
{
    public class GameMaster : MonoBehaviour
    {
        [SerializeField]
        private FloatingCombatTextView floatingCombatTextTemplate;

        [SerializeField]
        private float combatTextVariance, frequencyOfSpawnLimit;

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
            FloatingCombatTextView obj = Instantiate(floatingCombatTextTemplate, transform);
            obj.gameObject.SetActive(false);
            return obj;
        }

        private void OnGetFromPool(FloatingCombatTextView textView) { }

        private void OnReturnToPool(FloatingCombatTextView textView)
        {
            textView.gameObject.SetActive(false);
            textView.m_onAnimationEnd = null;
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
            this.Invoke(() => FireFloatingText(transformToTrack, textToShow, newColor, text), frequencyOfSpawnLimit * (_floatingCombatTextPool.CountActive - 1));
        }

        private void FireFloatingText(Transform transformToTrack, string textToShow, Color newColor, FloatingCombatTextView text)
        {
            text.SetHorizontalOffset(Random.Range(-combatTextVariance, combatTextVariance));
            text.SetTrackedTransform(transformToTrack);
            text._Text.text = textToShow;
            text.SetColor(newColor);
            text.m_onAnimationEnd += () => { _floatingCombatTextPool.Release(text); };
            text.gameObject.SetActive(true);
        }
    }
}