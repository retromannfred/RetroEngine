using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Graphics.Settings
{
    public class GraphicSettings
    {
        /// <summary>
        /// Gets the width of the window.
        /// </summary>
        public int Width { get; internal set; }

        /// <summary>
        /// Gets the height of the window.
        /// </summary>
        public int Height { get; internal set; }

        /// <summary>
        /// Gets the aspect ratio of the window (width / height).
        /// </summary>
        public float AspectRatio
        {
            get { return (float)Width / Height; }
        }

        internal GraphicSettings(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
