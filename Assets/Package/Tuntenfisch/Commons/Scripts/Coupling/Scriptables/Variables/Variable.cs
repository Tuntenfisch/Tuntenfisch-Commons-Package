using System;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Variables
{
    public abstract class Variable<T> : ScriptableObject
    {
        #region Public Variables
        public T CurrentValue
        {
            get => m_currentValue;

            set
            {
                if (m_isConstant)
                {
                    throw new InvalidOperationException($"Assigning a new value to constant {nameof(Variable<T>)} \"{name}\" isn't allowed.");
                }
                m_currentValue = value;
            }
        }
        #endregion

        #region Inspector Variables
        [SerializeField]
        private T m_defaultValue;
        [SerializeField]
        private T m_currentValue;
        [SerializeField]
        private bool m_isConstant;
        #endregion

        #region Unity Callbacks
        private void Awake()
        {
            m_currentValue = m_defaultValue;
        }

        private void OnEnable()
        {
            m_currentValue = m_defaultValue;
        }

        private void OnValidate()
        {
            if (m_isConstant)
            {
                m_currentValue = m_defaultValue;
            }
        }
        #endregion
    }
}