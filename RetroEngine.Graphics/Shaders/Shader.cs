using OpenTK.Graphics.OpenGL4;
using System.Reflection;

namespace RetroEngine.Graphics.Shaders
{
    /// <summary>
    /// Defines a piece of code executed on the GPU.
    /// </summary>
    public class Shader
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

        /// <summary>
        /// Loads a shader from a file compiled as a resource in an assembly.
        /// </summary>
        /// <param name="resourceName">Resource name inside the assembly.</param>
        /// <returns>String with the shader code read from the resource file.</returns>
        public static string LoadShaderSource(Assembly assembly, string resourceName)
        {
            using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
            }

            return string.Empty;
        }
    }
}
