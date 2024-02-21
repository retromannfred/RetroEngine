using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Core.Batching
{
    /// <summary>
    /// Represents position, texture coord and color of a vertex.
    /// </summary>
    public struct VertexInfo
    {
        /// <summary>
        /// Gets or sets vertex position.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Gets or sets vertex texture coord.
        /// </summary>
        public Vector2 TextureCoord;

        /// <summary>
        /// Gets or sets vertex color.
        /// </summary>
        public Vector4 Color;
    }
}
