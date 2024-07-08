using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using RetroEngine.Core.Elements;
using RetroEngine.Graphics.Batching;

namespace RetroEngine.Ecs.Components
{
    /// <summary>
    /// Defines parameters needed for a sprite to be rendered.
    /// </summary>
    public struct SpriteRenderer : IComponent
    {
        /// <summary>
        /// Gets or sets the texture of the sprite.
        /// </summary>
        public Texture Texture { get; private set; }

        /// <summary>
        /// Gets or sets the width of the sprite.
        /// </summary>
        /// <remarks>This would be removed when Sprite content type is created.</remarks>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the sprite.
        /// </summary>
        /// <remarks>This would be removed when Sprite content type is created.</remarks>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets color tincture of the sprite.
        /// </summary>
        public Color4 Color { get; set; }

        /// <summary>
        /// Gets or sets the flip mode of the sprite.
        /// </summary>
        public Flip Flip { get; set; }

        /// <summary>
        /// Gets or sets how deep this sprite is drawn (z component of its position).
        /// </summary>
        public int LayerDepth { get; set; }

        /// <summary>
        /// Creates a new sprite renderer.
        /// <param name="texture">Texture to render.</param>
        /// </summary>
        public SpriteRenderer(Texture texture)
        {
            Texture = texture;
            Width = 0;
            Height = 0;
            Color = Color4.White;
            Flip = Flip.None;
            LayerDepth = 0;
        }

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
    /// Defines all flipping moves of an image.
    /// </summary>
    public enum Flip
    {
        None = 0,
        X = 1,
        Y = 2,
        Both = 3
    }
}
