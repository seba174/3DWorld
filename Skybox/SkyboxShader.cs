using Entities;
using OpenTK;
using Shaders;
using ToolBox;
using Utilities;

namespace Skybox
{
    public class SkyboxShader : ShaderProgram
    {
        private const float RotationSpeed = 0.001f;

        private int location_projectionMatrix;
        private int location_viewMatrix;
        private int location_fogColour;
        private int location_cubeMap;
        private int location_cubeMap2;
        private int location_blendFactor;

        private float rotation = 0;

        public SkyboxShader()
        {
            Initialize(Constants.SkyboxShaderFolder + Constants.SkyboxVertexShader, Constants.SkyboxShaderFolder + Constants.SkyboxFragmentShader);
        }

        protected override void BindAttributes()
        {
            BindAttribute(0, "position");
        }

        protected override void GetAllUniformLocations()
        {
            location_projectionMatrix = GetUniformLocation("projectionMatrix");
            location_viewMatrix = GetUniformLocation("viewMatrix");
            location_fogColour = GetUniformLocation("fogColour");
            location_cubeMap = GetUniformLocation("cubeMap");
            location_cubeMap2 = GetUniformLocation("cubeMap2");
            location_blendFactor = GetUniformLocation("blendFactor");
        }

        public void ConnectTextureUnits()
        {
            LoadInt(location_cubeMap, 0);
            LoadInt(location_cubeMap2, 1);
        }

        public void LoadBlendFactor(float blendFactor)
        {
            LoadFloat(location_blendFactor, blendFactor);
        }

        public void LoadFogColour(Vector3 fogColour)
        {
            LoadVector(location_fogColour, fogColour);
        }

        public void LoadProjectionMatrix(Matrix4 matrix)
        {
            LoadMatrix(location_projectionMatrix, matrix);
        }

        public void LoadViewMatrix(BaseCamera camera, long frameTime)
        {
            rotation += RotationSpeed * frameTime;
            Matrix4 matrix = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation));
            matrix *= Maths.CreateViewMatrix(camera);
            matrix.M41 = 0;
            matrix.M42 = 0;
            matrix.M43 = 0;

            LoadMatrix(location_viewMatrix, matrix);
        }
    }
}
