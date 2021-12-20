using UnityEngine;

namespace Tuntenfisch.Commons.Attributes
{
    public class DisplayNameAttribute : PropertyAttribute
    {
        public string DisplayName => m_displayName;

        private string m_displayName;

        public DisplayNameAttribute(string displayName)
        {
            m_displayName = displayName;
        }
    }
}