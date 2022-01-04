using UnityEngine;
using Tuntenfisch.Commons.Attributes;

namespace Tuntenfisch.Commons.Pooling
{
    public class Poolable : MonoBehaviour
    {
        #region Public Variables
        public GameObject Prefab { get => m_prefab; set => m_prefab = value; }
        #endregion

        #region Inspector Variables
        [ReadOnly]
        [SerializeField]
        private GameObject m_prefab;
        #endregion
    }
}