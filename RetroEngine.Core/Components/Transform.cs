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
    }
}