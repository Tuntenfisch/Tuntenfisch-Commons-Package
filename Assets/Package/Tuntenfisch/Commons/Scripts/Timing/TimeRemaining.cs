using UnityEngine;

namespace Tuntenfisch.Commons.Timing
{
    public struct TimeRemaining
    {
        private float m_time;

        public static implicit operator float(TimeRemaining timeRemaining)
        {
            return timeRemaining.m_time - Time.time;
        }

        public static implicit operator TimeRemaining(float time)
        {
            return new TimeRemaining { m_time = Time.time + time };
        }
    }
}