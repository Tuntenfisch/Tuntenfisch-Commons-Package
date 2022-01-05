﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Tuntenfisch.Commons.Pooling
{
    public class MultiPrefabPool
    {
        #region Private Variables
        private Dictionary<GameObject, ObjectPool<GameObject>> m_pool = new Dictionary<GameObject, ObjectPool<GameObject>>();
        #endregion

        public MultiPrefabPool()
        {

        }

        #region Public Methods
        public GameObject Get(GameObject prefab)
        {
            if (!m_pool.ContainsKey(prefab))
            {
                AllocatePool(prefab, 1, 1000);
            }
            return m_pool[prefab].Get();
        }

        public void Return(GameObject instance)
        {
            PooledPrefab poolable = instance.GetComponent<PooledPrefab>();

            if (poolable == null)
            {
                Debug.LogWarning($"{nameof(GameObject)} {instance.name} is not a pooled {nameof(GameObject)}. Destroying it instead.");
                Object.Destroy(instance);

                return;
            }
            m_pool[poolable.Prefab].Release(instance);
        }

        public MultiPrefabPool AllocatePool(GameObject prefab, int size, int capacity)
        {
            if (m_pool.ContainsKey(prefab))
            {
                return this;
            }

            ObjectPool<GameObject> pool = new ObjectPool<GameObject>
            (
                () =>
                {
                    GameObject instance = Object.Instantiate(prefab);
                    PooledPrefab pooledPrefab = instance.AddComponent<PooledPrefab>();
                    pooledPrefab.Prefab = prefab;

                    return instance;
                },
                (instance) => instance.SetActive(true),
                (instance) => instance.SetActive(false),
                (instance) => Object.Destroy(instance),
                true,
                size,
                capacity
            );
            m_pool[prefab] = pool;

            return this;
        }
        #endregion
    }
}