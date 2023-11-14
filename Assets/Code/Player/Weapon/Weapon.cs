using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Code.Player.Weapon
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Projectile projectilePrefab;
        private ObjectPool<Projectile> _projectilePool;

        private void Start()
        {
            _projectilePool = new ObjectPool<Projectile>(SpawnProjectile, OnGetFromPool, OnReturnToPool,
                OnDestroyProjectile, true, 10, 500);
        }

        #region Projectile Pool Functions

        private Projectile SpawnProjectile()
        {
            return Instantiate(projectilePrefab, transform);
        }

        private void OnGetFromPool(Projectile projectile)
        {
            projectile.gameObject.SetActive(true);
        }

        private void OnReturnToPool(Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
            projectile.ONHit = null;
        }

        private void OnDestroyProjectile(Projectile projectile)
        {
            Destroy(projectile);
        }

        #endregion

        public void FireProjectile(Vector2 fireDirection, float projectileSpeed)
        {
            Projectile projectile = _projectilePool.Get();
            projectile.ONHit += () => _projectilePool.Release(projectile);
            projectile.FireProjectile(fireDirection, projectileSpeed);
        }
    }
}