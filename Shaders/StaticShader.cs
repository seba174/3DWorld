using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaders
{
    public class StaticShader : ShaderProgram
    {
        private static string VertexFile = "../../../Shaders/VertexShader.c";
        private static string FragmentFile = "../../../Shaders/FragmentShader.c";

        public StaticShader() : base(VertexFile, FragmentFile)
        {
        }

        protected override void BindAttributes()
        {
            BindAttribute(0, "position");
        }
    }
}
