using System;
using UnityEngine;

namespace Tuntenfisch.Commons.Pooling
{
    public class UnknownPooledPrefabException : Exception
    {
        #region Public Methods
        public UnknownPooledPrefabException()
        {

        }

        public UnknownPooledPrefabException(string message) : base(message)
        {

        }

        public UnknownPooledPrefabException(string message, Exception inner) : base(message, inner)
        {

        }

        public UnknownPooledPrefabException(GameObject prefab) : base($"The prefab \"{prefab.name}\" is not known to this pool.")
        {

        }
        #endregion
    }
}