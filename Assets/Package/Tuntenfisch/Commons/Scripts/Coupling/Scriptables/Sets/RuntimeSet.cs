using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Sets
{
    public abstract class RuntimeSet<T> : ScriptableObject, IEnumerable<T>, ISerializationCallbackReceiver
    {
        #region Events
        public event Action<T> OnElementAdded;
        public event Action<T> OnElementRemoved;
        #endregion

        #region Inspector Variables
        [SerializeField]
        private List<T> m_serializedSet = new List<T>();
        #endregion

        #region Private Variables
        private HashSet<T> m_set = new HashSet<T>();
        private List<(Operator, T)> m_doLater = new List<(Operator, T)>();
        private bool m_isIterating;
        #endregion

        #region Public Methods
        public IEnumerator<T> GetEnumerator()
        {
            m_isIterating = true;

            try
            {
                foreach (T element in m_set)
                {
                    yield return element;
                }
            }
            finally
            {
                m_isIterating = false;

                foreach ((Operator @operator, T element) in m_doLater)
                {
                    switch (@operator)
                    {
                        case Operator.Add:
                            Add(element);
                            break;

                        case Operator.Remove:
                            Remove(element);
                            break;
                    }
                }
                m_doLater.Clear();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void OnBeforeSerialize()
        {
            m_serializedSet.Clear();
            m_serializedSet.AddRange(m_set);
        }

        public void OnAfterDeserialize()
        {
            m_set.Clear();
            m_set.UnionWith(m_serializedSet);
        }

        public bool Contains(T element)
        {
            return m_set.Contains(element);
        }

        public void Add(T element)
        {
            if (!m_set.Contains(element))
            {
                // We shouldn't directly add elements as we iterate over the set.
                if (m_isIterating)
                {
                    m_doLater.Add((Operator.Add, element));
                }
                else
                {
                    m_set.Add(element);
                    OnElementAdded?.Invoke(element);
                }
            }
            else
            {
                Debug.LogWarning($"Tried to add an {nameof(element)} to the {nameof(RuntimeSet<T>)} \"{name}\" that is already contained in the set.");
            }
        }

        public void Remove(T element)
        {
            if (m_set.Contains(element))
            {
                // We cannot directly remove elements as we iterate over the set.
                if (m_isIterating)
                {
                    m_doLater.Add((Operator.Remove, element));
                }
                else
                {
                    m_set.Remove(element);
                    OnElementRemoved?.Invoke(element);
                }
            }
            else
            {
                Debug.LogWarning($"Tried to remove an {nameof(element)} from the {nameof(RuntimeSet<T>)} \"{name}\" that is not contained in the set.");
            }
        }
        #endregion

        private enum Operator
        {
            Add,
            Remove
        }
    }
}