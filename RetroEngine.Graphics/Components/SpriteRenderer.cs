using OpenTK.Mathematics;
using RetroEngine.Graphics.Batching;

namespace RetroEngine.Graphics.Components
{
    /// <summary>
    /// Defines how a sprite of an entity is rendered in the screen.
    /// </summary>
    /// <param name="texture">Texture to render.</param>
    public struct SpriteRenderer(Texture2D texture)
    {
        /// <summary>
        /// Gets or sets the texture of the sprite.
        /// </summary>
        public Texture2D Texture { get; private set; } = texture;

        /// <summary>
        /// Gets or sets the width of the sprite.
        /// </summary>
        /// <remarks>This would be removed when Sprite content type is created.</remarks>
        public int Width { get; set; } = 0;

        /// <summary>
        /// Gets or sets the height of the sprite.
        /// </summary>
        /// <remarks>This would be removed when Sprite content type is created.</remarks>
        public int Height { get; set; } = 0;

        /// <summary>
        /// Gets or sets color tincture of the sprite.
        /// </summary>
        public Color4 Color { get; set; } = Color4.White;

        /// <summary>
        /// Gets or sets the flip mode of the sprite.
        /// </summary>
        public Flip Flip { get; set; } = Flip.None;

        /// <summary>
        /// Flips this sprite horizontally.
        /// </summary>
        public void FlipHorizontally()
        {
            Flip ^= Flip.X;
        }

        /// <summary>
        /// Flips this sprite vertically.
        /// </summary>
        public void FlipVertically()
        {
            Flip ^= Flip.Y;
        }
    }

    /// <summary>
    /// Enumerates all flipping moves of an image.
    /// </summary>
    public enum Flip
    {
        None = 0,
        X = 1,
        Y = 2,
        Both = 3
    }
}
