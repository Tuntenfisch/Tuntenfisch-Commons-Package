using Tuntenfisch.Commons.Coupling.Scriptables.Attributes;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Variables
{
    public sealed class VariableSetter : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField]
        private VariableType m_variableType;
        [AccessHint(AccessFlags.Write)]
        [SerializeField]
        private Variable<GameObject> m_gameObjectVariable;
        [SerializeField]
        private GameObject m_gameObjectValue;
        [AccessHint(AccessFlags.Write)]
        [SerializeField]
        private Variable<Transform> m_transformVariable;
        [SerializeField]
        private Transform m_transformValue;
        #endregion

        #region Unity Callbacks
        private void Awake()
        {
            switch(m_variableType)
            {
                case VariableType.GameObject:
                    m_gameObjectVariable.CurrentValue = m_gameObjectValue;
                    break;

                case VariableType.Transform:
                    m_transformVariable.CurrentValue = m_transformValue;
                    break;
            }
        }
        #endregion

        public enum VariableType
        {
            GameObject,
            Transform
        }
    }
}