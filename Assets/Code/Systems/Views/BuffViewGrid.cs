using System.Collections.Generic;
using Code.Entity.Buffs;
using UnityEngine;
using UnityEngine.Pool;

namespace Code.Systems.Views
{
    public class BuffViewGrid : MonoBehaviour
    {
        [SerializeField]
        private BuffView buffViewPrefab;

        [SerializeField]
        private Transform buffViewParent;

        private List<BuffView> _activeBuffs;
        private ObjectPool<BuffView> _buffViewPool;


        private void Awake()
        {
            _activeBuffs = new List<BuffView>();
            _buffViewPool = new ObjectPool<BuffView>(SpawnBuffView, OnGetFromPool, OnReturnToPool, OnDestroyBuffView);
        }


        #region Buff Pool Functions

        private BuffView SpawnBuffView()
        {
            return Instantiate(buffViewPrefab, buffViewParent);
        }

        private void OnGetFromPool(BuffView buffView)
        {
            buffView.gameObject.SetActive(true);
            _activeBuffs.Add(buffView);
        }

        private void OnReturnToPool(BuffView buffView)
        {
            buffView.gameObject.SetActive(false);
            _activeBuffs.Remove(buffView);
        }

        private void OnDestroyBuffView(BuffView buffView)
        {
            Destroy(buffView);
        }

        #endregion


        public BuffView GetBuffView()
        {
            BuffView newBuff = _buffViewPool.Get();
            return newBuff;
        }

        public void RemoveBuffView(BuffView buffViewToRemove)
        {
            if (_activeBuffs.Contains(buffViewToRemove))
            {
                _buffViewPool.Release(buffViewToRemove);
            }
        }
    }
}