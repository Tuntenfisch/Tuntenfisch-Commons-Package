using UnityEngine;

namespace Tuntenfisch.Commons.Attributes
{
    public class MinMaxRangeAttribute : PropertyAttribute
    {
        #region Public Variables
        public float Min => m_min;
        public float Max => m_max;
        #endregion

        #region Private Variables
        private float m_min;
        private float m_max;
        #endregion

        #region Public Methods
        public MinMaxRangeAttribute(float min, float max)
        {
            m_min = min;
            m_max = max;
        }
        #endregion
    }
}