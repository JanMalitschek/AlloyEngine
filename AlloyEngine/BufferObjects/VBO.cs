using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Alloy.BufferObjects
{
    class VBO : BufferObject
    {
        public VBO(Mesh mesh, BufferUsageHint mode)
        {
            //Create VBO
            buffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            //Load Data
            var vertexEnum = mesh.GetVerticesAsFloatArray();
            List<float> vertexData = new List<float>();
            while (vertexEnum.MoveNext())
                vertexData.Add(vertexEnum.Current);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Count * sizeof(float), vertexData.ToArray(), mode);
            Unbind();
        }

        ~VBO()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(buffer);
        }

        public override void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
        }
        public override void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void LinkAttributes()
        {
            //Set up Vertex Attribute Pointers
            //Position
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12 * sizeof(float), 0);
            //Normal
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 12 * sizeof(float), 3 * sizeof(float));
            //UV
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 12 * sizeof(float), 6 * sizeof(float));
            //Color
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 12 * sizeof(float), 8 * sizeof(float));
        }
    }
}
