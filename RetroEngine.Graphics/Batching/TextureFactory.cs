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
    public static class TextureFactory
    {
        public static Texture CreateFromFile(string filepath)
        {
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult image = ImageResult.FromStream(File.OpenRead(filepath), ColorComponents.RedGreenBlueAlpha);

            return new Texture(image.Width, image.Height, image.Data);
        }

        public static Texture CreateRectangle(int width, int height, Color4 color)
        {
            var data = new byte[width * height * 4];
            for (int i = 0; i < data.Length; i++)
            {
                switch (i % 4)
                {
                    case 0: data[i] = (byte)(color.R * 255); break;
                    case 1: data[i] = (byte)(color.G * 255); break;
                    case 2: data[i] = (byte)(color.B * 255); break;
                    case 3: data[i] = (byte)(color.A * 255); break;
                }
            }

            return new Texture(width, height, data);
        }
    }
}
