namespace Code.Systems
{
    public interface IPoolableObject
    {
        public void OnInstantiate();
        public void OnDestroy();
        public void OnSpawn();
        public void OnDespawn();
        public void GetPrefab();
        public void GetAvailable();
    }
}
