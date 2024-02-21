using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using StbImageSharp;

namespace RetroEngine.Core.Batching
{
    /// <summary>
    /// Abstracts the texture functionallity from OpenGL.
    /// </summary>
    public class Texture
    {
        /// <summary>
        /// Gets this texture ID in OpenGL.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the width of this texture.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height of this texture.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Creates a new texture.
        /// </summary>
        /// <param name="width">Texture width in pixels.</param>
        /// <param name="height">Texture height in pixels.</param>
        /// <param name="data">Data containing colors of the texture.</param>
        public Texture(int width, int height, byte[] data)
        {
            Width = width;
            Height = height;

            Id = GL.GenTexture();
            Bind();

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            Unbind();
        }

        public Texture(int textureId)
        {
            Id = textureId;
            Bind();

            int width, height;
            GL.GetTexParameter(TextureTarget.Texture2D, GetTextureParameter.TextureWidth, out width);
            GL.GetTexParameter(TextureTarget.Texture2D, GetTextureParameter.TextureHeight, out height);
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Binds this OpenGL texture.
        /// </summary>
        public void Bind()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Id);
        }

        /// <summary>
        /// Unbinds this OpenGL texture.
        /// </summary>
        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        /// <summary>
        /// Deletes this OpenGL texture.
        /// </summary>
        public void Delete()
        {
            GL.DeleteTexture(Id);
        }
    }
}
