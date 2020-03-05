using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Alloy.BufferObjects
{
    public class RBO : BufferObject
    {
        public RBO(int width, int height)
        {
            buffer = GL.GenBuffer();
            Bind();
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, width, height);
            Unbind();
        }

        public override void Bind()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, buffer);
        }

        public override void Unbind()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        }
    }
}
