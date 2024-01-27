using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Core.Shaders
{
    internal class ShaderProgram
    {
        public int Id { get; private set; }

        public ShaderProgram()
        {
            Id = GL.CreateProgram();
        }

        public void AddShader(Shader shader)
        {
            if (shader.Compile())
                GL.AttachShader(Id, shader.Id);
        }

        public void Link()
        {
            GL.LinkProgram(Id);
        }

        public void Bind()
        {
            GL.UseProgram(Id);
        }

        public void Unbind()
        {
            GL.UseProgram(0);
        }

        public void Delete()
        {
            GL.DeleteProgram(0);
        }
    }
}
