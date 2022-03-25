using Cysharp.Threading.Tasks;
using System;
using Tuntenfisch.Commons.Timing;
using Unity.Mathematics;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Pool;

namespace Tuntenfisch.Commons.Audio
{
    public static class TempAudioSource
    {
        #region Private Fields
        private static ObjectPool<AudioSource> m_pool;
        #endregion

        static TempAudioSource()
        {
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
            m_pool = new ObjectPool<AudioSource>
            (
                () =>
                {
                    GameObject gameObject = new GameObject("Temporary Audio Source");
                    AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                    audioSource.playOnAwake = false;
                    audioSource.loop = false;
                    return audioSource;
                },
                (audioSource) => audioSource.gameObject.SetActive(true),
                (audioSource) => audioSource.gameObject.SetActive(false),
                (audioSource) => UnityEngine.Object.Destroy(audioSource.gameObject),
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
                throw new ArgumentException(nameof(AudioSource));
            }
            m_pool.Release(audioSource);
        }

        public static void PlayAtPoint(float3 position, Action<AudioSource> setupAudioSource)
        {
            if (setupAudioSource == null)
            {
                throw new ArgumentException(nameof(setupAudioSource));
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

        #region Private Methods
#if UNITY_EDITOR
        private static void OnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            if (playModeStateChange == PlayModeStateChange.ExitingPlayMode || playModeStateChange == PlayModeStateChange.EnteredPlayMode)
            {
                m_pool?.Clear();
            }
        }
#endif
        #endregion
    }
}