using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Tuntenfisch.Commons.Animations.Rigging
{
    public class BipedIKFootPlacement : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField]
        private Animator m_animator;
        [Min(0.0f)]
        [SerializeField]
        private float m_legLength = 0.85f;

        [Header("Feet")]
        [SerializeField]
        private IKFoot m_leftFoot;
        [SerializeField]
        private IKFoot m_rightFoot;

        [Header("Spine")]
        [SerializeField]
        private ExtractTransformConstraint m_spineTransformExtractor;
        [SerializeField]
        private OverrideTransform m_spineOverrideTransform;

        [Header("Sphere Cast")]
        [Min(0.0f)]
        [SerializeField]
        private float m_sphereCastRadius = 0.05f;
        [Min(0.0f)]
        [SerializeField]
        private float m_sphereCastOriginOffsetY = 0.5f;
        [Min(0.0f)]
        [SerializeField]
        private float m_maxSphereCastDistance = 0.4f;
        [SerializeField]
        private LayerMask m_sphereCastLayerMask;

        [Header("Debug")]
        [SerializeField]
        private bool m_drawGizmos;
        [SerializeField]
        private Color m_gizmosColor = Color.cyan;
        #endregion

        #region Private Variables
        private float m_ankleHeight;
        #endregion

        #region Unity Callbacks
        private void OnEnable()
        {
            SetWeights(1.0f);
        }

        private void Start()
        {
            m_ankleHeight = CalculateAnkleHeight();
        }

        private void LateUpdate()
        {
            bool leftFootIsGrounded = PlaceFoot(m_leftFoot);
            bool rightFootIsGrounded = PlaceFoot(m_rightFoot);
            AdjustSpineHeight(leftFootIsGrounded, rightFootIsGrounded);
        }

        private void OnDestroy()
        {
            SetWeights(0.0f);
        }

        private void OnDisable()
        {
            SetWeights(0.0f);
        }

        private void OnDrawGizmosSelected()
        {
            if (!enabled || !m_drawGizmos)
            {
                return;
            }

            Gizmos.color = m_gizmosColor;
            DrawFootGizmos(m_leftFoot);
            DrawFootGizmos(m_rightFoot);
        }
        #endregion

        #region Private Methods
        private void DrawFootGizmos(IKFoot foot)
        {
            TwoBoneIKConstraint constraint = foot.FootIKConstraint;

            if (TryFindPotentialFootPlacement(constraint.data.tip, out RaycastHit hit, out Ray ray))
            {
                Gizmos.DrawLine(ray.origin, hit.point);
                Gizmos.DrawWireSphere(hit.point, m_sphereCastRadius);
            }
            else
            {
                Gizmos.DrawLine(ray.origin, (float3)ray.origin + (m_maxSphereCastDistance + m_sphereCastOriginOffsetY) * new float3(0.0f, -1.0f, 0.0f));
            }
            Gizmos.DrawSphere(ray.origin, 0.5f * m_sphereCastRadius);
        }

        private float CalculateAnkleHeight()
        {
            float ankleHeight = 0.0f;
            ankleHeight += 0.5f * math.abs(m_leftFoot.FootIKConstraint.data.target.position.y - m_leftFoot.FootIKConstraint.data.tip.position.y);
            ankleHeight += 0.5f * math.abs(m_rightFoot.FootIKConstraint.data.target.position.y - m_rightFoot.FootIKConstraint.data.tip.position.y);

            return ankleHeight;
        }

        private bool TryFindPotentialFootPlacement(Transform footTransform, out RaycastHit hit, out Ray ray)
        {
            ray = new Ray((float3)footTransform.position + new float3(0.0f, m_sphereCastOriginOffsetY, 0.0f), new float3(0.0f, -1.0f, 0.0f));

            return Physics.SphereCast(ray, m_sphereCastRadius, out hit, m_maxSphereCastDistance + m_sphereCastOriginOffsetY, m_sphereCastLayerMask, QueryTriggerInteraction.Ignore);
        }

        private float GetFootWeight(IKFoot foot, RaycastHit hit)
        {
            // If the foot's position is below the raycast's hit point, return a weight of 1 regardless of the animation curve value.
            // This prevents the foot from clipping into the ground.
            if (foot.FootTransformExtractor.data.m_position.y < hit.point.y + m_ankleHeight)
            {
                return 1.0f;
            }

            return m_animator.GetFloat(foot.WeightCurveName);
        }

        private bool PlaceFoot(IKFoot foot)
        {
            bool footIsGrounded;
            ExtractTransformConstraint footTransform = foot.FootTransformExtractor;
            TwoBoneIKConstraint constraint = foot.FootIKConstraint;

            if (footIsGrounded = TryFindPotentialFootPlacement(constraint.data.tip, out RaycastHit hit, out Ray ray))
            {
                float3 position = new float3(footTransform.data.m_position.x, hit.point.y, footTransform.data.m_position.z);
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.AngleAxis(foot.FootTransformExtractor.data.m_rotation.eulerAngles.y - 180.0f, new float3(0.0f, 1.0f, 0.0f));
                constraint.weight = GetFootWeight(foot, hit);
                constraint.data.target.SetPositionAndRotation(position, rotation);
            }
            else
            {
                constraint.weight = 0.0f;
            }

            return footIsGrounded;
        }

        // This is based on JamesB's post found here: https://forum.unity.com/threads/ik-legs-and-colliders-whats-the-theory-for-how-this-is-done.310037/
        private void AdjustSpineHeight(bool leftFootIsGrounded, bool rightFootIsGrounded)
        {
            float3 spinePosition = m_spineTransformExtractor.data.m_position;

            if (leftFootIsGrounded && rightFootIsGrounded)
            {
                float leftFootTargetHeight = m_leftFoot.FootIKConstraint.data.target.position.y;
                float rightFootTargetHeight = m_rightFoot.FootIKConstraint.data.target.position.y;
                spinePosition.y = math.min(spinePosition.y, math.min(leftFootTargetHeight, rightFootTargetHeight) + m_legLength);
            }
            m_spineOverrideTransform.data.sourceObject.position = spinePosition;
        }

        private void SetWeights(float value)
        {
            m_leftFoot.FootIKConstraint.weight = value;
            m_rightFoot.FootIKConstraint.weight = value;
            m_spineOverrideTransform.weight = value;
        }
        #endregion

        [Serializable]
        private class IKFoot
        {
            public ExtractTransformConstraint FootTransformExtractor => m_footTransformExtractor;
            public TwoBoneIKConstraint FootIKConstraint => m_footIKConstraint;
            public string WeightCurveName => m_weightCurveName;

            [SerializeField]
            private ExtractTransformConstraint m_footTransformExtractor;
            [SerializeField]
            private TwoBoneIKConstraint m_footIKConstraint;
            [SerializeField]
            private string m_weightCurveName;
        }
    }
}