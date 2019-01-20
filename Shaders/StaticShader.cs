﻿using Entities;
using OpenTK;
using ToolBox;

namespace Shaders
{
    public class StaticShader : ShaderProgram
    {
        private static string VertexFile = basePath + "VertexShader.glsl";
        private static string FragmentFile = basePath + "FragmentShader.glsl";

        private int location_transformationMatrix;
        private int location_projectionMatrix;
        private int location_viewMatrix;
        private int location_lightPosition;
        private int location_lightColour;
        private int location_shineDamper;
        private int location_reflectivity;
        private int location_useFakeLighting;
        private int location_skyColour;
        private int location_numberOfRows;
        private int location_offset;

        public StaticShader() : base(VertexFile, FragmentFile)
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
            location_transformationMatrix =  GetUniformLocation("transformationMatrix");
            location_projectionMatrix = GetUniformLocation("projectionMatrix");
            location_viewMatrix = GetUniformLocation("viewMatrix");
            location_lightPosition = GetUniformLocation("lightPosition");
            location_lightColour = GetUniformLocation("lightColour");
            location_shineDamper = GetUniformLocation("shineDamper");
            location_reflectivity = GetUniformLocation("reflectivity");
            location_useFakeLighting = GetUniformLocation("useFakeLighting");
            location_skyColour = GetUniformLocation("skyColour");
            location_numberOfRows = GetUniformLocation("numberOfRows");
            location_offset = GetUniformLocation("offset");
        }

        public void LoadNumberOfRows(int numberOfRows)
        {
            LoadFloat(location_numberOfRows, numberOfRows);
        }

        public void LoadOffset(float x, float y)
        {
            LoadVector(location_offset, new Vector2(x, y));
        }

        public void LoadSkyColour(Vector3 color)
        {
            LoadVector(location_skyColour, color);
        }

        public void LoadFakeLightingVariable(bool useFakeLighting)
        {
            LoadBoolean(location_useFakeLighting, useFakeLighting);
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
