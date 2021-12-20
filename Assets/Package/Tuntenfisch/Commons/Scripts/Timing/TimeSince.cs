using UnityEngine;

namespace Tuntenfisch.Commons.Timing
{
    public struct TimeSince
    {
        private float m_time;

        public static implicit operator float(TimeSince timeSince)
        {
            return Time.time - timeSince.m_time;
        }

        public static implicit operator TimeSince(float time)
        {
            return new TimeSince { m_time = Time.time - time };
        }
    }
}