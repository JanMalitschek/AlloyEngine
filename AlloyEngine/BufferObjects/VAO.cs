using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Alloy.BufferObjects
{
    class VAO : BufferObject
    {
        public VAO()
        {
            buffer = GL.GenVertexArray();
        }
        ~VAO()
        {
            GL.DeleteVertexArray(buffer);
        }

        public override void Bind()
        {
            GL.BindVertexArray(buffer);
        }
        public override void Unbind()
        {
            GL.BindVertexArray(0);
        }
    }
}
