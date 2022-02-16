using System;
using Unity.Mathematics;
using UnityEngine;

namespace Tuntenfisch.Commons.Environment
{
    [Serializable]
    public struct WindProperties
    {
        #region Public Properties
        public float3 Rotation { get => m_rotation; set => m_rotation = value; }
        public float Speed { get => m_speed; set => m_speed = value; }
        public float Frequency { get => m_frequency; set => m_frequency = value; }
        #endregion

        #region Inspector Fields
        [SerializeField]
        private float3 m_rotation;
        [SerializeField]
        private float m_speed;
        [SerializeField]
        private float m_frequency;
        #endregion
    }
}