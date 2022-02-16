using UnityEngine;

namespace Tuntenfisch.Commons.Animations
{
    [CreateAssetMenu(fileName = "Clip Collection", menuName = "Tuntenfisch Commons/Animations/New Clip Collection", order = 1)]
    public class ClipCollection : ScriptableObject
    {
        #region Inspector Fields
        [SerializeField]
        private Clip[] m_clips;
        #endregion

        #region Public Methods
        public virtual Clip GetClip()
        {
            return m_clips[Random.Range(0, m_clips.Length)];
        }
        #endregion
    }
}