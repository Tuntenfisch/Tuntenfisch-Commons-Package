using System;
using Tuntenfisch.Commons.Coupling.Attributes;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Variables
{
    [Serializable]
    public sealed class VariableReadWriteReference<T> : VariableReference<T>
    {
        #region Protected Properties
        protected override Variable<T> Variable => m_variable;
        #endregion

        #region Inspector Fields
        [AccessHint(AccessFlags.Read | AccessFlags.Write)]
        [SerializeField]
        private Variable<T> m_variable;
        #endregion

        #region Protected Methods
        protected override AccessFlags GetAccessFlags()
        {
            return AccessFlags.Read | AccessFlags.Write;
        }
        #endregion
    }
}