using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Graphics.Shaders
{
    /// <summary>
    /// Defines a piece of code executed on the GPU.
    /// </summary>
    internal class Shader
    {
        /// <summary>
        /// Gets the ID of this shader.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Creates a new shader.
        /// </summary>
        /// <param name="code">Code to execute.</param>
        /// <param name="type">Type of shader.</param>
        public Shader(string code, ShaderType type)
        {
            Id = GL.CreateShader(type);
            GL.ShaderSource(Id, code);
        }

        /// <summary>
        /// Compile this shader.
        /// </summary>
        /// <returns>True if the code was properly compiled, and false otherwise.</returns>
        /// <remarks>Prints on the standard output the compiling errors.</remarks>
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

        /// <summary>
        /// Deletes this shader from the GPU.
        /// </summary>
        public void Delete()
        {
            GL.DeleteShader(Id);
        }
    }
}
