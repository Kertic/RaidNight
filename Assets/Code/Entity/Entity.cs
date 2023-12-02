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
        private BuffUIView buffUIView;

        protected float m_currentHealth;
        protected IEntityView m_view;

        private Dictionary<Buff, BuffView> _activeBuffs;

        protected virtual void Awake()
        {
            m_currentHealth = maxHealth;
            _activeBuffs = new Dictionary<Buff, BuffView>();
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
                buff.OnBuffUpdate(_activeBuffs[buff]);
            }
        }

        public void AddBuff(Buff newBuffToAdd)
        {
            _activeBuffs[newBuffToAdd] = buffUIView.GetBuffView();
            newBuffToAdd.OnBuffEnter(_activeBuffs[newBuffToAdd]);
            newBuffToAdd.m_onBuffExpire += () => { RemoveBuff(newBuffToAdd); };
        }

        public virtual void RemoveBuff(Buff buffToRemove)
        {
            buffUIView.RemoveBuffView(_activeBuffs[buffToRemove]);
            _activeBuffs.Remove(buffToRemove);
        }

        protected virtual void OnDeath() { }
    }
}