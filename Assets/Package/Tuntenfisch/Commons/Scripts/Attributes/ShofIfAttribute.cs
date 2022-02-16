using UnityEngine;

namespace Tuntenfisch.Commons.Attributes
{
    public class ShowIfAttribute : PropertyAttribute
    {
        #region Public Properties
        public string FieldName => m_fieldName;
        public object Value => m_value;
        #endregion

        #region Private Fields
        private string m_fieldName;
        private object m_value;
        #endregion

        #region Public Methods
        public ShowIfAttribute(string fieldName, object value)
        {
            m_fieldName = fieldName;
            m_value = value;
        }
        #endregion
    }
}