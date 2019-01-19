using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Shaders
{
    public abstract class ShaderProgram
    {
        protected const string basePath = "../../../Shaders/";
        private readonly int programID;
        private readonly int vertexShaderID;
        private readonly int fragmentShaderID;

        public ShaderProgram(string vertexFile, string fragmentFile)
        {
            vertexShaderID = LoadShader(vertexFile, ShaderType.VertexShader);
            fragmentShaderID = LoadShader(fragmentFile, ShaderType.FragmentShader);

            programID = GL.CreateProgram();
            GL.AttachShader(programID, vertexShaderID);
            GL.AttachShader(programID, fragmentShaderID);
            BindAttributes();
            GL.LinkProgram(programID);
            GL.ValidateProgram(programID);
            GetAllUniformLocations();
        }

        public void Start()
        {
            GL.UseProgram(programID);
        }

        public void Stop()
        {
            GL.UseProgram(0);
        }

        public void CleanUp()
        {
            Stop();

            GL.DetachShader(programID, vertexShaderID);
            GL.DetachShader(programID, fragmentShaderID);
            GL.DeleteShader(vertexShaderID);
            GL.DeleteShader(fragmentShaderID);
            GL.DeleteProgram(programID);
        }

        protected abstract void GetAllUniformLocations();

        protected int GetUniformLocation(string uniformName)
        {
            return GL.GetUniformLocation(programID, uniformName);
        }

        protected abstract void BindAttributes();

        protected void BindAttribute(int attribute, string variableName)
        {
            GL.BindAttribLocation(programID, attribute, variableName);
        }

        #region Load

        protected void LoadFloat(int location, float value)
        {
            GL.Uniform1(location, value);
        }

        protected void LoadInt(int location, int value)
        {
            GL.Uniform1(location, value);
        }

        protected void LoadVector(int location, Vector3 vector)
        {
            GL.Uniform3(location, vector);
        }

        protected void LoadBoolean(int location, bool value)
        {
            float toLoad = value == true ? 1 : 0;
            GL.Uniform1(location, toLoad);
        }

        protected void LoadMatrix(int location, Matrix4 matrix)
        {
            GL.UniformMatrix4(location, false, ref matrix);
        }

        private static int LoadShader(string file, ShaderType type)
        {
            int shaderID = GL.CreateShader(type);
            GL.ShaderSource(shaderID, File.ReadAllText(file));
            GL.CompileShader(shaderID);

            string infoLog = GL.GetShaderInfoLog(shaderID);
            if (infoLog != string.Empty)
            {
                Console.WriteLine(infoLog);
                throw new Exception(infoLog);
            }

            return shaderID;
        }

        #endregion
    }
}
