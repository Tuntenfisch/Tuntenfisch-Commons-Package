﻿using UnityEngine;

namespace Tuntenfisch.Commons.Tasks
{
    public static partial class AsyncTriggerExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (!gameObject.TryGetComponent(out T component))
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }
    }
}