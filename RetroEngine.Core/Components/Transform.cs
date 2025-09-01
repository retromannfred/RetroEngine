using OpenTK.Mathematics;

namespace RetroEngine.Core
{
    /// <summary>
    /// Defines the transformation of an entity in the world space.
    /// </summary>
    public struct Transform()
    {
        /// <summary>
        /// Gets or sets the position of the entity.
        /// </summary>
        public Vector3 Position { get; set; } = Vector3.Zero;

        private Vector3 _rotation = Vector3.Zero;
        /// <summary>
        /// Gets or sets the angle of rotation in radians of the entity.
        /// </summary>
        public Vector3 Rotation
        {
            readonly get => _rotation;
            set
            {
                _rotation = new Vector3(
                    MathHelper.ClampRadians(value.X),
                    MathHelper.ClampRadians(value.Y),
                    MathHelper.ClampRadians(value.Z)
                );
            }
        }

        /// <summary>
        /// Gets or sets the scale of the entity.
        /// </summary>
        public Vector3 Scale { get; set; } = Vector3.One;

        /// <summary>
        /// Translates the current component.
        /// </summary>
        /// <param name="translation">Translation to be added to current position.</param>
        public void Translate(Vector3 translation)
        {
            Position += translation;
        }

        /// <summary>
        /// Rotates the current component.
        /// </summary>
        /// <param name="radians">Rotation vector in radians to be added to current rotation.</param>
        public void Rotate(Vector3 radians)
        {
            Rotation += radians;
        }

        /// <summary>
        /// Rescales the current component.
        /// </summary>
        /// <param name="scale">Module to multiply the current scale.</param>
        public void Rescale(Vector3 scale)
        {
            Scale *= scale;
        }
    }
}