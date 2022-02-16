using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Tuntenfisch.Commons.URP
{
    // Based on https://gist.github.com/alexanderameye/bb4ec2798a2d101ad505ce4f7a0f58f4.
    public class CopyDepthBufferPass : ScriptableRenderPass
    {
        #region Private Fields
        private CopyDepthBufferFeature.PassProperties m_passProperties;
        private Material m_material;
        #endregion

        #region Public Methods
        public CopyDepthBufferPass(CopyDepthBufferFeature.PassProperties passSettings)
        {
            m_passProperties = passSettings;
            renderPassEvent = passSettings.RenderPassEvent;

            if (m_material == null)
            {
                m_material = CoreUtils.CreateEngineMaterial("Tuntenfisch/Commons/Misc/Copy Depth");
            }
        }

        public override void OnCameraSetup(CommandBuffer commandBuffer, ref RenderingData renderingData)
        {
            ConfigureInput(ScriptableRenderPassInput.Depth);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer commandBuffer = CommandBufferPool.Get();
            Blit(commandBuffer, renderingData.cameraData.renderer.cameraDepthTarget, m_passProperties.TargetRenderTexture, m_material);
            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }
        #endregion
    }
}