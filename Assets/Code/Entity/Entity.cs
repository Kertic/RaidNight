using System;
using UnityEngine;

namespace Code.Entity
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField]
        protected int maxHealth;
        
        protected int currentHealth;
        protected IEntityView _view;

        protected virtual void Awake()
        {
            currentHealth = maxHealth;
        }

        public virtual void TakeDamage(float damage)
        {
            currentHealth = Math.Clamp(currentHealth - (int)damage, 0, maxHealth);
            _view.SetHealthPercent((float)currentHealth / maxHealth);
            
            if (currentHealth == 0.0f)
            {
                OnDeath();
            }
        }

        protected abstract void OnDeath();
    }
}
