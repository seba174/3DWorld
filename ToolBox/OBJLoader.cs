using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK;

namespace ToolBox
{
    public class OBJLoader
    {
        private const string baseObjPath = "../../../Resources/";
        private const string type = ".obj";

        public static ModelData LoadObjModel(string fileName)
        {
            var vertices = new List<Vertex>();
            var textures = new List<Vector2>();
            var normals = new List<Vector3>();
            var indices = new List<int>();

            using (var fr = new StreamReader(baseObjPath + fileName + type))
            {
                string line = null;

                while (!fr.EndOfStream)
                {
                    line = fr.ReadLine();
                    var splitedCurrentLine = line.Split(' ');
                    if (line.StartsWith("v "))
                    {
                        Vector3 vertex = new Vector3(float.Parse(splitedCurrentLine[1]),
                            float.Parse(splitedCurrentLine[2]), float.Parse(splitedCurrentLine[3]));

                        Vertex newVertex = new Vertex(vertices.Count, vertex);
                        vertices.Add(newVertex);
                    }
                    else if (line.StartsWith("vt "))
                    {
                        Vector2 texture = new Vector2(float.Parse(splitedCurrentLine[1]), float.Parse(splitedCurrentLine[2]));
                        textures.Add(texture);
                    }
                    else if (line.StartsWith("vn "))
                    {
                        Vector3 normal = new Vector3(float.Parse(splitedCurrentLine[1]),
                            float.Parse(splitedCurrentLine[2]), float.Parse(splitedCurrentLine[3]));
                        normals.Add(normal);
                    }
                    else if (line.StartsWith("f "))
                    {
                        break;
                    }
                }

                while (!fr.EndOfStream)
                {
                    if (!line.StartsWith("f "))
                    {
                        line = fr.ReadLine();
                        continue;
                    }

                    string[] splittedCurrentLine = line.Split(' ');
                    for (int i = 1; i <= 3; i++)
                    {
                        string[] vertex = splittedCurrentLine[i].Split('/');
                        ProcessVertex(vertex, vertices, indices);
                    }

                    line = fr.ReadLine();
                }
            }

            RemoveUnusedVertices(vertices);

            float[] verticesArray = vertices.Select(v => v.Position).SelectMany(p => new float[] { p.X, p.Y, p.Z }).ToArray();
            float[] texturesArray = vertices.Select(v => v.TextureIndex).Select(i => textures[i]).SelectMany(t => new float[] { t.X, 1 - t.Y }).ToArray();
            float[] normalsArray = vertices.Select(v => v.NormalIndex).Select(i => normals[i]).SelectMany(n => new float[] { n.X, n.Y, n.Z }).ToArray();
            int[] indicesArray = indices.ToArray();
            float furthestPoint = vertices.Max(v => v.Length);

            ModelData data = new ModelData(verticesArray, texturesArray, normalsArray, indicesArray, furthestPoint);
            return data;
        }

        private static void RemoveUnusedVertices(List<Vertex> vertices)
        {
            foreach (var vertex in vertices)
            {
                if (!vertex.IsSet())
                {
                    vertex.TextureIndex = 0;
                    vertex.NormalIndex = 0;
                }
            }
        }

        private static void ProcessVertex(string[] vertexData, List<Vertex> vertices, List<int> indices)
        {
            int index = int.Parse(vertexData[0]) - 1;
            Vertex currentVertex = vertices[index];
            int textureIndex = int.Parse(vertexData[1]) - 1;
            int normalIndex = int.Parse(vertexData[2]) - 1;

            if (!currentVertex.IsSet())
            {
                currentVertex.TextureIndex = textureIndex;
                currentVertex.NormalIndex = normalIndex;
                indices.Add(index);
            }
            else
            {
                DealWithAlreadyProcessedVertex(currentVertex, textureIndex, normalIndex, indices, vertices);
            }
        }


        private static void DealWithAlreadyProcessedVertex(Vertex previousVertes, int newTextureIndex, int newNormalIndex,
            List<int> indices, List<Vertex> vertices)
        {
            if (previousVertes.HasSameTextureAndNormal(newTextureIndex, newNormalIndex))
            {
                indices.Add(previousVertes.Index);
            }
            else
            {
                Vertex anotherVertex = previousVertes.DuplicateVertex;
                if (anotherVertex != null)
                {
                    DealWithAlreadyProcessedVertex(anotherVertex, newTextureIndex, newNormalIndex, indices, vertices);
                }
                else
                {
                    Vertex duplicateVertex = new Vertex(vertices.Count, previousVertes.Position)
                    {
                        TextureIndex = newTextureIndex,
                        NormalIndex = newNormalIndex
                    };
                    duplicateVertex.DuplicateVertex = duplicateVertex;
                    vertices.Add(duplicateVertex);
                    indices.Add(duplicateVertex.Index);
                }
            }
        }
    }
}
