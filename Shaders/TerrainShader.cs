using Utilities;

namespace Shaders
{
    public class TerrainShader : BaseShader
    {
        private int location_backgroundTexture;
        private int location_rTexture;
        private int location_gTexture;
        private int location_bTexture;
        private int location_blendMap;

        public TerrainShader(ShadingType shadingType)
        {
            if (shadingType == ShadingType.Phong)
            {
                Initialize(Constants.BasePhongShaderFolder + Constants.TerrainVertexShader, Constants.BasePhongShaderFolder + Constants.TerrainFragmentShader);
            }
            else if (shadingType == ShadingType.Flat)
            {
                Initialize(Constants.BaseFlatShaderFolder + Constants.TerrainVertexShader, Constants.BaseFlatShaderFolder + Constants.TerrainFragmentShader);
            }
        }

        protected override void BindAttributes()
        {
            base.BindAttributes();
        }

        protected override void GetAllUniformLocations()
        {
            base.GetAllUniformLocations();

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
    }
}
