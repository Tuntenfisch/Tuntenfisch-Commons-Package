using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Tuntenfisch.Commons.Pooling
{
    public class MultiPrefabPool
    {
        #region Private Variables
        private Dictionary<GameObject, ObjectPool<GameObject>> m_pool = new Dictionary<GameObject, ObjectPool<GameObject>>();
        #endregion

        #region Public Methods
        public T Get<T>(GameObject prefab) where T : Component
        {
            return Get(prefab).GetComponent<T>();
        }

        public GameObject Get(GameObject prefab)
        {
            if (prefab == null)
            {
                throw new ArgumentException(nameof(prefab));
            }

            if (!m_pool.ContainsKey(prefab))
            {
                throw new UnkownPooledPrefabException(prefab);
            }
            return m_pool[prefab].Get();
        }

        public void Return(Component pooledComponent)
        {
            Return(pooledComponent.gameObject);
        }

        public void Return(GameObject pooledInstance)
        {
            if (pooledInstance == null)
            {
                throw new ArgumentException(nameof(pooledInstance));
            }
            PooledPrefab pooledPrefab = pooledInstance.GetComponent<PooledPrefab>();

            if (pooledPrefab == null)
            {
                Debug.LogWarning($"{nameof(GameObject)} \"{pooledInstance.name}\" is not a pooled {nameof(GameObject)}. Destroying it instead.");
                UnityEngine.Object.Destroy(pooledInstance);

                return;
            }

            if (pooledPrefab.Parent != this)
            {
                throw new InvalidOperationException($"Cannot return {nameof(GameObject)} \"{pooledInstance.name}\" to this pool. Another pool created this pooled instance.");
            }
            m_pool[pooledPrefab.Prefab].Release(pooledInstance);
        }

        public void AllocatePool(GameObject prefab, Transform parent, int size, int capacity)
        {
            if (m_pool.ContainsKey(prefab))
            {
                return;
            }

            ObjectPool<GameObject> pool = new ObjectPool<GameObject>
            (
                () =>
                {
                    GameObject instance = UnityEngine.Object.Instantiate(prefab, parent);
                    PooledPrefab pooledPrefab = instance.AddComponent<PooledPrefab>();
                    pooledPrefab.Parent = this;
                    pooledPrefab.Prefab = prefab;

                    return instance;
                },
                (instance) => instance.SetActive(true),
                (instance) => instance.SetActive(false),
                (instance) => UnityEngine.Object.Destroy(instance),
                true,
                size,
                capacity
            );
            m_pool[prefab] = pool;

            return;
        }
        #endregion
    }
}