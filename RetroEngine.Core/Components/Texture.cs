using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace RetroEngine.Core.Components
{
    /// <summary>
    /// Abstracts the texture functionallity from OpenGL.
    /// </summary>
    public struct Texture
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
        /// Creates a new texture from an image file.
        /// </summary>
        /// <param name="filepath">Path of the image file.</param>
        public Texture(string filepath)
        {
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult image = ImageResult.FromStream(File.OpenRead(filepath), ColorComponents.RedGreenBlueAlpha);
            Width = image.Width;
            Height = image.Height;

            Id = GL.GenTexture();
            Bind();

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            Unbind();
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
