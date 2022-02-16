using System;
using Tuntenfisch.Commons.Attributes;
using Unity.Mathematics;
using UnityEngine;

namespace Tuntenfisch.Commons.Audio
{
    [CreateAssetMenu(fileName = "Audio Clip Collection", menuName = "Tuntenfisch Commons/Audio/New Audio Clip Collection")]
    public class AudioClipCollection : ScriptableObject
    {
        #region Inspector Fields
        [SerializeField]
        private AudioClip[] m_audioClips;
        [MinMaxRange(0.0f, 1.0f)]
        [SerializeField]
        private float2 m_volumeRange = 1.0f;
        [MinMaxRange(0.0f, 5.0f)]
        [SerializeField]
        private float2 m_pitchRange = 1.0f;
        #endregion

        #region Public Methods
        public virtual float Play(AudioSource audioSource)
        {
            ValidateState();
            SetupAudioSource(audioSource);
            audioSource.Play();
            return audioSource.clip.length;
        }

        public virtual void SetupAudioSource(AudioSource audioSource)
        {
            ValidateState();
            audioSource.clip = m_audioClips[UnityEngine.Random.Range(0, m_audioClips.Length)];
            audioSource.volume = UnityEngine.Random.Range(m_volumeRange.x, m_volumeRange.y);
            audioSource.pitch = UnityEngine.Random.Range(m_pitchRange.x, m_pitchRange.y);
        }
        #endregion

        #region Protected Methods
        protected virtual void ValidateState()
        {
            if (m_audioClips.Length == 0)
            {
                throw new InvalidOperationException($"{nameof(AudioClipCollection)} \"{name}\" cannot play an audio clip because none are registered.");
            }
        }
        #endregion
    }
}