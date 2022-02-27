using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

namespace Tuntenfisch.Commons.Animations.Rigging
{
    // Courtesy of Catsoft-Studios (https://forum.unity.com/threads/read-bones-transforms-before-ik-corrections-with-animation-rigging.813966/).
    [DisallowMultipleComponent]
    public class ExtractTransformConstraint : RigConstraint<ExtractTransformConstraintJob, ExtractTransformConstraintData, ExtractTransformConstraintJobBinder>
    {

    }

    public struct ExtractTransformConstraintJob : IWeightedAnimationJob
    {
        #region Public Properties
        public FloatProperty jobWeight { get; set; }
        #endregion

        #region Public Fields
        public ReadWriteTransformHandle m_transformHandle;
        public Vector3Property m_position;
        public Vector4Property m_rotation;
        #endregion

        #region Public Methods
        public void ProcessRootMotion(AnimationStream stream)
        {

        }

        public void ProcessAnimation(AnimationStream stream)
        {
            AnimationRuntimeUtils.PassThrough(stream, m_transformHandle);

            float3 position = m_transformHandle.GetPosition(stream);
            Quaternion rotation = m_transformHandle.GetRotation(stream);

            m_position.Set(stream, position);
            m_rotation.Set(stream, new Vector4(rotation.x, rotation.y, rotation.z, rotation.w));
        }
        #endregion
    }

    [Serializable]
    public struct ExtractTransformConstraintData : IAnimationJobData
    {
        #region Public Fields
        [SyncSceneToStream]
        public Transform m_bone;
        [HideInInspector]
        public float3 m_position;
        [HideInInspector]
        public Quaternion m_rotation;
        #endregion

        #region Public Methods
        public bool IsValid()
        {
            return m_bone != null;
        }

        public void SetDefaultValues()
        {
            m_bone = null;
            m_position = 0.0f;
            m_rotation = Quaternion.identity;
        }
        #endregion
    }

    public class ExtractTransformConstraintJobBinder : AnimationJobBinder<ExtractTransformConstraintJob, ExtractTransformConstraintData>
    {
        #region Public Methods
        public override ExtractTransformConstraintJob Create(Animator animator, ref ExtractTransformConstraintData data, Component component)
        {
            return new ExtractTransformConstraintJob
            {
                m_transformHandle = ReadWriteTransformHandle.Bind(animator, data.m_bone),
                m_position = Vector3Property.Bind(animator, component, "m_Data." + nameof(data.m_position)),
                m_rotation = Vector4Property.Bind(animator, component, "m_Data." + nameof(data.m_rotation)),
            };
        }

        public override void Destroy(ExtractTransformConstraintJob job)
        {

        }
        #endregion
    }
}
