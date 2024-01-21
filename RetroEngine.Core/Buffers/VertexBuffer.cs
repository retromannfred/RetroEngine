using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

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
        /// Creates a new vertex buffer object with 3-dimension vertices.
        /// </summary>
        /// <param name="data">Collection of 3-dimension vertices data for the buffer.</param>
        public VertexBuffer(List<Vector3> data)
        {
            Id = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Count * Vector3.SizeInBytes, data.ToArray(), BufferUsageHint.StaticDraw);
        }

        /// <summary>
        /// Creates a new vertex buffer object with 2-dimension vertices.
        /// </summary>
        /// <param name="data">Collection of 2-dimension vertices data for the buffer.</param>
        public VertexBuffer(List<Vector2> data)
        {
            Id = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Count * Vector2.SizeInBytes, data.ToArray(), BufferUsageHint.StaticDraw);
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
