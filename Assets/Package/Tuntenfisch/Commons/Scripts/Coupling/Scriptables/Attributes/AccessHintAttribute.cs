using System;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AccessHintAttribute : Attribute
    {
        public AccessFlags AccessFlags => m_accessFlags;

        private AccessFlags m_accessFlags;

        public AccessHintAttribute(AccessFlags accessFlags)
        {
            m_accessFlags = accessFlags;
        }
    }
}