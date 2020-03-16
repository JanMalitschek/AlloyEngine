using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Alloy.BufferObjects
{
    public class FBO : BufferObject
    {
        public FBO()
        {
            buffer = GL.Ext.GenFramebuffer();
        }

        public void AttachTexture(int textureHandle)
        {
            GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D, textureHandle, 0);
        }
        public void AttachDepthStencilTexture(int textureHandle)
        {
            GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, textureHandle, 0);
        }

        public void AttachRenderbuffer(RBO rbo)
        {
            GL.Ext.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, rbo.buffer);
        }

        public override void Bind()
        {
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, buffer);
        }

        public override void Unbind()
        {
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
        }
    }
}
