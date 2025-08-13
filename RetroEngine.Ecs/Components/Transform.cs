using OpenTK.Mathematics;
using RetroEngine.Core.Elements;

namespace RetroEngine.Ecs.Components
{
    /// <summary>
    /// Defines a transformation of an entity in the world space.
    /// </summary>
    public struct Transform : IComponent
    {
        /// <summary>
        /// Gets or sets the position of the entity.
        /// </summary>
        public Vector3 Position { get; set; }

        private Vector3 _rotation;
        /// <summary>
        /// Gets or sets the angle of rotation in radians of the entity.
        /// </summary>
        public Vector3 Rotation
        {
            get { return _rotation; }
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
        public Vector3 Scale { get; set; }

        /// <summary>
        /// Creates a new transform component.
        /// </summary>
        public Transform()
        {
            Position = Vector3.Zero;
            _rotation = Vector3.Zero;
            Scale = Vector3.One;
        }

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