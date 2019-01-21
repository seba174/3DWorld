using OpenTK;
using Utilities;

namespace Shaders
{
    public class EntityShader : BaseShader
    {
        private int location_useFakeLighting;
        private int location_numberOfRows;
        private int location_offset;

        public EntityShader(ShadingType shadingType)
        {
            if (shadingType == ShadingType.Phong)
            {
                Initialize(Constants.BasePhongShaderFolder + Constants.EntityVertexShader, Constants.BasePhongShaderFolder + Constants.EntityFragmentShader);
            }
            else if (shadingType == ShadingType.Flat)
            {
                Initialize(Constants.BaseFlatShaderFolder + Constants.EntityVertexShader, Constants.BaseFlatShaderFolder + Constants.EntityFragmentShader);
            }
            else if (shadingType == ShadingType.Gouraud)
            {
                Initialize(Constants.BaseGouraudShaderFolder + Constants.EntityVertexShader, Constants.BaseGouraudShaderFolder + Constants.EntityFragmentShader);
            }
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
