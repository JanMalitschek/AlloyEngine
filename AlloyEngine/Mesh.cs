using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Alloy.BufferObjects;

namespace Alloy
{
    public struct Vertex
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector2 uv;
        public Vector4 color;
    }

    class SubMesh
    {
        public List<uint> indices;
        public VAO vao { get; private set; }
        public EBO ebo { get; private set; }

        public SubMesh()
        {
            indices = new List<uint>();
        }
        public void GenerateBuffers(Mesh mesh)
        {
            vao = new VAO();
            vao.Bind();
            mesh.vbo.Bind();
            mesh.vbo.LinkAttributes();
            ebo = new EBO(this, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
            vao.Unbind();
        }
    }

    class Mesh
    {
        public string name;
        public List<Vertex> vertices;
        public List<SubMesh> subMeshes;
        public VBO vbo { get; private set; }

        public Mesh(string name = "Mesh")
        {
            this.name = name;
            vertices = new List<Vertex>();
            subMeshes = new List<SubMesh>();
        }
        public void GenerateBuffers()
        {
            vbo = new VBO(this, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
        }

        public IEnumerator<float> GetPositions()
        {
            foreach (Vertex v in vertices)
            {
                yield return v.position.X;
                yield return v.position.Y;
                yield return v.position.Z;
            }
        }
        public IEnumerator<float> GetNormals()
        {
            foreach (Vertex v in vertices)
            {
                yield return v.normal.X;
                yield return v.normal.Y;
                yield return v.normal.Z;
            }
        }
        public IEnumerator<float> GetUVs()
        {
            foreach (Vertex v in vertices)
            {
                yield return v.uv.X;
                yield return v.uv.Y;
            }
        }
        public IEnumerator<float> GetColors()
        {
            foreach (Vertex v in vertices)
            {
                yield return v.color.X;
                yield return v.color.Y;
                yield return v.color.Z;
                yield return v.color.W;
            }
        }
        public IEnumerator<float> GetVerticesAsFloatArray()
        {
            foreach (Vertex v in vertices)
            {
                yield return v.position.X;
                yield return v.position.Y;
                yield return v.position.Z;

                yield return v.normal.X;
                yield return v.normal.Y;
                yield return v.normal.Z;

                yield return v.uv.X;
                yield return v.uv.Y;

                yield return v.color.X;
                yield return v.color.Y;
                yield return v.color.Z;
                yield return v.color.W;
            }
        }
    }

    
}
