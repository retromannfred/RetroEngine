using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Graphics.Batching
{
    /// <summary>
    /// Defines functionallity to create textures.
    /// </summary>
    public static class TextureFactory
    {
        /// <summary>
        /// Creates a new texture from file.
        /// </summary>
        /// <param name="filepath">File path of the image.</param>
        /// <returns>A new texture struct.</returns>
        public static Texture2D CreateFromFile(string filepath)
        {
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult image = ImageResult.FromStream(File.OpenRead(filepath), ColorComponents.RedGreenBlueAlpha);

            return new Texture2D(image.Width, image.Height, image.Data);
        }

        /// <summary>
        /// Creates a new solid rectangle texture.
        /// </summary>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="color">Color of the rectangle.</param>
        /// <returns>A new texture struct representing the rectangle.</returns>
        public static Texture2D CreateRectangle(int width, int height, Color4 color)
        {
            var data = new byte[width * height * 4];
            for (int i = 0; i < data.Length; i+=4)
            {
                data[i]   = (byte)(color.R * 255);
                data[i+1] = (byte)(color.G * 255);
                data[i+2] = (byte)(color.B * 255);
                data[i+3] = (byte)(color.A * 255);
            }

            return new Texture2D(width, height, data);
        }
    }
}
