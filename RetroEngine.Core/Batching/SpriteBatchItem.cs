using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Core.Batching
{
    /// <summary>
    /// Defines how a sprite will be drawn in a sprite batch.
    /// </summary>
    public struct SpriteBatchItem
    {
        /// <summary>
        /// Gets or sets top left vertex info.
        /// </summary>
        public VertexInfo TopLeft;

        /// <summary>
        /// Gets or sets top right vertex info.
        /// </summary>
        public VertexInfo TopRight;

        /// <summary>
        /// Gets or sets bottom left vertex info.
        /// </summary>
        public VertexInfo BottomLeft;

        /// <summary>
        /// Gets or sets bottom right vertex info.
        /// </summary>
        public VertexInfo BottomRight;
    }
}
