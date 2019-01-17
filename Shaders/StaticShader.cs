using Entities;
using OpenTK;
using ToolBox;

namespace Shaders
{
    public class StaticShader : ShaderProgram
    {
        private const string basePath = "../../../Shaders/";
        private static string VertexFile = basePath + "VertexShader.c";
        private static string FragmentFile = basePath + "FragmentShader.c";

        private int location_transformationMatrix;
        private int location_projectionMatrix;
        private int location_viewMatrix;

        public StaticShader() : base(VertexFile, FragmentFile)
        {
        }

        protected override void BindAttributes()
        {
            BindAttribute(0, "position");
            BindAttribute(1, "textureCoords");
        }

        protected override void GetAllUniformLocations()
        {
            location_transformationMatrix =  GetUniformLocation("transformationMatrix");
            location_projectionMatrix = GetUniformLocation("projectionMatrix");
            location_viewMatrix = GetUniformLocation("viewMatrix");
        }

        public void LoadTransformationMatrix(Matrix4 matrix)
        {
            LoadMatrix(location_transformationMatrix, matrix);
        }

        public void LoadProjectionMatrix(Matrix4 matrix)
        {
            LoadMatrix(location_projectionMatrix, matrix);
        }

        public void LoadViewMatrix(Camera camera)
        {
            Matrix4 viewMatrix = Maths.CreateViewMatrix(camera);
            LoadMatrix(location_viewMatrix, viewMatrix);
        }
    }
}
