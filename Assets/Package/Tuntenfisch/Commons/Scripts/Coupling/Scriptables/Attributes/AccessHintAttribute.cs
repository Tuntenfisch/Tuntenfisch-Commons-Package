using System;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AccessHintAttribute : Attribute
    {
        #region Public Variables
        public AccessFlags AccessFlags => m_accessFlags;
        #endregion

        #region Private Variables
        private AccessFlags m_accessFlags;
        #endregion

        #region Public Methods
        public AccessHintAttribute(AccessFlags accessFlags)
        {
            m_accessFlags = accessFlags;
        }
        #endregion
    }
}