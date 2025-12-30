using OpenTK.Mathematics;

namespace RetroEngine.Core
{
    /// <summary>
    /// Defines functions for Transform component.
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Translates the current component.
        /// </summary>
        /// <param name="component">Component to manipulate.</param>
        /// <param name="translation">Translation to be added to current position.</param>
        public static void Translate(this ref Transform component, Vector3 translation)
        {
            component.Position += translation;
        }

        /// <summary>
        /// Rotates the current component.
        /// </summary>
        /// <param name="component">Component to manipulate.</param>
        /// <param name="radians">Rotation vector in radians to be added to current rotation.</param>
        public static void Rotate(this ref Transform component, Vector3 radians)
        {
            component.Rotation += radians;
        }

        /// <summary>
        /// Rescales the current component.
        /// </summary>
        /// <param name="component">Component to manipulate.</param>
        /// <param name="scale">Module to multiply the current scale.</param>
        public static void Rescale(this ref Transform component, Vector3 scale)
        {
            component.Scale *= scale;
        }
    }
}
