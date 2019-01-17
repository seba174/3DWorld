using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderEngine;
using Textures;

namespace Models
{
    public class TexturedModel
    {
        public RawModel RawModel { get; }
        public ModelTexture Texture { get; }

        public TexturedModel(RawModel model, ModelTexture texture)
        {
            RawModel = model;
            Texture = texture;
        }
    }
}
