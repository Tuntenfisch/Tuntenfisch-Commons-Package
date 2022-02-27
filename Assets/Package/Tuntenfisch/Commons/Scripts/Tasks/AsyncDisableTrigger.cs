using System.Threading;
using UnityEngine;

namespace Tuntenfisch.Commons.Tasks
{
    [DisallowMultipleComponent]
    public sealed class AsyncDisableTrigger : MonoBehaviour
    {
        #region Public Properties
        public CancellationToken Token
        {
            get 
            {
                if (m_tokenSource == null)
                {
                    m_tokenSource = new CancellationTokenSource();
                }
                return m_tokenSource.Token;
            }
        }
        #endregion

        #region Private Fields
        private CancellationTokenSource m_tokenSource;
        #endregion

        #region Unity Callbacks
        private void OnDisable()
        {
            m_tokenSource?.Cancel();
            m_tokenSource?.Dispose();
            m_tokenSource = null;
        }

        private void OnDestroy()
        {
            m_tokenSource?.Cancel();
            m_tokenSource?.Dispose();
            m_tokenSource = null;
        }
        #endregion
    }

    public static partial class AsyncTriggerExtensions
    {
        #region Public Methods
        public static CancellationToken GetCancellationTokenOnDisable(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncDisableTrigger>(gameObject).Token;
        }
        #endregion
    }
}