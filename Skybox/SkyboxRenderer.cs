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

        public void Render(BaseCamera camera, Vector3 fogColour, DayTime dayTime)
        {
            shader.Start();

            shader.LoadViewMatrix(camera, dayTime.LastFrameTime);
            shader.LoadFogColour(fogColour);

            GL.BindVertexArray(cube.VaoID);
            GL.EnableVertexAttribArray(0);

            BindTextures(dayTime);
            GL.DrawArrays(PrimitiveType.Triangles, 0, cube.VertexCount);

            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);

            shader.Stop();
        }

        public void BindTextures(DayTime dayTime)
        {
            int texture1, texture2;
            if (dayTime.TimeOfDay == TimeOfDay.Dawn)
            {
                texture1 = nightTexture;
                texture2 = dayTexture;
            }
            else if (dayTime.TimeOfDay == TimeOfDay.Morning || dayTime.TimeOfDay == TimeOfDay.Noon)
            {
                texture1 = dayTexture;
                texture2 = dayTexture;
            }
            else if (dayTime.TimeOfDay == TimeOfDay.Noon)
            {
                texture1 = dayTexture;
                texture2 = dayTexture;
            }
            else if(dayTime.TimeOfDay == TimeOfDay.Evening)
            {
                texture1 = dayTexture;
                texture2 = nightTexture;
            }
            else
            {
                texture1 = nightTexture;
                texture2 = nightTexture;
            }

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, texture1);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.TextureCubeMap, texture2);
            shader.LoadBlendFactor(dayTime.TimeOfDayFactor);
        }

        public void CleanUp()
        {
            shader.CleanUp();
            GL.DeleteTexture(dayTexture);
            GL.DeleteTexture(nightTexture);
        }
    }
}
