using Entities;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using RenderEngine;
using ToolBox;
using Utilities;

namespace Skybox
{
    public class SkyboxRenderer
    {
        private const float SIZE = 500f;
        private static float[] Vertices = {
        -SIZE,  SIZE, -SIZE,
        -SIZE, -SIZE, -SIZE,
         SIZE, -SIZE, -SIZE,
         SIZE, -SIZE, -SIZE,
         SIZE,  SIZE, -SIZE,
        -SIZE,  SIZE, -SIZE,

        -SIZE, -SIZE,  SIZE,
        -SIZE, -SIZE, -SIZE,
        -SIZE,  SIZE, -SIZE,
        -SIZE,  SIZE, -SIZE,
        -SIZE,  SIZE,  SIZE,
        -SIZE, -SIZE,  SIZE,

         SIZE, -SIZE, -SIZE,
         SIZE, -SIZE,  SIZE,
         SIZE,  SIZE,  SIZE,
         SIZE,  SIZE,  SIZE,
         SIZE,  SIZE, -SIZE,
         SIZE, -SIZE, -SIZE,

        -SIZE, -SIZE,  SIZE,
        -SIZE,  SIZE,  SIZE,
         SIZE,  SIZE,  SIZE,
         SIZE,  SIZE,  SIZE,
         SIZE, -SIZE,  SIZE,
        -SIZE, -SIZE,  SIZE,

        -SIZE,  SIZE, -SIZE,
         SIZE,  SIZE, -SIZE,
         SIZE,  SIZE,  SIZE,
         SIZE,  SIZE,  SIZE,
        -SIZE,  SIZE,  SIZE,
        -SIZE,  SIZE, -SIZE,

        -SIZE, -SIZE, -SIZE,
        -SIZE, -SIZE,  SIZE,
         SIZE, -SIZE, -SIZE,
         SIZE, -SIZE, -SIZE,
        -SIZE, -SIZE,  SIZE,
         SIZE, -SIZE,  SIZE
    };

        private static string[] DayTextureFiles =
        {
            Constants.SkyboxFolderInResources + "right",
            Constants.SkyboxFolderInResources + "left",
            Constants.SkyboxFolderInResources + "top",
            Constants.SkyboxFolderInResources + "bottom",
            Constants.SkyboxFolderInResources + "back",
            Constants.SkyboxFolderInResources + "front"
        };
        private static string[] NightTextureFiles =
        {
            Constants.SkyboxFolderInResources + "nightRight",
            Constants.SkyboxFolderInResources + "nightLeft",
            Constants.SkyboxFolderInResources + "nightTop",
            Constants.SkyboxFolderInResources + "nightBottom",
            Constants.SkyboxFolderInResources + "nightBack",
            Constants.SkyboxFolderInResources + "nightFront"
        };

        private float time = 0;
        private RawModel cube;
        private readonly int dayTexture;
        private readonly int nightTexture;
        private SkyboxShader shader = new SkyboxShader();

        public SkyboxRenderer(Loader loader, Matrix4 projectionMatrix)
        {
            cube = loader.LoadToVAO(Vertices, 3);
            dayTexture = loader.LoadCubeMap(DayTextureFiles);
            nightTexture = loader.LoadCubeMap(NightTextureFiles);
            shader.Start();
            shader.ConnectTextureUnits();
            shader.LoadProjectionMatrix(projectionMatrix);
            shader.Stop();
        }

        public void Render(BaseCamera camera, Vector3 fogColour, long frameTime)
        {
            shader.Start();

            shader.LoadViewMatrix(camera, frameTime);
            shader.LoadFogColour(fogColour);

            GL.BindVertexArray(cube.VaoID);
            GL.EnableVertexAttribArray(0);

            BindTextures(frameTime);
            GL.DrawArrays(PrimitiveType.Triangles, 0, cube.VertexCount);

            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);

            shader.Stop();
        }

        public void BindTextures(long frameTime)
        {
            time = (time + frameTime / 2) % 24000;

            float blendFactor;
            int dawnTime = 5000, morning = 8000, evening = 21000, night = 24000, texture1, texture2;
            if (time >= 0 && time < dawnTime)
            {
                texture1 = nightTexture;
                texture2 = nightTexture;
                blendFactor = 1;
            }
            else if (time >= dawnTime && time < morning)
            {
                texture1 = nightTexture;
                texture2 = dayTexture;
                blendFactor = (time - dawnTime) / (morning - dawnTime);
            }
            else if (time >= morning && time <= evening)
            {
                texture1 = dayTexture;
                texture2 = dayTexture;
                blendFactor = (time - morning) / (evening - morning);
            }
            else
            {
                texture1 = dayTexture;
                texture2 = nightTexture;
                blendFactor = (time - evening) / (night - evening);
            }

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, texture1);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.TextureCubeMap, texture2);
            shader.LoadBlendFactor(blendFactor);
        }

        public void CleanUp()
        {
            shader.CleanUp();
            GL.DeleteTexture(dayTexture);
            GL.DeleteTexture(nightTexture);
        }
    }
}
