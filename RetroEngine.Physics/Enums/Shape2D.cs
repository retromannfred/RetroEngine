using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Physics.Enums
{
    /// <summary>
    /// Defines the shape of a collider.
    /// </summary>
    public enum Shape2D
    {
        /// <summary>
        /// Rectangle with a width and a height.
        /// </summary>
        Rectangle,

        /// <summary>
        /// Circle with a radius.
        /// </summary>
        Circle,

        /// <summary>
        /// Convex polygon with undefined vertices.
        /// </summary>
        Polygon
    }
}
