using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Graphics.Shaders
{
    /// <summary>
    /// Defines a program to execute on the GPU.
    /// </summary>
    public class ShaderProgram
    {
        /// <summary>
        /// Gets this program ID.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Creates a new program.
        /// </summary>
        public ShaderProgram()
        {
            Id = GL.CreateProgram();
        }

        /// <summary>
        /// Adds a shader to this program.
        /// </summary>
        /// <param name="shader">Shader to attach.</param>
        public void AddShader(Shader shader)
        {
            if (shader.Compile())
                GL.AttachShader(Id, shader.Id);
        }

        /// <summary>
        /// Creates an executable of this program to run on the GPU.
        /// </summary>
        public void Link()
        {
            GL.LinkProgram(Id);
        }

        /// <summary>
        /// Marks this program to be used for rendering.
        /// </summary>
        public void Bind()
        {
            GL.UseProgram(Id);
        }

        /// <summary>
        /// Stops this program to be use for rendering.
        /// </summary>
        public static void Unbind()
        {
            GL.UseProgram(0);
        }

        /// <summary>
        /// Deletes this program from the GPU.
        /// </summary>
        public void Delete()
        {
            GL.DeleteProgram(Id);
        }
    }
}
