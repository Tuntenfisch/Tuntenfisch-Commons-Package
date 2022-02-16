using Tuntenfisch.Commons.Coupling.Variables;
using Unity.Mathematics;
using UnityEngine;

namespace Tuntenfisch.Commons.Environment
{
    [ExecuteAlways]
    public class Wind : MonoBehaviour
    {
        #region Inspector Fields
        [SerializeField]
        private VariableWriteReference<WindProperties> m_windProperties;
        [Min(0.0f)]
        [SerializeField]
        private float m_speed = 1.0f;
        [Min(0.0f)]
        [SerializeField]
        private float m_frequency = 0.25f;
        #endregion

        #region Unity Callbacks
        private void OnEnable()
        {
            SetWindProperties();
        }

        private void Update()
        {
            if (transform.hasChanged)
            {
                SetWindProperties();
                transform.hasChanged = false;
            }
        }

        private void OnValidate()
        {
            SetWindProperties();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "WindZone Gizmo", true);
            Gizmos.color = new Color32(0, 170, 228, 255);
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, new float3(1.0f));
            Editor.Gizmos.DrawForwardArrow();
        }
        #endregion

        #region Private Methods
        private void SetWindProperties()
        {
            if (m_windProperties == null)
            {
                return;
            }

            m_windProperties.CurrentValue = new WindProperties
            {
                Rotation = transform.rotation.eulerAngles,
                Speed = m_speed,
                Frequency = m_frequency
            };
        }
        #endregion
    }
}