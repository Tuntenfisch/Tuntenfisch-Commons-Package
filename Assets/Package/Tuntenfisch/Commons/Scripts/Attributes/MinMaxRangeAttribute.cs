using UnityEngine;

namespace Tuntenfisch.Commons.Attributes
{
    public class MinMaxRangeAttribute : PropertyAttribute
    {
        #region Public Properties
        public float MinLimit => m_minLimit;
        public float MaxLimit => m_maxLimit;
        #endregion

        #region Private Fields
        private float m_minLimit;
        private float m_maxLimit;
        #endregion

        #region Public Methods
        public MinMaxRangeAttribute(float minLimit, float maxLimit)
        {
            m_minLimit = minLimit;
            m_maxLimit = maxLimit;
        }
        #endregion
    }
}