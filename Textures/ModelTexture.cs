
namespace Textures
{
    public class ModelTexture
    {
        public int ID { get; }

        public float ShineDampler { get; set; } = 1;
        public float Reflectivity { get; set; }

        public bool HasTransparency { get; set; }
        public bool UseFakeLightning { get; set; }

        public int NumberOfRows { get; set; } = 1;

        public ModelTexture(int id)
        {
            ID = id;
        }
    }
}
