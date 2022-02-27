using UnityEngine;

namespace Tuntenfisch.Commons.Timing
{
    public struct TimeRemaining
    {
        #region Private Fields
        private float m_time;
        #endregion

        #region Public Methods
        public static implicit operator float(TimeRemaining timeRemaining)
        {
            return timeRemaining.m_time - Time.time;
        }

        public static implicit operator TimeRemaining(float time)
        {
            return new TimeRemaining { m_time = Time.time + time };
        }
        #endregion
    }
}