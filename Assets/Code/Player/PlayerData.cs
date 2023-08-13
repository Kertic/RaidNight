using UnityEngine;

namespace Code.Player
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private float dashVector;

        [SerializeField]
        private float attackSpeed;



        public float _MoveSpeed
        {
            get => moveSpeed;
            private set => moveSpeed = value;
        }

        public float _JumpHeight
        {
            get => dashVector;
            private set => dashVector = value;
        }

        public float _BreakSpeed
        {
            get => attackSpeed;
            private set => attackSpeed = value;
        }


    }
}