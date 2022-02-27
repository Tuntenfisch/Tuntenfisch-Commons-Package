using System;
using Tuntenfisch.Commons.Coupling.Attributes;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Variables
{
    [Serializable]
    public sealed class VariableReadReference<T> : VariableReference<T>
    {
        #region Public Properties
        public override T CurrentValue
        {
            get
            {
                return base.CurrentValue;
            }

            set
            {
                throw new InvalidOperationException();
            }
        }
        #endregion

        #region Protected Properties
        protected override Variable<T> Variable => m_variable;
        #endregion

        #region Inspector Fields
        [AccessHint(AccessFlags.Read)]
        [SerializeField]
        private Variable<T> m_variable;
        #endregion

        #region Protected Methods
        protected override AccessFlags GetAccessFlags()
        {
            return AccessFlags.Read;
        }
        #endregion
    }
}