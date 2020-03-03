using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK;
using System.Xml;
using System.Globalization;

namespace Alloy.Assets
{
    public class Model : Asset
    {
        public List<Mesh> meshes;

        public Model(string path) : base(path)
        {
            meshes = new List<Mesh>();
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(path);
                XmlNodeList geometries = xml.SelectNodes("//*[local-name()='COLLADA']/*[local-name()='library_geometries']/*[local-name()='geometry']");
                foreach(XmlNode g in geometries)
                {
                    meshes.Add(new Mesh(g.Attributes["name"].Value));
                    Mesh currentMesh = meshes.Last();
                    //tempData
                    List<Vector3> vertexPositions = new List<Vector3>();
                    List<Vector3> normals = new List<Vector3>();
                    List<Vector2> texCoords = new List<Vector2>();
                    List<Vector4> vertexColors = new List<Vector4>();
                    //Load Raw Data
                    XmlNodeList sources = g.SelectNodes("*[local-name()='mesh']/*[local-name()='source']");
                    foreach(XmlNode s in sources)
                    {
                        if (s.Attributes["id"].Value.Contains("positions"))
                        {
                            XmlNode array = s.SelectSingleNode("*[local-name()='float_array']");
                            string[] asciiPositionData = array.InnerText.Split(' ');
                            int numVerts = asciiPositionData.Length / 3;
                            for (int i = 0; i < numVerts; i++)
                            {
                                vertexPositions.Add(new Vector3(Convert.ToSingle(asciiPositionData[i * 3], CultureInfo.InvariantCulture),
                                                                Convert.ToSingle(asciiPositionData[i * 3 + 1], CultureInfo.InvariantCulture),
                                                                Convert.ToSingle(asciiPositionData[i * 3 + 2], CultureInfo.InvariantCulture)));
                                currentMesh.vertices.Add(new Vertex());
                            }
                        }
                        else if (s.Attributes["id"].Value.Contains("normals"))
                        {
                            XmlNode array = s.SelectSingleNode("*[local-name()='float_array']");
                            string[] asciiNormalData = array.InnerText.Split(' ');
                            int numVerts = asciiNormalData.Length / 3;
                            for (int i = 0; i < numVerts; i++)
                            {
                                normals.Add(new Vector3(Convert.ToSingle(asciiNormalData[i * 3], CultureInfo.InvariantCulture),
                                                        Convert.ToSingle(asciiNormalData[i * 3 + 1], CultureInfo.InvariantCulture),
                                                        Convert.ToSingle(asciiNormalData[i * 3 + 2], CultureInfo.InvariantCulture)));
                            }
                        }
                        else if (s.Attributes["id"].Value.Contains("map"))
                        {
                            XmlNode array = s.SelectSingleNode("*[local-name()='float_array']");
                            string[] asciiUVData = array.InnerText.Split(' ');
                            int numVerts = asciiUVData.Length / 2;
                            for (int i = 0; i < numVerts; i++)
                            {
                                texCoords.Add(new Vector2(Convert.ToSingle(asciiUVData[i * 2], CultureInfo.InvariantCulture),
                                                          Convert.ToSingle(asciiUVData[i * 2 + 1], CultureInfo.InvariantCulture)));
                            }
                        }
                        else if (s.Attributes["id"].Value.Contains("colors"))
                        {
                            XmlNode array = s.SelectSingleNode("*[local-name()='float_array']");
                            string[] asciiColorData = array.InnerText.Split(' ');
                            int numVerts = asciiColorData.Length / 4;
                            for (int i = 0; i < numVerts; i++)
                            {
                                vertexColors.Add(new Vector4(Convert.ToSingle(asciiColorData[i * 4], CultureInfo.InvariantCulture),
                                                             Convert.ToSingle(asciiColorData[i * 4 + 1], CultureInfo.InvariantCulture),
                                                             Convert.ToSingle(asciiColorData[i * 4 + 2], CultureInfo.InvariantCulture),
                                                             Convert.ToSingle(asciiColorData[i * 4 + 3], CultureInfo.InvariantCulture)));
                            }
                        }
                    }
                    //Organize Vertices and Indices
                    XmlNodeList triangles = g.SelectNodes("*[local-name()='mesh']/*[local-name()='triangles']");
                    foreach(XmlNode n in triangles)
                    {
                        currentMesh.subMeshes.Add(new SubMesh());
                        SubMesh currentSubMesh = currentMesh.subMeshes.Last();
                        string[] asciiIndexData = n.SelectSingleNode("*[local-name()='p']").InnerText.Split(' ');
                        int stride = vertexColors.Count > 0 ? 4 : 3;
                        int numIndices = asciiIndexData.Length / stride;

                        List<int>[] indexedVertices = new List<int>[vertexPositions.Count];
                        for(int i = 0; i < numIndices; i++)
                        {
                            int ind = Convert.ToInt32(asciiIndexData[i * stride]);
                            currentSubMesh.indices.Add((uint)ind);
                            Vertex currentVertex = currentMesh.vertices[ind];
                            currentVertex.position = vertexPositions[ind];
                            currentVertex.normal = normals[Convert.ToInt32(asciiIndexData[i * stride + 1])];
                            if (vertexColors.Count > 0)
                                currentVertex.color = vertexColors[Convert.ToInt32(asciiIndexData[i * stride + 3])];
                            else
                                currentVertex.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

                            int uvInd = Convert.ToInt32(asciiIndexData[i * stride + 2]);
                            currentVertex.uv = texCoords[uvInd];
                            //Has this vertex been indexed before?
                            if (indexedVertices[ind] == null) {
                                indexedVertices[ind] = new List<int>();
                                indexedVertices[ind].Add(uvInd);
                                currentMesh.vertices[ind] = currentVertex;
                            }
                            else
                            {
                                //The Referenced uvIndex is a new one
                                if (!indexedVertices[ind].Contains(uvInd)){
                                    currentMesh.vertices.Add(currentVertex);
                                    currentSubMesh.indices[i] = (uint)(currentMesh.vertices.Count - 1);
                                }
                                else
                                    currentMesh.vertices[ind] = currentVertex;
                                indexedVertices[ind].Add(uvInd);
                            }
                        }
                        
                    }

                }
                foreach(Mesh m in meshes)
                {
                    m.GenerateBuffers();
                    foreach (SubMesh sm in m.subMeshes)
                        sm.GenerateBuffers(m);
                }
            }
            catch(Exception e)
            {
                Logging.LogWarning(this, e.Message);
            }
        }

        protected override void SaveMetaData(out List<MetaDataEntry> metaData)
        {
            metaData = new List<MetaDataEntry>();
        }
    }
}
