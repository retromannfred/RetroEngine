using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Core.Components
{
    internal class Shader
    {
        public int Id { get; private set; }

        public Shader(string code, ShaderType type)
        {
            Id = GL.CreateShader(type);
            GL.ShaderSource(Id, code);
        }

        public bool Compile()
        {
            GL.CompileShader(Id);
            GL.GetShader(Id, ShaderParameter.CompileStatus, out var shaderCompilationCode);
            if (shaderCompilationCode != (int)All.True)
            {
                Console.WriteLine(GL.GetShaderInfoLog(Id));
                return false;
            }

            return true;
        }

        public void Delete()
        {
            GL.DeleteShader(Id);
        }
    }
}
