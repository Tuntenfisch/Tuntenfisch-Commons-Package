using System;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Events
{
    [CreateAssetMenu(fileName = "Game Event", menuName = "Tuntenfisch/Coupling/Scriptables/Events/New Game Event")]
    public sealed class GameEvent : ScriptableObject
    {
        public event Action OnGameEventInvoked;

        public void InvokeGameEvent()
        {
            OnGameEventInvoked?.Invoke();
        }
    }
}