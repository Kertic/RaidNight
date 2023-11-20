using UnityEngine;

namespace Code.Entity.Player
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed, attackSpeed, baseAttackDamage;

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

        public float _BaseAttackDamage
        {
            get => baseAttackDamage;
            private set => baseAttackDamage = value;
        }
    }
}