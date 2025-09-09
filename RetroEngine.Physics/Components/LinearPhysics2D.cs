using OpenTK.Mathematics;

namespace RetroEngine.Physics
{
    /// <summary>
    /// Defines a linear movement behaviour.
    /// </summary>
    public struct LinearPhysics2D()
    {
        private float _linearDrag = 0;

        /// <summary>
        /// Gets or sets the linear velocity of the entity.
        /// </summary>
        public Vector2 Velocity { get; set; } = Vector2.Zero;

        /// <summary>
        /// Gets or sets the linear drag of the entity.
        /// </summary>
        /// <remarks>Set value will be clamped between 0 and 1.</remarks></remarks>
        public float Drag { readonly get => _linearDrag; set => _linearDrag = Math.Clamp(value, 0, 1); }

        /// <summary>
        /// Gets or sets if the movement is freezed in any of the axis.
        /// </summary>
        public FreezedMovement FreezedMovement { get; set; } = FreezedMovement.None;
    }
}
