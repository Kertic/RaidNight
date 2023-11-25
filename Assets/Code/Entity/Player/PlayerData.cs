using UnityEngine;

namespace Code.Entity.Player
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed, attackSpeed, baseAttackDamage, luck;

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
        
        /*
         * 1.5f luck means a 50% chance of rerolling a failed roll. 2.25f means you always reroll once if you don't get it, and a 25% chance of rolling a 3rd time if both fail
         */
        public float _LuckChance 
        {
            get => luck;
            private set => luck = value;
        }
    }
}