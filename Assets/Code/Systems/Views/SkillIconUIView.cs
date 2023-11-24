using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Systems.Views
{
    [RequireComponent(typeof(Animator))]
    public class SkillIconUIView : MonoBehaviour
    {
        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private TextMeshProUGUI cooldownText;

        [SerializeField]
        private RectTransform cooldownOverlay;

        private Animator _animator;
        private static readonly int CooldownPercent = Animator.StringToHash("cooldownPercent");


        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetProgress(float progressRatio, float timeRemaining)
        {
            _animator.SetFloat(CooldownPercent, progressRatio);
            cooldownText.text = timeRemaining != 0.0f ? timeRemaining.ToString("0.0") : "";// There should be a way to make this function with a connection to a skill
            
        }

        public void SetSprite(Sprite newSprite)
        {
            iconImage.sprite = newSprite;
        }
    }
}
