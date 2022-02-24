using UnityEngine;

namespace Tuntenfisch.Commons.Attributes
{
    public class FloatColorAttribute : PropertyAttribute
    {
        #region Public Properties
        public bool IsLinear => m_isLinear;
        #endregion

        #region Private Fields
        private readonly bool m_isLinear;
        #endregion

        #region Public Methods
        public FloatColorAttribute(bool isLinear = false)
        {
            m_isLinear = isLinear;
        }
        #endregion
    }
}