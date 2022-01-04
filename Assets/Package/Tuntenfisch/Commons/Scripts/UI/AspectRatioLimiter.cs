using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Tuntenfisch.Commons.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(AspectRatioFitter))]
    public class AspectRatioLimiter : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField]
        private float m_maxAspectRatio = 16.0f / 9.0f;
        [Range(1, 60)]
        [SerializeField]
        private int m_validationFrequency = 5;
        #endregion

        #region Private Variables
        private AspectRatioFitter m_aspectRatioFitter;
        private int m_delayInMilliseconds;
        private int2 m_oldScreenSize;
        private int2 m_newScreenSize;
        #endregion

        # region Unity Callbacks
        private void Start()
        {
            m_aspectRatioFitter = GetComponent<AspectRatioFitter>();
            m_aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            m_delayInMilliseconds = (int)math.round(1000.0f / m_validationFrequency);
            UniTask.Void(() => LimitAspectRatio(gameObject.GetCancellationTokenOnDestroy()));
        }

        private void OnValidate()
        {
            m_delayInMilliseconds = (int)math.round(1000.0f / m_validationFrequency);
        }
        #endregion

        #region Private Methods
        private async UniTaskVoid LimitAspectRatio(CancellationToken token)
        {
            while(!token.IsCancellationRequested)
            {
                m_newScreenSize = new int2(Screen.width, Screen.height);

                if (math.any(m_oldScreenSize != m_newScreenSize))
                {
                    float aspectRatio = (float)m_newScreenSize.x / m_newScreenSize.y;
                    m_aspectRatioFitter.aspectRatio = aspectRatio < m_maxAspectRatio ? aspectRatio : m_maxAspectRatio;
                    m_oldScreenSize = m_newScreenSize;
                }
                await UniTask.Delay(m_delayInMilliseconds);
            }
        }
        #endregion
    }
}