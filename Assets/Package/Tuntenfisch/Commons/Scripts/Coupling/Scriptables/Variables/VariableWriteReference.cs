﻿using System;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Variables
{
    [Serializable]
    public sealed class VariableWriteReference<T>
    {
        public T CurrentValue
        {
            set
            {
                if (m_useLiteral)
                {
                    m_literalValue = value;
                }
                else
                {
                    m_variable.CurrentValue = value;
                }
            }
        }

        [SerializeField]
        private Variable<T> m_variable;
        [SerializeField]
        private T m_literalValue;
        [SerializeField]
        private bool m_useLiteral;
    }
}