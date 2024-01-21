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
        /// <param name="data">Collection of vertex indices.</param>
        public ElementBuffer(List<uint> data)
        {
            Id = GL.GenBuffer();
            Count = data.Count;
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Id);
            GL.BufferData(BufferTarget.ElementArrayBuffer, data.Count * sizeof(uint), data.ToArray(), BufferUsageHint.StaticDraw);
        }

        /// <summary>
        /// Arrange the vertex indices to match a new element list.
        /// </summary>
        /// <param name="elements"></param>
        public void Arrange(List<int> elements)
        {
            List<uint> newIndices = new();

            foreach (var item in elements)
            {
                uint quadOffset = (uint)item * 4;
                newIndices.AddRange(new List<uint>
                {
                    quadOffset + 0, quadOffset + 1, quadOffset + 3,
                    quadOffset + 1, quadOffset + 2, quadOffset + 3,
                });
            }

            Bind();
            GL.BufferSubData<uint>(BufferTarget.ElementArrayBuffer, IntPtr.Zero, newIndices.Count * sizeof(uint), newIndices.ToArray());
            Unbind();
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
