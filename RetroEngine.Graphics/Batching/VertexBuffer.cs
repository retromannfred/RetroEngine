using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

namespace RetroEngine.Graphics.Batching
{
    /// <summary>
    /// Abstracts the vertex buffer object functionallity from OpenGL.
    /// </summary>
    /// <remarks>
    /// Creates a new vertex buffer object.
    /// </remarks>
    /// <param name="bufferUsage">Specifies the expected usage pattern of the data store.</param>
    public class VertexBuffer<T>(BufferUsageHint bufferUsage)
        where T : struct
    {
        /// <summary>
        /// Gets the OpenGL ID of this object.
        /// </summary>
        public int Id { get; private set; } = GL.GenBuffer();

        /// <summary>
        /// Gets the number of items in this buffer.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Specifies the expected usage pattern of the data store.
        /// </summary>
        public BufferUsageHint UsageHint { get; private set; } = bufferUsage;

        /// <summary>
        /// Creates and initializes a buffer object's data store
        /// </summary>
        /// <param name="data">New data of this buffer.</param>
        public void CreateData(T[] data)
        {
            Bind();
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * Unsafe.SizeOf<T>(), data, UsageHint);
            Count = data.Length;
        }

        /// <summary>
        /// Creates and initializes a buffer object's data store
        /// </summary>
        /// <param name="size">Number of floats used to represent each element.</param>
        public void CreateData(int size)
        {
            Bind();
            GL.BufferData(BufferTarget.ArrayBuffer, size * Unsafe.SizeOf<T>(), IntPtr.Zero, UsageHint);
            Count = size;
        }

        /// <summary>
        /// Updates a section of data of this vertex buffer object.
        /// </summary>
        /// <param name="offset">Index of first element of data.</param>
        /// <param name="data">New data of this buffer.</param>
        public void UpdateData(int offset, int size, T[] data)
        {
            var minSize = Math.Min(offset + size, data.Length);

            Bind();
            GL.BufferSubData(BufferTarget.ArrayBuffer, new nint(nint.Zero.ToInt64() + offset * Unsafe.SizeOf<T>()), minSize * Unsafe.SizeOf<T>(), data);
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
