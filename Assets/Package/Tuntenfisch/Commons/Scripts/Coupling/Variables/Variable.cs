using System;
using Tuntenfisch.Commons.Attributes;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Variables
{
    public abstract class Variable<T> : ScriptableObject
    {
        #region Events
        public event Action<T> OnCurrentValueChanged
        {
            add
            {
                m_onCurrentValueChanged -= value;
                m_onCurrentValueChanged += value;
            }

            remove 
            {
                m_onCurrentValueChanged -= value;
            }
        }
        #endregion

        #region Public Properties
        public T CurrentValue
        {
            get
            {
                if (!m_reset)
                {
                    m_currentValue = GetCopyOf(m_defaultValue);
                }
                m_reset = true;
                return m_currentValue;
            }

            set
            {
                if (m_isConstant)
                {
                    throw new InvalidOperationException($"Assigning a new value to constant {nameof(Variable<T>)} \"{name}\" isn't allowed.");
                }
                m_reset = true;
                m_currentValue = value;
                m_onCurrentValueChanged?.Invoke(m_currentValue);
            }
        }
        #endregion

        #region Inspector Fields
        [InlineField]
        [SerializeField]
        private T m_defaultValue;
        [InlineField]
        [SerializeField]
        private T m_currentValue;
        [SerializeField]
        private bool m_isConstant;
        #endregion

        #region Private Fields
        private bool m_reset;
        private Action<T> m_onCurrentValueChanged;
        #endregion

        #region Unity Callbacks
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                m_currentValue = GetCopyOf(m_defaultValue);
            }
        }
        #endregion

        #region Proptected Methods
        // If the generic type parameter specifies a reference type the derived class should override this.
        protected virtual T GetCopyOf(T obj)
        {
            return obj;
        }
        #endregion
    }
}