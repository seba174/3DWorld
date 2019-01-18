
namespace Textures
{
    public class ModelTexture
    {
        public int ID { get; }
        public float ShineDampler { get; set; } = 1;
        public float Reflectivity { get; set; }

        public ModelTexture(int id)
        {
            ID = id;
        }
    }
}
