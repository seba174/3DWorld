using System.Collections.Generic;
using Entities;
using OpenTK;
using ToolBox;

namespace Shaders
{
    public class BaseShader : ShaderProgram
    {
        private int location_transformationMatrix;
        private int location_projectionMatrix;
        private int location_viewMatrix;
        private int[] location_lightPosition;
        private int[] location_lightColour;
        private int location_shineDamper;
        private int location_reflectivity;
        private int location_skyColour;


        public BaseShader(string vertexFile, string fragmentFile) : base(vertexFile, fragmentFile)
        {
        }

        protected override void BindAttributes()
        {
            BindAttribute(0, "position");
            BindAttribute(1, "textureCoords");
            BindAttribute(2, "normal");
        }

        protected override void GetAllUniformLocations()
        {
            location_transformationMatrix = GetUniformLocation("transformationMatrix");
            location_projectionMatrix = GetUniformLocation("projectionMatrix");
            location_viewMatrix = GetUniformLocation("viewMatrix");
            location_shineDamper = GetUniformLocation("shineDamper");
            location_reflectivity = GetUniformLocation("reflectivity");
            location_skyColour = GetUniformLocation("skyColour");

            location_lightPosition = new int[MaxLights];
            location_lightColour = new int[MaxLights];
            for (int i = 0; i < MaxLights; i++)
            {
                location_lightPosition[i] = GetUniformLocation("lightPosition[" + i + "]");
                location_lightColour[i] = GetUniformLocation("lightColour[" + i + "]");
            }
        }

        public void LoadSkyColour(Vector3 color)
        {
            LoadVector(location_skyColour, color);
        }

        public void LoadTransformationMatrix(Matrix4 matrix)
        {
            LoadMatrix(location_transformationMatrix, matrix);
        }

        public void LoadProjectionMatrix(Matrix4 matrix)
        {
            LoadMatrix(location_projectionMatrix, matrix);
        }

        public void LoadViewMatrix(BaseCamera camera)
        {
            Matrix4 viewMatrix = Maths.CreateViewMatrix(camera);
            LoadMatrix(location_viewMatrix, viewMatrix);
        }

        public void LoadLights(List<Light> lights)
        {
            for (int i = 0; i < MaxLights; i++)
            {
                if (i < lights.Count)
                {
                    LoadVector(location_lightPosition[i], lights[i].Position);
                    LoadVector(location_lightColour[i], lights[i].Colour);
                }
                else
                {
                    LoadVector(location_lightPosition[i], new Vector3(0, 0, 0));
                    LoadVector(location_lightColour[i], new Vector3(0, 0, 0));
                }
            }
        }

        public void LoadShineVariables(float damper, float reflectivity)
        {
            LoadFloat(location_shineDamper, damper);
            LoadFloat(location_reflectivity, reflectivity);
        }
    }
}
