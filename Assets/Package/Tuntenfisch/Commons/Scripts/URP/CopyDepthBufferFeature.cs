using System;
using Tuntenfisch.Commons.Attributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Tuntenfisch.Commons.URP
{
    // Based on https://gist.github.com/alexanderameye/20914089079069eaeb144c1e17821aa3.
    public class CopyDepthBufferFeature : ScriptableRendererFeature
    {
        [Serializable]
        public class PassProperties
        {
            #region Public Properties
            public RenderPassEvent RenderPassEvent => m_renderPassEvent;
            public RenderTexture TargetRenderTexture => m_targetRenderTexture;
            #endregion

            #region Private Fields
            private const RenderPassEvent m_renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
            #endregion

            #region Inspector Fields
            [SerializeField]
            private RenderTexture m_targetRenderTexture;
            #endregion
        }

        #region Private Fields
        private CopyDepthBufferPass m_pass;
        #endregion

        #region Inspector Fields
        [InlineField]
        [SerializeField]
        private PassProperties m_passProperties = new PassProperties();
        #endregion

        #region Public Methods
        public override void Create()
        {
            m_pass = new CopyDepthBufferPass(m_passProperties);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(m_pass);
        }
        #endregion
    }
}