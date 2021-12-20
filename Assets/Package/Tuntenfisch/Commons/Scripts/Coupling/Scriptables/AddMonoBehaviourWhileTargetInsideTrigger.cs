using Tuntenfisch.Commons.Coupling.Scriptables.Attributes;
using Tuntenfisch.Commons.Coupling.Scriptables.Sets;
using Tuntenfisch.Commons.Coupling.Scriptables.Variables;
using UnityEngine;


namespace Tuntenfisch.Commons.Coupling.Scriptables
{
    public abstract class AddMonoBehaviourWhileTargetInsideTrigger<T> : MonoBehaviour where T : MonoBehaviour
    {
        [AccessHint(AccessFlags.Write)]
        [SerializeField]
        private RuntimeSet<T> m_runtimeSet;
        [AccessHint(AccessFlags.Read)]
        [SerializeField]
        private Variable<GameObject> m_target;

        private T m_monobehaviour;

        private void Awake()
        {
            m_monobehaviour = GetComponent<T>();

            if (m_monobehaviour == null)
            {
                Debug.LogWarning($"{nameof(gameObject)} \"{gameObject.name}\" does not have a {nameof(T)} component attached.");
            }
        }

        private void OnDestroy()
        {
            if (m_runtimeSet.Contains(m_monobehaviour))
            {
                m_runtimeSet.Remove(m_monobehaviour);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_monobehaviour == null || other.gameObject != m_target.CurrentValue)
            {
                return;
            }
            m_runtimeSet.Add(m_monobehaviour);
        }

        private void OnTriggerExit(Collider other)
        {
            if (m_monobehaviour == null || other.gameObject != m_target.CurrentValue)
            {
                return;
            }
            m_runtimeSet.Remove(m_monobehaviour);
        }
    }
}