using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

namespace RetroEngine.Core.Buffers
{
    /// <summary>
    /// Abstracts the vertex buffer object functionallity from OpenGL.
    /// </summary>
    internal class VertexBuffer
    {
        /// <summary>
        /// Gets the OpenGL ID of this object.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Creates a new vertex buffer object.
        /// </summary>
        public VertexBuffer()
        {
            Id = GL.GenBuffer();
        }

        /// <summary>
        /// Updates all data of this vertex buffer.
        /// </summary>
        /// <typeparam name="T">Type of vertex.</typeparam>
        /// <param name="data">New data of this buffer.</param>
        public void UpdateData<T>(T[] data) where T : struct
        {
            Bind();
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * Unsafe.SizeOf<T>(), data, BufferUsageHint.StaticDraw);
        }

        /// <summary>
        /// Updates a section of data of this vertex buffer object.
        /// </summary>
        /// <typeparam name="T">Type of vertex.</typeparam>
        /// <param name="offset">Index of first element of data.</param>
        /// <param name="data">New data of this buffer.</param>
        public void UpdateData<T>(int offset, T[] data) where T : struct
        {
            Bind();
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)offset, data.Length * Unsafe.SizeOf<T>(), data);
        }

        /// <summary>
        /// Binds this OpenGL buffer.
        /// </summary>
        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
        }

        /// <summary>
        /// Unbinds this OpenGL buffer.
        /// </summary>
        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        /// <summary>
        /// Deletes this OpenGL buffer.
        /// </summary>
        public void Delete()
        {
            GL.DeleteBuffer(Id);
        }
    }
}
