using RenderEngine;
using Textures;

namespace Models
{
    public class TexturedModel
    {
        public RawModel RawModel { get; }
        public ModelTexture Texture { get; }
        public float Height { get; }

        public TexturedModel(RawModel model, float height, ModelTexture texture)
        {
            RawModel = model;
            Texture = texture;
            Height = height;
        }
    }
}
