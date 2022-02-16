using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Tuntenfisch.Commons.Animations.Rigging
{
    [RequireComponent(typeof(Animator))]
    public class RootMotion : MonoBehaviour
    {
        #region Inspector Fields
        [SerializeField]
        private UnityEvent<float3, float3> OnRootMotionExtracted;
        #endregion

        #region Private Fields
        private Animator m_animator;
        #endregion

        #region Unity Callbacks
        private void Start()
        {
            m_animator = GetComponent<Animator>();
        }

        private void OnAnimatorMove()
        {
            OnRootMotionExtracted?.Invoke(m_animator.velocity, m_animator.angularVelocity);
        }
        #endregion
    }
}