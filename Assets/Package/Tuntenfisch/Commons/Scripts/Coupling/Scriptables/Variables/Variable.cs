using System;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Variables
{
    public abstract class Variable<T> : ScriptableObject
    {
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

        [SerializeField]
        private T m_defaultValue;
        [SerializeField]
        private T m_currentValue;
        [SerializeField]
        private bool m_isConstant;

        private void Awake()
        {
            m_currentValue = m_defaultValue;
        }

        private void OnEnable()
        {
            m_currentValue = m_defaultValue;
        }
    }
}