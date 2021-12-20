using UnityEngine;

namespace Tuntenfisch.Commons.Attributes
{
    public class ShowIfAttribute : PropertyAttribute
    {
        public string FieldName => m_fieldName;
        public object Value => m_value;

        private string m_fieldName;
        private object m_value;

        public ShowIfAttribute(string fieldName, object value)
        {
            m_fieldName = fieldName;
            m_value = value;
        }
    }
}