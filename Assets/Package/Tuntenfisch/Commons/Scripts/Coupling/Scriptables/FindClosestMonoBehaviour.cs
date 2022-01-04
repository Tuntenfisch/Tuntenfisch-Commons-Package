using Tuntenfisch.Commons.Coupling.Scriptables.Attributes;
using Tuntenfisch.Commons.Coupling.Scriptables.Sets;
using Tuntenfisch.Commons.Coupling.Scriptables.Variables;
using Unity.Mathematics;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Scriptables
{
    public abstract class FindClosestMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Inspector Variables
        [AccessHint(AccessFlags.Read)]
        [SerializeField]
        private RuntimeSet<T> m_runtimeSet;
        [AccessHint(AccessFlags.Write)]
        [SerializeField]
        private Variable<T> m_output;
        #endregion

        #region Unity Callbacks
        private void Update()
        {
            float shortestDistanceSquared = float.MaxValue;
            T closestMonoBehaviour = null;

            foreach (T monoBehaviour in m_runtimeSet)
            {
                float distanceSquared = math.lengthsq(monoBehaviour.transform.position - transform.position);

                if (distanceSquared < shortestDistanceSquared)
                {
                    shortestDistanceSquared = distanceSquared;
                    closestMonoBehaviour = monoBehaviour;
                }
            }
            m_output.CurrentValue = closestMonoBehaviour;
        }
        #endregion
    }
}