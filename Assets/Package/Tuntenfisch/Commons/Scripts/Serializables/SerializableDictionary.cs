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
        private List<TKey> m_keys;
        [SerializeField]
        private List<TValue> m_values;
        #endregion

        #region public Methods
        public void OnAfterDeserialize()
        {
            Clear();
            
            if (m_keys.Count != m_values.Count)
            {
                throw new Exception("Key count didn't match value count during deserialization. Ensure key and value types are serializable.");
            }

            for (int index = 0; index < m_keys.Count; index++)
            {
                this[m_keys[index]] = m_values[index];
            }
        }

        public void OnBeforeSerialize()
        {
            m_keys.Clear();
            m_values.Clear();

            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                m_keys.Add(pair.Key);
                m_values.Add(pair.Value);
            }
        }
        #endregion
    }
}