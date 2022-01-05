using Tuntenfisch.Commons.Audio;
using UnityEngine;
using Tuntenfisch.Commons.Timing;

public class TempAudioSourceTest : MonoBehaviour
{
    [SerializeField]
    private AudioClipCollection m_audioClipCollection;
    [Min(0.1f)]
    [SerializeField]
    private float m_period = 1.0f;

    private TimeSince m_timeSince;

    private void Start()
    {
        m_timeSince = 0.0f;
    }

    private void Update()
    {
        if (m_timeSince >= m_period)
        {
            TempAudioSource.PlayAtPoint(transform.position, m_audioClipCollection.SetupAudioSource);
            m_timeSince = 0.0f;
        }
    }
}