using System;
using System.Collections.Generic;
using System.Linq;
using Code.Entity.Buffs;
using Code.Systems;
using Code.Systems.Views;
using UnityEngine;

namespace Code.Entity
{
    [RequireComponent(typeof(Collider2D))]
    public class Entity : MonoBehaviour
    {
        [SerializeField]
        protected int maxHealth;

        [SerializeField]
        private List<BuffViewGrid> buffViewGrids;

        protected float m_currentHealth;
        protected IEntityView m_view;

        private Dictionary<Buff, List<BuffView>> _activeBuffs;

        protected virtual void Awake()
        {
            m_currentHealth = maxHealth;
            _activeBuffs = new Dictionary<Buff, List<BuffView>>();
        }

        public virtual void TakeDamage(float damage)
        {
            m_currentHealth = Mathf.Clamp(m_currentHealth - (float)damage, 0.0f, (float)maxHealth);
            m_view.SetHealthPercent((float)m_currentHealth / maxHealth);
            m_view.SetHealthbarText(m_currentHealth.ToString("0.0"));

            if (m_currentHealth == 0.0f)
            {
                OnDeath();
            }

            GameMaster.m_instance.SpawnFloatingTextAtTransform(transform, damage.ToString(damage > 5 ? "0" : "0.0"), Color.white);
        }

        protected virtual void FixedUpdate()
        {
            foreach (Buff buff in _activeBuffs.Keys.ToList())
            {
                buff.OnBuffUpdate(_activeBuffs[buff].ToArray());
            }
        }

        public void AddBuff(Buff newBuffToAdd)
        {
            if (!_activeBuffs.ContainsKey(newBuffToAdd))
            {
                _activeBuffs[newBuffToAdd] = new List<BuffView>();
            }

            foreach (BuffViewGrid currentGrid in buffViewGrids)
            {
                _activeBuffs[newBuffToAdd].Add(currentGrid.GetBuffView());
                newBuffToAdd.OnBuffEnter(_activeBuffs[newBuffToAdd].ToArray());
            }

            newBuffToAdd.m_onBuffExpire += () => { RemoveBuff(newBuffToAdd); };
        }

        public virtual void RemoveBuff(Buff buffToRemove)
        {
            foreach (BuffViewGrid currentGrid in buffViewGrids)
            {
                foreach (BuffView buffView in _activeBuffs[buffToRemove])
                {
                    currentGrid.RemoveBuffView(buffView);
                }
            }

            _activeBuffs.Remove(buffToRemove);
        }

        protected virtual void OnDeath() { }
    }
}