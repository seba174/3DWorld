using Entities;
using OpenTK;
using Shaders;
using ToolBox;
using Utilities;

namespace Skybox
{
    public class SkyboxShader : ShaderProgram
    {
        private const string VertexFile = Constants.SkyboxShaderFolder + "skyboxVertexShader.glsl";
        private const string FragmentFile = Constants.SkyboxShaderFolder + "skyboxFragmentShader.glsl";

        private const float RotationSpeed = 0.001f;

        private int location_projectionMatrix;
        private int location_viewMatrix;
        private int location_fogColour;

        private float rotation = 0;

        public SkyboxShader()
            : base(VertexFile, FragmentFile)
        {
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
