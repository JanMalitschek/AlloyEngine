using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using Alloy.BufferObjects;
using OpenTK.Graphics;

namespace Alloy.Assets
{
    public class RenderTexture : Texture
    {
        private FBO fbo;
        public int depthStencil { get; private set; }

        public RenderTexture(int width, int height, Filter filterMode = Filter.Linear, Wrapping wrapMode = Wrapping.Repeat, string path = "") : base(path, false)
        {
            fbo = new FBO();
            fbo.Bind();

            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);

            depthStencil = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, depthStencil);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Depth24Stencil8, width, height, 0, PixelFormat.DepthStencil, PixelType.UnsignedInt248, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            var metaData = LoadMetaData();
            SetFilter(metaData.Count >= 1 ? (Filter)Convert.ToInt32(metaData[0].value) : filterMode, false);
            SetWrapping(metaData.Count >= 2 ? (Wrapping)Convert.ToInt32(metaData[1].value) : wrapMode);

            fbo.AttachTexture(Handle);
            fbo.AttachDepthStencilTexture(depthStencil);

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

        public static Color4 ReadColor(int x, int y)
        {
            byte[] index = new byte[3];
            GL.ReadPixels(x, y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, index);
            return new Color4(index[0], index[1], index[2], 255);
        }

        public static byte ReadStencil(int x, int y)
        {
            byte[] index = new byte[1];
            GL.ReadPixels(x, y, 1, 1, PixelFormat.StencilIndex, PixelType.UnsignedByte, index);
            return index[0];
        }
    }
}
