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

        private int location_projectionMatrix;
        private int location_viewMatrix;

        public SkyboxShader()
            : base(VertexFile, FragmentFile)
        {
        }

        public void LoadProjectionMatrix(Matrix4 matrix)
        {
            LoadMatrix(location_projectionMatrix, matrix);
        }

        public void LoadViewMatrix(BaseCamera camera)
        {
            Matrix4 matrix = Maths.CreateViewMatrix(camera);
            matrix.M41 = 0;
            matrix.M42 = 0;
            matrix.M43 = 0;
            LoadMatrix(location_viewMatrix, matrix);
        }

        protected override void BindAttributes()
        {
            BindAttribute(0, "position");
        }

        protected override void GetAllUniformLocations()
        {
            location_projectionMatrix = GetUniformLocation("projectionMatrix");
            location_viewMatrix = GetUniformLocation("viewMatrix");
        }
    }
}
