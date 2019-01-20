using OpenTK;
using Utilities;

namespace Shaders
{
    public class EntityShader : BaseShader
    {
        private static string VertexFile = Constants.BaseShaderFolder + "VertexShader.glsl";
        private static string FragmentFile = Constants.BaseShaderFolder + "FragmentShader.glsl";

        private int location_useFakeLighting;
        private int location_numberOfRows;
        private int location_offset;

        public EntityShader() : base(VertexFile, FragmentFile)
        {
        }

        protected override void GetAllUniformLocations()
        {
            base.GetAllUniformLocations();

            location_useFakeLighting = GetUniformLocation("useFakeLighting");
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

        public void LoadFakeLightingVariable(bool useFakeLighting)
        {
            LoadBoolean(location_useFakeLighting, useFakeLighting);
        }
    }
}
