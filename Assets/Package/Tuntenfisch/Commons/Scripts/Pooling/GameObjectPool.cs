using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Tuntenfisch.Commons.Pooling
{
    public class GameObjectPool
    {
        private Dictionary<GameObject, ObjectPool<GameObject>> m_pool = new Dictionary<GameObject, ObjectPool<GameObject>>();

        public GameObjectPool()
        {

        }

        public GameObject Get(GameObject prefab)
        {
            if (!m_pool.ContainsKey(prefab))
            {
                AllocatePool(prefab, 1, 1000);
            }
            return m_pool[prefab].Get();
        }

        public void Release(GameObject instance)
        {
            Poolable poolable = instance.GetComponent<Poolable>();

            if (poolable == null)
            {
                Debug.LogWarning($"{nameof(GameObject)} {instance.name} is not a pooled {nameof(GameObject)}. Destroying it instead.");
                Object.Destroy(instance);

                return;
            }
            m_pool[poolable.Prefab].Release(instance);
        }

        public GameObjectPool AllocatePool(GameObject prefab, int size, int capacity)
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
                    Poolable poolable = instance.AddComponent<Poolable>();
                    poolable.Prefab = prefab;

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
    }
}