using UnityEngine;

namespace Code.Player
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private float attackSpeed;

        [SerializeField]
        private float dashDuration;

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

        public float _DashDuration
        {
            get => dashDuration;
            private set => dashDuration = value;
        }
    }
}