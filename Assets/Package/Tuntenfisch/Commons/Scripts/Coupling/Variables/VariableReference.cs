using System;
using Tuntenfisch.Commons.Attributes;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Variables
{
    public abstract class VariableReference<T>
    {
        #region Events
        public virtual event Action<T> OnCurrentValueChanged
        {
            add
            {
                if (m_useLiteral)
                {
                    Debug.LogWarning($"{nameof(OnCurrentValueChanged)} event is not invoked for literal values.");
                    return;
                }

                if (Variable != null)
                {
                    Variable.OnCurrentValueChanged += value;
                }
            }

            remove
            {
                if (m_useLiteral)
                {
                    Debug.LogWarning($"{nameof(OnCurrentValueChanged)} event is not invoked for literal values.");
                    return;
                }

                if (Variable != null)
                {
                    Variable.OnCurrentValueChanged -= value;
                }
            }
        }
        #endregion

        #region Public Properties
        public virtual T CurrentValue
        {
            get
            {
                if (m_useLiteral)
                {
                    return m_literalValue;
                }

                if (Variable != null)
                {
                    return Variable.CurrentValue;
                }
                return default;
            }

            set
            {
                if (m_useLiteral)
                {
                    m_literalValue = value;
                }
                else if (Variable != null)
                {
                    Variable.CurrentValue = value;
                }
            }
        }
        #endregion

        #region Protected Properties
        protected virtual Variable<T> Variable { get => null; }
        #endregion

        #region Inspector Fields
        [InlineField]
        [SerializeField]
        private T m_literalValue;
        [SerializeField]
        private bool m_useLiteral;
        #endregion

        #region Protected Methods
        protected abstract AccessFlags GetAccessFlags();
        #endregion
    }
}