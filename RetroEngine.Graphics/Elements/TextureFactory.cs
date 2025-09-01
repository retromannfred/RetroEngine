using OpenTK.Mathematics;
using StbImageSharp;

namespace RetroEngine.Graphics
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

        /// <summary>
        /// Creates a new solid circle texture.
        /// </summary>
        /// <param name="radius">Radius of the circle in pixels.</param>
        /// <param name="color">Fill color of the circle.</param>
        /// <returns>A new texture struct representing the circle.</returns>
        public static Texture2D CreateCircle(int radius, Color4 color)
        {
            int diameter = radius * 2;
            var data = new byte[diameter * diameter * 4];

            int centerX = radius;
            int centerY = radius;
            float rSquared = radius * radius;

            for (int y = 0; y < diameter; y++)
            {
                for (int x = 0; x < diameter; x++)
                {
                    int index = (y * diameter + x) * 4;

                    int dx = x - centerX;
                    int dy = y - centerY;
                    float distanceSquared = dx * dx + dy * dy;

                    if (distanceSquared <= rSquared)
                    {
                        data[index] = (byte)(color.R * 255);
                        data[index + 1] = (byte)(color.G * 255);
                        data[index + 2] = (byte)(color.B * 255);
                        data[index + 3] = (byte)(color.A * 255);
                    }
                    else
                    {
                        data[index] = 0;
                        data[index + 1] = 0;
                        data[index + 2] = 0;
                        data[index + 3] = 0;
                    }
                }
            }

            return new Texture2D(diameter, diameter, data);
        }

    }
}
