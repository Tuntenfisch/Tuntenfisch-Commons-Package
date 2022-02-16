using System;

namespace Tuntenfisch.Commons.Coupling.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AccessHintAttribute : Attribute
    {
        #region Public Properties
        public AccessFlags AccessFlags => m_accessFlags;
        #endregion

        #region Private Fields
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