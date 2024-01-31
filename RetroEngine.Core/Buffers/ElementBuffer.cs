using OpenTK.Graphics.OpenGL4;

namespace RetroEngine.Core.Buffers
{
    /// <summary>
    /// Abstracts the element buffer object functionallity from OpenGL.
    /// </summary>
    internal class ElementBuffer
    {
        /// <summary>
        /// Gets the OpenGL ID of this object.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the number of elements in this buffer.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Creates a new element buffer object.
        /// </summary>
        public ElementBuffer()
        {
            Id = GL.GenBuffer();
        }

        /// <summary>
        /// Updates all data of this element buffer.
        /// </summary>
        /// <param name="data">New data of this buffer.</param>
        public void UpdateData(uint[] data)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Id);
            GL.BufferData(BufferTarget.ElementArrayBuffer, data.Length * sizeof(uint), data.ToArray(), BufferUsageHint.StaticDraw);
        }

        /// <summary>
        /// Updates a section of data of this element buffer object.
        /// </summary>
        /// <param name="offset">Index of first element of data.</param>
        /// <param name="data">Data to be updated.</param>
        public void UpdateData(int offset, uint[] data)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Id);
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)offset, data.Length * sizeof(uint), data);
        }

        /// <summary>
        /// Binds this OpenGL buffer.
        /// </summary>
        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Id);
        }

        /// <summary>
        /// Unbinds this OpenGL buffer.
        /// </summary>
        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
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
