using UnityEngine;

namespace Code.Entity.Player.Views.FaeArcher
{
    [RequireComponent(typeof(Animator))]
    public class WispView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer wispSprite;

        private Animator _animator;
        private static readonly int IsFlying = Animator.StringToHash("IsFlying");
        private static readonly int WiggleOffset = Animator.StringToHash("wiggleOffset");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() { }

        public void ChangeColor(Color color)
        {
            wispSprite.color = color;
        }

        public void SetWiggle(bool shouldWiggle, float animOffset = 0.0f)
        {
            _animator.SetBool(IsFlying, shouldWiggle);
            _animator.SetFloat(WiggleOffset, animOffset);
        }
        
        
        
    }
}