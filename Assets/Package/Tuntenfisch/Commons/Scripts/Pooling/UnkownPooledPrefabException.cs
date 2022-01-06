using System;
using UnityEngine;

namespace Tuntenfisch.Commons.Pooling
{
    public class UnkownPooledPrefabException : Exception
    {
        public UnkownPooledPrefabException()
        {

        }

        public UnkownPooledPrefabException(string message) : base(message)
        {

        }

        public UnkownPooledPrefabException(string message, Exception inner) : base(message, inner)
        {

        }

        public UnkownPooledPrefabException(GameObject prefab) : base($"The prefab \"{prefab.name}\" is not known to this pool.")
        {

        }
    }
}