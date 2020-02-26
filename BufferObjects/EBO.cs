using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Alloy.BufferObjects
{
    class EBO : BufferObject
    {
        public EBO(SubMesh subMesh, BufferUsageHint mode)
        {
            buffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, subMesh.indices.Count * sizeof(uint), subMesh.indices.ToArray(), mode);
        }
        ~EBO()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(buffer);
        }

        public override void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer);
        }
        public override void Unbind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
    }
}
