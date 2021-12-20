﻿using System;
using UnityEngine;

namespace Tuntenfisch.Commons.Animations
{
    [Serializable]
    public class Clip
    {
        public float SpeedMultiplier => m_animation.length / m_targetDuration;

        public AnimationClip Animation => m_animation;
        public float TargetDuration => m_targetDuration;

        [SerializeField]
        private AnimationClip m_animation;
        [SerializeField]
        private float m_targetDuration = 1.0f;
    }
}