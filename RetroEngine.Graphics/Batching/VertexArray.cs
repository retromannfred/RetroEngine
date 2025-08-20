using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace RetroEngine.Graphics.Batching
{
    /// <summary>
    /// Abstracts the vertex array object functionallity from OpenGL.
    /// </summary>
    public class VertexArray
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
        /// <param name="vbo">Vertext buffer object to link.</param>
        /// <remarks>Asumes each element is formed by floats to calculate size.</remarks>
        public void Link<T>(int location, VertexBuffer<T> vbo)
            where T : struct
        {
            Link(location, vbo, Unsafe.SizeOf<T>() / sizeof(float), 0, 0);
        }

        /// <summary>
        /// Links a vertex buffer object to this vertex array object.
        /// </summary>
        /// <param name="location">Shader location of the vertex buffer object.</param>
        /// <param name="vbo">Vertext buffer object to link.</param>
        /// <param name="size">Number of floats used to represent each element.</param>
        public void Link<T>(int location, VertexBuffer<T> vbo, int size)
            where T : struct
        {
            Link(location, vbo, size, 0, 0);
        }

        /// <summary>
        /// Links a vertex buffer object to this vertex array object.
        /// </summary>
        /// <param name="location">Shader location of the vertex buffer object.</param>
        /// <param name="vbo">Vertext buffer object to link.</param>
        /// <param name="size">Number of floats used to represent each element.</param>
        /// <param name="stride">Index jump after each index data.</param>
        /// <param name="offset">Where first index starts.</param>
        public void Link<T>(int location, VertexBuffer<T> vbo, int size, int stride, int offset)
            where T : struct
        {
            Bind();
            vbo.Bind();
            GL.EnableVertexAttribArray(location);
            GL.VertexAttribPointer(location, size, VertexAttribPointerType.Float, false, stride * sizeof(float), offset * sizeof(float));
        }

        /// <summary>
        /// Links a vertex buffer object to this vertex array object.
        /// </summary>
        /// <param name="location">Shader location of the vertex buffer object.</param>
        /// <param name="vbo">Vertext buffer object to link.</param>
        /// <param name="size">Number of floats used to represent each element.</param>
        /// <param name="stride">Index jump after each index data.</param>
        /// <param name="offset">Where first index starts.</param>
        /// <param name="divisor">Specify the number of instances that will pass between updates of the generic attribute at slot index.</param>
        public void LinkDivided<T>(int location, VertexBuffer<T> vbo, int size, int stride, int offset, int divisor)
            where T : struct
        {
            Bind();
            vbo.Bind();
            GL.EnableVertexAttribArray(location);
            GL.VertexAttribPointer(location, size, VertexAttribPointerType.Float, false, stride * sizeof(float), offset * sizeof(float));
            GL.VertexAttribDivisor(location, divisor);
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
        public static void Unbind()
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
