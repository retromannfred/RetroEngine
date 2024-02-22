using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

namespace RetroEngine.Graphics.Buffers
{
    /// <summary>
    /// Abstracts the vertex buffer object functionallity from OpenGL.
    /// </summary>
    internal class VertexBuffer<T> where T : struct
    {
        /// <summary>
        /// Gets the OpenGL ID of this object.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the number of items in this buffer.
        /// </summary>
        public int Count { get; private set; }

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
        /// <param name="data">New data of this buffer.</param>
        public void UpdateData(T[] data)
        {
            Bind();
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * Unsafe.SizeOf<T>(), data, BufferUsageHint.StaticDraw);
            Count = data.Length;
        }

        /// <summary>
        /// Updates a section of data of this vertex buffer object.
        /// </summary>
        /// <param name="offset">Index of first element of data.</param>
        /// <param name="data">New data of this buffer.</param>
        public void UpdateData(int offset, T[] data)
        {
            Bind();
            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(IntPtr.Zero.ToInt64() + offset * Unsafe.SizeOf<T>()), data.Length * Unsafe.SizeOf<T>(), data);
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
