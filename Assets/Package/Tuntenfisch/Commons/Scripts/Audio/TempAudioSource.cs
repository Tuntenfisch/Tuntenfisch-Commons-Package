using Cysharp.Threading.Tasks;
using System;
using Tuntenfisch.Commons.Timing;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

namespace Tuntenfisch.Commons.Scripts.Audio
{
    public static class TempAudioSource
    {
        #region Private Variables
        private static GameObject m_prefab;
        private static ObjectPool<AudioSource> m_pool;
        #endregion

        static TempAudioSource()
        {
            m_prefab = new GameObject("Temporary Audio Source");
            m_prefab.AddComponent<AudioSource>();
            m_pool = new ObjectPool<AudioSource>
            (
                () => 
                {
                    GameObject instance = UnityEngine.Object.Instantiate(m_prefab);
                    return instance.GetComponent<AudioSource>();
                },
                (instance) => instance.gameObject.SetActive(true),
                (instance) => instance.gameObject.SetActive(false),
                (instance) => UnityEngine.Object.Destroy(instance.gameObject),
                true,
                10,
                100
            );
        }

        #region Public Methods
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

        public static void Play(float3 position, Action<AudioSource> setupAudioSource)
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
                Return(audioSource);
            });
        }
        #endregion
    }
}