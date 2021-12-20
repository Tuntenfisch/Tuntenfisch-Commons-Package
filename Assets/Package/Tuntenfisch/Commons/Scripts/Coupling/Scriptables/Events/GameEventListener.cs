using Tuntenfisch.Commons.Coupling.Scriptables.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Events
{
    public sealed class GameEventListener : MonoBehaviour
    {
        public GameEvent GameEvent => m_gameEvent;
        public UnityEvent Response => m_response;

        [AccessHint(AccessFlags.Read)]
        [Tooltip("Game event to register with.")]
        [SerializeField]
        private GameEvent m_gameEvent;
        [Tooltip("Response to invoke when game event is raised.")]
        [SerializeField]
        private UnityEvent m_response;

        private void OnEnable()
        {
            m_gameEvent.OnGameEventInvoked += OnGameEventInvoked;
        }

        private void OnDisable()
        {
            m_gameEvent.OnGameEventInvoked -= OnGameEventInvoked;
        }

        private void OnGameEventInvoked()
        {
            m_response.Invoke();
        }
    }
}