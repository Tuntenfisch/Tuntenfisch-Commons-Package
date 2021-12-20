using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Tuntenfisch.Commons.Animations.Rigging
{
    [RequireComponent(typeof(Animator))]
    public class RootMotion : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<float3, float3> OnRootMotionExtracted;

        private Animator m_animator;

        private void Start()
        {
            m_animator = GetComponent<Animator>();
        }

        private void OnAnimatorMove()
        {
            OnRootMotionExtracted?.Invoke(m_animator.velocity, m_animator.angularVelocity);
        }
    }
}