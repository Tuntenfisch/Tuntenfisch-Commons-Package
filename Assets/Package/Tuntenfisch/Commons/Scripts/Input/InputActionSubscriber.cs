using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tuntenfisch.Commons.Input
{
    public class InputActionSubscriber : MonoBehaviour
    {
        private static Dictionary<InputAction, int> s_subscriberCounts = new Dictionary<InputAction, int>();

        [SerializeField]
        private InputActionReference[] m_subscribedInputActions;

        private Dictionary<InputAction, Action<InputAction.CallbackContext>> m_subscriptions = new Dictionary<InputAction, Action<InputAction.CallbackContext>>();

        private void Start()
        {
            SubscribeToInputActions(m_subscribedInputActions);
        }

        private void OnDestroy()
        {
            UnsubscribeFromInputActions(m_subscribedInputActions);
        }

        private void SubscribeToInputActions(InputActionReference[] actions)
        {
            foreach (InputActionReference inputActionReference in actions)
            {
                InputAction inputAction = inputActionReference.action;
                Action<InputAction.CallbackContext> callback = (context) => gameObject.SendMessage($"On{inputAction.name.Replace(" ", "")}", context, SendMessageOptions.RequireReceiver);
                inputAction.started += callback;
                inputAction.performed += callback;
                inputAction.canceled += callback;
                m_subscriptions[inputAction] = callback;

                if (!s_subscriberCounts.ContainsKey(inputAction))
                {
                    s_subscriberCounts.Add(inputAction, 1);
                    inputAction.Enable();
                }
                else
                {
                    s_subscriberCounts[inputAction]++;
                }
            }
        }

        private void UnsubscribeFromInputActions(InputActionReference[] actions)
        {
            foreach (InputActionReference inputActionReference in actions)
            {
                InputAction inputAction = inputActionReference.action;
                Action<InputAction.CallbackContext> callback = m_subscriptions[inputAction];
                inputAction.started -= callback;
                inputAction.performed -= callback;
                inputAction.canceled -= callback;
                m_subscriptions.Remove(inputAction);

                if (s_subscriberCounts[inputAction] == 1)
                {
                    s_subscriberCounts.Remove(inputAction);
                    inputAction.Disable();
                }
                else
                {
                    s_subscriberCounts[inputAction]--;
                }
            }
        }
    }
}