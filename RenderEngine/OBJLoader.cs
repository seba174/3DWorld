using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK;
using ToolBox;

namespace RenderEngine
{
    public class OBJLoader
    {
        private const string baseObjPath = "../../../Resources/";
        private const string type = ".obj";

        public static RawModel LoadObjModel(string fileName, Loader loader)
        {
            var vertices = new List<Vector3>();
            var textures = new List<Vector2>();
            var normals = new List<Vector3>();
            var indices = new List<int>();

            float[] texturesArray = null, normalsArray = null, verticesArray = null;
            int[] indicesArray = null;

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
                        vertices.Add(vertex);
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
                        normalsArray = new float[vertices.Count * 3];
                        texturesArray = new float[vertices.Count * 2];
                        break;
                    }
                }

                while (!fr.EndOfStream)
                {
                    if (!line.StartsWith("f "))
                    {
                        continue;
                    }

                    string[] splittedCurrentLine = line.Split(' ');
                    for (int i = 1; i <= 3; i++)
                    {
                        string[] vertex = splittedCurrentLine[i].Split('/');
                        ProcessVertex(vertex, indices, textures, normals, texturesArray, normalsArray);
                    }

                    line = fr.ReadLine();
                }
            }

            verticesArray = vertices.SelectMany(v => new float[] { v.X, v.Y, v.Z }).ToArray();
            indicesArray = indices.ToArray();

            return loader.LoadToVAO(verticesArray, texturesArray, normalsArray, indicesArray);
        }

        private static void ProcessVertex(string[] vertexData, List<int> indices, List<Vector2> textures,
            List<Vector3> normals, float[] textureArray, float[] normalsArray)
        {
            int currentVertexPointer = int.Parse(vertexData[0]) - 1;
            indices.Add(currentVertexPointer);

            Vector2 currentTex = textures[int.Parse(vertexData[1]) - 1];
            textureArray[currentVertexPointer * 2] = currentTex.X;
            textureArray[currentVertexPointer * 2 + 1] = 1 - currentTex.Y;

            Vector3 currentNorm = normals[int.Parse(vertexData[2]) - 1];
            normalsArray[currentVertexPointer * 3] = currentNorm.X;
            normalsArray[currentVertexPointer * 3 + 1] = currentNorm.Y;
            normalsArray[currentVertexPointer * 3 + 2] = currentNorm.Z;
        }
    }
}
