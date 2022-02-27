using UnityEngine;

namespace Tuntenfisch.Commons.Timing
{
    public struct TimeSince
    {
        #region Private Fields
        private float m_time;
        #endregion

        #region Public Methods
        public static implicit operator float(TimeSince timeSince)
        {
            return Time.time - timeSince.m_time;
        }

        public static implicit operator TimeSince(float time)
        {
            return new TimeSince { m_time = Time.time - time };
        }
        #endregion
    }
}