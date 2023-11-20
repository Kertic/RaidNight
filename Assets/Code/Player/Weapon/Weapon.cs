using UnityEngine;
using UnityEngine.Pool;

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
            projectile.m_onHit = null;
        }

        private void OnDestroyProjectile(Projectile projectile)
        {
            Destroy(projectile);
        }

        #endregion

        public Projectile FireProjectile(Vector2 fireDirection, float projectileSpeed)
        {
            Projectile firedProjectile = _projectilePool.Get();
            firedProjectile.m_onHit += (_) => _projectilePool.Release(firedProjectile);
            firedProjectile.transform.position = transform.position;
            firedProjectile.FireProjectile(fireDirection, projectileSpeed);
            return firedProjectile;
        }
    }
}