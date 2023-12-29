using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Code.Entity.Player.Weapon
{
    public abstract class Weapon<TProjectile> : MonoBehaviour, IWeapon<TProjectile> where TProjectile : Projectile
    {
        [SerializeField]
        protected TProjectile projectilePrefab;

        protected ObjectPool<TProjectile> m_projectilePool;

        private void Awake()
        {
            m_projectilePool = new ObjectPool<TProjectile>(SpawnProjectile, OnGetFromPool, OnReturnToPool,
                OnDestroyProjectile, true, 10, 500);
        }


        #region Projectile Pool Functions

        private TProjectile SpawnProjectile()
        {
            return Instantiate(projectilePrefab, transform);
        }

        private void OnGetFromPool(TProjectile projectile)
        {
            projectile.gameObject.SetActive(true);
            projectile.transform.position = transform.position;
        }

        protected virtual void OnReturnToPool(TProjectile projectile)
        {
            projectile.gameObject.SetActive(false);
            projectile.m_onEntityHit = null;
            projectile.m_onWallHit = null;
            projectile.m_onTrigger = null;
            projectile.transform.position = transform.position;
        }

        private void OnDestroyProjectile(TProjectile projectile)
        {
            Destroy(projectile);
        }

        #endregion

        public virtual TProjectile FireProjectile(Vector2 fireDirection, float projectileSpeed)
        {
            TProjectile firedProjectile = m_projectilePool.Get();
            firedProjectile.m_onEntityHit += (_) => m_projectilePool.Release(firedProjectile);
            firedProjectile.m_onWallHit += (_) => m_projectilePool.Release(firedProjectile);
            firedProjectile.transform.position = transform.position;
            firedProjectile.FireProjectile(fireDirection, projectileSpeed);
            return firedProjectile;
        }
    }
}