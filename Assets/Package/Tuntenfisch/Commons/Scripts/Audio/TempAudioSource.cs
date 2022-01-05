using Cysharp.Threading.Tasks;
using System;
using Tuntenfisch.Commons.Timing;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

namespace Tuntenfisch.Commons.Audio
{
    public static class TempAudioSource
    {
        #region Private Variables
        private static ObjectPool<AudioSource> m_pool;
        #endregion

        static TempAudioSource()
        {
            m_pool = new ObjectPool<AudioSource>
            (
                () =>
                {
                    GameObject gameObject = new GameObject("Temporary Audio Source");
                    // If we don't set HideFlags.DontSave an NRE exception will be thrown 
                    // occasionally when we enter and exit playmode without a domain reload.
                    gameObject.hideFlags |= HideFlags.DontSave;
                    AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                    audioSource.playOnAwake = false;
                    audioSource.loop = false;
                    return audioSource;
                },
                (audioSource) =>
                {
                    audioSource.gameObject.SetActive(true);
                },
                (audioSource) =>
                {
                    audioSource.gameObject.SetActive(false);
                },
                (audioSource) =>
                {
                    UnityEngine.Object.Destroy(audioSource.gameObject);
                },
                true,
                10,
                100
            );
        }

        #region Public Methods
        public static AudioSource Get()
        {
            return Get(0.0f);
        }

        public static AudioSource Get(float3 position)
        {
            AudioSource audioSource = m_pool.Get();
            audioSource.transform.position = position;
            return audioSource;
        }

        public static void Return(AudioSource audioSource)
        {
            if (audioSource == null)
            {
                throw new NullReferenceException($"Parameter {nameof(audioSource)} cannot be null.");
            }
            m_pool.Release(audioSource);
        }

        public static void PlayAtPoint(float3 position, Action<AudioSource> setupAudioSource)
        {
            if (setupAudioSource == null)
            {
                throw new NullReferenceException($"Parameter {nameof(setupAudioSource)} cannot be null.");
            }
            AudioSource audioSource = Get(position);
            setupAudioSource(audioSource);
            TimeRemaining timeRemaining = audioSource.clip.length;
            audioSource.Play();
            UniTask.Void(async () =>
            {
                await UniTask.WaitUntil(() => timeRemaining <= 0.0f);
                m_pool.Release(audioSource);
            });
        }
        #endregion
    }
}