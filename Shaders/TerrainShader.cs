using Utilities;

namespace Shaders
{
    public class TerrainShader : BaseShader
    {
        private const string VertexFile = Constants.BaseShaderFolder + "TerrainVertexShader.glsl";
        private const string FragmentFile = Constants.BaseShaderFolder + "TerrainFragmentShader.glsl";

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
