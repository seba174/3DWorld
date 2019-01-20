using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using OpenTK;
using ToolBox;

namespace Shaders
{
    public class TerrainShader : ShaderProgram
    {
        private static string VertexFile = basePath + "TerrainVertexShader.glsl";
        private static string FragmentFile = basePath + "TerrainFragmentShader.glsl";

        private int location_transformationMatrix;
        private int location_projectionMatrix;
        private int location_viewMatrix;
        private int location_lightPosition;
        private int location_lightColour;
        private int location_shineDamper;
        private int location_reflectivity;
        private int location_skyColour;

        private int location_backgroundTexture;
        private int location_rTexture;
        private int location_gTexture;
        private int location_bTexture;
        private int location_blendMap;


        public TerrainShader() : base(VertexFile, FragmentFile)
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
            location_lightPosition = GetUniformLocation("lightPosition");
            location_lightColour = GetUniformLocation("lightColour");
            location_shineDamper = GetUniformLocation("shineDamper");
            location_reflectivity = GetUniformLocation("reflectivity");
            location_skyColour = GetUniformLocation("skyColour");

            location_backgroundTexture = GetUniformLocation("backgroundTexture");
            location_rTexture = GetUniformLocation("rTexture");
            location_gTexture = GetUniformLocation("gTexture");
            location_bTexture = GetUniformLocation("bTexture");
            location_blendMap = GetUniformLocation("blendMap");
        }

        public void ConnectTextureUnits()
        {
            LoadInt(location_backgroundTexture, 0);
            LoadInt(location_rTexture, 1);
            LoadInt(location_gTexture, 2);
            LoadInt(location_bTexture, 3);
            LoadInt(location_blendMap, 4);
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

        public void LoadLight(Light light)
        {
            LoadVector(location_lightColour, light.Colour);
            LoadVector(location_lightPosition, light.Position);
        }

        public void LoadShineVariables(float damper, float reflectivity)
        {
            LoadFloat(location_shineDamper, damper);
            LoadFloat(location_reflectivity, reflectivity);
        }
    }
}
