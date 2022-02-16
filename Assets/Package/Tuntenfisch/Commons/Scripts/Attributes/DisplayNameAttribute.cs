using UnityEngine;

namespace Tuntenfisch.Commons.Attributes
{
    public class DisplayNameAttribute : PropertyAttribute
    {
        #region Public Properties
        public string DisplayName => m_displayName;
        #endregion

        #region Private Fields
        private string m_displayName;
        #endregion

        #region Public Methods
        public DisplayNameAttribute(string displayName)
        {
            m_displayName = displayName;
        }
        #endregion
    }
}