using System;
using Code.Systems;
using UnityEngine;

namespace Code.Entity
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField]
        protected int maxHealth;
        
        protected int m_currentHealth;
        protected IEntityView m_view;

        protected virtual void Awake()
        {
            m_currentHealth = maxHealth;
        }

        public virtual void TakeDamage(float damage)
        {
            m_currentHealth = Math.Clamp(m_currentHealth - (int)damage, 0, maxHealth);
            m_view.SetHealthPercent((float)m_currentHealth / maxHealth);
            
            if (m_currentHealth == 0.0f)
            {
                OnDeath();
            }
            
            GameMaster.m_instance.SpawnFloatingTextAtTransform(transform, damage.ToString("0"), Color.white);
        }

        protected abstract void OnDeath();
    }
}
