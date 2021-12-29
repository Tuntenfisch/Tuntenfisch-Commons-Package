using System;
using Tuntenfisch.Commons.Coupling.Scriptables.Attributes;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Variables
{
    [Serializable]
    public sealed class VariableReadReference<T>
    {
        public T CurrentValue
        {
            get => m_useLiteral ? m_literalValue : m_variable.CurrentValue;
        }

        [AccessHint(AccessFlags.Read)]
        [SerializeField]
        private Variable<T> m_variable;
        [SerializeField]
        private T m_literalValue;
        [SerializeField]
        private bool m_useLiteral;
    }
}