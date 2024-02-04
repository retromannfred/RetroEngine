using OpenTK.Graphics.OpenGL4;
using System.Runtime.CompilerServices;

namespace RetroEngine.Core.Buffers
{
    /// <summary>
    /// Abstracts the vertex array object functionallity from OpenGL.
    /// </summary>
    internal class VertexArray
    {
        /// <summary>
        /// Gets the OpenGL ID of this object.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Creates a new vertex array object.
        /// </summary>
        public VertexArray()
        {
            Id = GL.GenVertexArray();
        }

        /// <summary>
        /// Links a vertex buffer object to this vertex array object.
        /// </summary>
        /// <param name="location">Shader location of the vertex buffer object.</param>
        /// <param name="size">Dimensions of the vertices in the vertex buffer object.</param>
        /// <param name="vbo">Vertext buffer object to link.</param>
        public void Link<T>(int location, VertexBuffer<T> vbo) where T : struct
        {
            Bind();
            vbo.Bind();
            GL.VertexAttribPointer(location, Unsafe.SizeOf<T>() / sizeof(float), VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(location);
            Unbind();
        }

        /// <summary>
        /// Binds this OpenGL array.
        /// </summary>
        public void Bind()
        {
            GL.BindVertexArray(Id);
        }

        /// <summary>
        /// Unbinds this OpenGL array.
        /// </summary>
        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        /// <summary>
        /// Deletes this OpenGL array.
        /// </summary>
        public void Delete()
        {
            GL.DeleteVertexArray(Id);
        }
    }
}
