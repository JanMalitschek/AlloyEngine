using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

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
        public List<Vertex> vertices;
        public List<int> indices;

        public SubMesh()
        {
            vertices = new List<Vertex>();
            indices = new List<int>();
        }
    }

    class Mesh
    {
        public string name;
        public List<SubMesh> subMeshes;

        public Mesh(string name = "Mesh")
        {
            this.name = name;
            subMeshes = new List<SubMesh>();
        }
    }

    
}
