using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Entity.Player
{
    public class PlayerData : MonoBehaviour
    {
        public delegate float ModifyStat(float currentStat);

        public enum StatType
        {
            MOVESPEED,
            ATTSPEED,
            BASEATTDMG,
            LUCK,
            NUMOFSTATTYPES
        }

        public struct StatModifier
        {
            public ModifyStat m_modifier;
            public int m_priority;

            public StatModifier(ModifyStat modifier, int priority)
            {
                m_modifier = modifier;
                m_priority = priority;
            }
        }


        [SerializeField]
        private float moveSpeed, attackSpeed, baseAttackDamage, luck;

        private List<StatModifier> moveSpeedModifiers = new();
        private List<StatModifier> attackSpeedModifiers = new();
        private List<StatModifier> baseAttackDamageModifiers = new();
        private List<StatModifier> luckModifiers = new();

        public float _MoveSpeed
        {
            get
            {
                float finalValue = moveSpeed;
                for (int i = 0; i < moveSpeedModifiers.Count; i++)
                {
                    finalValue = moveSpeedModifiers[i].m_modifier(finalValue);
                }

                return finalValue;
            }
            private set => moveSpeed = value;
        }

        public float _AttackSpeed
        {
            get
            {
                float finalValue = attackSpeed;
                for (int i = 0; i < attackSpeedModifiers.Count; i++)
                {
                    finalValue = attackSpeedModifiers[i].m_modifier(finalValue);
                }

                return finalValue;
            }
            private set => attackSpeed = value;
        }

        public float _BaseAttackDamage
        {
            get
            {
                float finalValue = baseAttackDamage;
                for (int i = 0; i < baseAttackDamageModifiers.Count; i++)
                {
                    finalValue = baseAttackDamageModifiers[i].m_modifier(finalValue);
                }

                return finalValue;
            }
            private set => baseAttackDamage = value;
        }

        /*
         * 1.5f luck means a 50% chance of rerolling a failed roll. 2.25f means you always reroll once if you don't get it, and a 25% chance of rolling a 3rd time if both fail
         */
        public float _LuckChance
        {
            get
            {
                float finalValue = luck;
                for (int i = 0; i < luckModifiers.Count; i++)
                {
                    finalValue = luckModifiers[i].m_modifier(finalValue);
                }

                return finalValue;
            }
            private set => luck = value;
        }

        public void AddModifier(StatModifier newMod, StatType type)
        {
            switch (type)
            {
                case StatType.MOVESPEED:
                    moveSpeedModifiers.Add(newMod);
                    break;
                case StatType.ATTSPEED:
                    attackSpeedModifiers.Add(newMod);
                    break;
                case StatType.BASEATTDMG:
                    baseAttackDamageModifiers.Add(newMod);
                    break;
                case StatType.LUCK:
                    luckModifiers.Add(newMod);
                    break;
            }

            moveSpeedModifiers.Sort((modifier1, modifier2) => modifier1.m_priority.CompareTo(modifier2.m_priority));
            attackSpeedModifiers.Sort((modifier1, modifier2) => modifier1.m_priority.CompareTo(modifier2.m_priority));
            baseAttackDamageModifiers.Sort((modifier1, modifier2) => modifier1.m_priority.CompareTo(modifier2.m_priority));
            luckModifiers.Sort((modifier1, modifier2) => modifier1.m_priority.CompareTo(modifier2.m_priority));
        }

        public void RemoveModifier(StatModifier modToRemove, StatType type)
        {
            switch (type)
            {
                case StatType.MOVESPEED:
                    moveSpeedModifiers.Remove(modToRemove);
                    break;
                case StatType.ATTSPEED:
                    attackSpeedModifiers.Remove(modToRemove);
                    break;
                case StatType.BASEATTDMG:
                    baseAttackDamageModifiers.Remove(modToRemove);
                    break;
                case StatType.LUCK:
                    luckModifiers.Remove(modToRemove);
                    break;
            }

            moveSpeedModifiers.Sort((modifier1, modifier2) => modifier1.m_priority.CompareTo(modifier2.m_priority));
            attackSpeedModifiers.Sort((modifier1, modifier2) => modifier1.m_priority.CompareTo(modifier2.m_priority));
            baseAttackDamageModifiers.Sort((modifier1, modifier2) => modifier1.m_priority.CompareTo(modifier2.m_priority));
            luckModifiers.Sort((modifier1, modifier2) => modifier1.m_priority.CompareTo(modifier2.m_priority));
        }
    }
}