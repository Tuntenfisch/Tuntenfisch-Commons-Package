using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tuntenfisch.Commons.Serializables
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        #region Inspector Fields
        [SerializeField]
        private List<SerializableKeyValuePair> m_keyValuePairs = new List<SerializableKeyValuePair>();
        [SerializeField]
        private SerializableKeyValuePair m_keyValuePair;
        #endregion

        #region public Methods
        public void OnAfterDeserialize()
        {
            Clear();
            
            foreach (SerializableKeyValuePair pair in m_keyValuePairs)
            {
                this[pair.Key] = pair.Value;
            }
        }

        public void OnBeforeSerialize()
        {
            m_keyValuePairs.Clear();
            
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                m_keyValuePairs.Add(new SerializableKeyValuePair { Key = pair.Key, Value = pair.Value });
            }
        }
        #endregion

        #region Private Structs, Classes and Enums
        [Serializable]
        private struct SerializableKeyValuePair
        {
            #region Public Fields
            public TKey Key;
            public TValue Value;
            #endregion
        }
        #endregion
    }
}