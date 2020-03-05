using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using Alloy.BufferObjects;

namespace Alloy.Assets
{
    public class RenderTexture : Texture
    {
        private FBO fbo;
        private int depthTex;

        public RenderTexture(int width, int height, Filter filterMode = Filter.Linear, Wrapping wrapMode = Wrapping.Repeat, string path = "") : base(path, false)
        {
            fbo = new FBO();
            fbo.Bind();

            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);

            depthTex = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, depthTex);
            GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)All.DepthComponent32, width, height, 0, PixelFormat.DepthComponent, PixelType.UnsignedInt, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            var metaData = LoadMetaData();
            SetFilter(metaData.Count >= 1 ? (Filter)Convert.ToInt32(metaData[0].value) : filterMode, false);
            SetWrapping(metaData.Count >= 2 ? (Wrapping)Convert.ToInt32(metaData[1].value) : wrapMode);

            fbo.AttachTexture(Handle);
            fbo.AttachDepthTexture(depthTex);

            if (GL.Ext.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                Logging.LogWarning(this, "Framebuffer is not complete!");
            fbo.Unbind();
        }

        public void Bind()
        {
            fbo.Bind();
        }
        public void Unbind()
        {
            fbo.Unbind();
        }
    }
}
