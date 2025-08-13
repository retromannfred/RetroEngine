using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Core
{
    /// <summary>
    /// Defines a group of graphic settings of a game.
    /// </summary>
    public class GraphicSettings
    {
        /// <summary>
        /// Gets the width of the window.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets the height of the window.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets the aspect ratio of the window (width / height).
        /// </summary>
        public float AspectRatio
        {
            get { return (float)Width / Height; }
        }

        public List<Matrix4> ClipSpaces { get; internal set; }

        /// <summary>
        /// Creates a new
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public GraphicSettings(int width, int height)
        {
            Width = width;
            Height = height;
            ClipSpaces = new List<Matrix4>();
        }
    }
}
