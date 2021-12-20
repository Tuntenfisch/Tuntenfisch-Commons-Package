using UnityEngine;
using Tuntenfisch.Commons.Attributes;

namespace Tuntenfisch.Commons.Pooling
{
    public class Poolable : MonoBehaviour
    {
        public GameObject Prefab { get => m_prefab; set => m_prefab = value; }

        [ReadOnly]
        [SerializeField]
        private GameObject m_prefab;
    }
}