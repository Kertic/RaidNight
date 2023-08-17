using UnityEngine;

namespace Code.Player
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private float attackSpeed;

        public float _MoveSpeed
        {
            get => moveSpeed;
            private set => moveSpeed = value;
        }

        public float _AttackSpeed
        {
            get => attackSpeed;
            private set => attackSpeed = value;
        }
    }
}