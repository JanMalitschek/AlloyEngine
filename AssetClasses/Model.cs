using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK;

namespace Alloy.Assets
{
    class Model : Asset
    {
        public List<Mesh> meshes;

        public Model(string path) : base(path)
        {
            meshes = new List<Mesh>();
            List<Vector3> vertexPositions = new List<Vector3>();
            List<Vector2> texCoords = new List<Vector2>();
            List<Vector3> faceNormals = new List<Vector3>();
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    while(reader.Peek() >= 0)
                    {
                        string line = reader.ReadLine();
                        string[] splitLine = line.Split(' ');
                        if(splitLine.Length > 0) {
                            switch (splitLine[0])
                            {
                                case "o": //start new mesh
                                    vertexPositions = new List<Vector3>();
                                    meshes.Add(new Mesh() { name = splitLine[1] });
                                    break;
                                case "v": //vertex position
                                    vertexPositions.Add(new Vector3(Convert.ToSingle(splitLine[1]),
                                                                    Convert.ToSingle(splitLine[2]),
                                                                    Convert.ToSingle(splitLine[3])));
                                    break;
                                case "vt": //uv
                                    texCoords.Add(new Vector2(Convert.ToSingle(splitLine[1]),
                                                              Convert.ToSingle(splitLine[2])));
                                    break;
                                case "vn": //face normal
                                    faceNormals.Add(new Vector3(Convert.ToSingle(splitLine[1]),
                                                                Convert.ToSingle(splitLine[2]),
                                                                Convert.ToSingle(splitLine[3])));
                                    break;
                                case "usemtl": //start new submesh
                                    meshes.Last().subMeshes.Add(new SubMesh());
                                    break;
                                case "f": //face indices
                                    for(int i = 1; i < 4; i++)
                                    {
                                        var vertInfo = splitLine[i].Split('/');
                                        meshes.Last().subMeshes.Last().vertices.Add(new Vertex
                                        {
                                            position = vertexPositions[Convert.ToInt32(vertInfo[0]) - 1],
                                            normal = faceNormals[Convert.ToInt32(vertInfo[2]) - 1],
                                            uv = texCoords[Convert.ToInt32(vertInfo[1]) - 1],
                                            color = Vector4.One
                                        });
                                    }
                                    break;
                            }

                        }
                    }
                }
            }
            catch(Exception e)
            {
                Logging.LogWarning(this, e.Message);
            }
        }
    }
}
