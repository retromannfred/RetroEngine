namespace RetroEngine.Graphics
{
    /// <summary>
    /// Defines functions for SpriteRenderer component.
    /// </summary>
    public static class SpriteRendererExtensions
    {
        /// <summary>
        /// Flips this sprite horizontally.
        /// <param name="component">Component to manipulate.</param>
        /// </summary>
        public static void FlipHorizontally(this ref SpriteRenderer component)
        {
            component.Flip ^= Flip.X;
        }

        /// <summary>
        /// Flips this sprite vertically.
        /// <param name="component">Component to manipulate.</param>
        /// </summary>
        public static void FlipVertically(this ref SpriteRenderer component)
        {
            component.Flip ^= Flip.Y;
        }
    }
}
