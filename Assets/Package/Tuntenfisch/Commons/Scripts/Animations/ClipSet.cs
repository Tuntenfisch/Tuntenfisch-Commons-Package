using UnityEngine;

namespace Tuntenfisch.Commons.Animations
{
    [CreateAssetMenu(fileName = "Clip Set", menuName = "Tuntenfisch/Commons/Animations/New Clip Set", order = 1)]
    public class ClipSet : ScriptableObject
    {
        [SerializeField]
        private Clip[] m_clips;

        public virtual Clip GetClip()
        {
            return m_clips[Random.Range(0, m_clips.Length)];
        }
    }
}