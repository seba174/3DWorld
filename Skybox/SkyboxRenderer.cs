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

        private static string[] TextureFiles = {
            Constants.SkyboxFolderInResources + "right",
            Constants.SkyboxFolderInResources + "left",
            Constants.SkyboxFolderInResources + "top",
            Constants.SkyboxFolderInResources + "bottom",
            Constants.SkyboxFolderInResources + "back",
            Constants.SkyboxFolderInResources + "front"
        };

        private RawModel cube;
        private readonly int texture;
        private SkyboxShader shader = new SkyboxShader();

        public SkyboxRenderer(Loader loader, Matrix4 projectionMatrix)
        {
            cube = loader.LoadToVAO(Vertices, 3);
            texture = loader.LoadCubeMap(TextureFiles);
            shader.Start();
            shader.LoadProjectionMatrix(projectionMatrix);
            shader.Stop();
        }

        public void Render(BaseCamera camera)
        {
            shader.Start();
            shader.LoadViewMatrix(camera);
            GL.BindVertexArray(cube.VaoID);
            GL.EnableVertexAttribArray(0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, texture);
            GL.DrawArrays(PrimitiveType.Triangles, 0, cube.VertexCount);
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
            shader.Stop();
        }
    }
}
