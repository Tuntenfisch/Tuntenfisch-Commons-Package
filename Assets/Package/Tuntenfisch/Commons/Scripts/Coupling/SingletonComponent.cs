using UnityEngine;

namespace Tuntenfisch.Commons.Coupling
{
    public abstract class SingletonComponent<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Public Variables
        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = FindObjectOfType<T>();
                }

                return s_instance;
            }
        }
        #endregion

        #region Private Variables
        private static T s_instance = default;
        #endregion
    }
}