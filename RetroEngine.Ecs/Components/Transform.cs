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
        public Vector2 Position { get; set; }

        private float _rotation;
        /// <summary>
        /// Gets or sets the angle of rotation in radians of the entity.
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = MathHelper.ClampRadians(value); }
        }

        /// <summary>
        /// Gets or sets the scale of the entity.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// Creates a new transform component.
        /// </summary>
        public Transform()
        {
            Position = Vector2.Zero;
            _rotation = 0f;
            Scale = Vector2.One;
        }

        /// <summary>
        /// Adds a given value in the X and Y axis to the current component.
        /// </summary>
        /// <param name="translation">Translation to be added to current position.</param>
        public void Translate(Vector2 translation)
        {
            Position += translation;
        }

        /// <summary>
        /// Rotates the current component.
        /// </summary>
        /// <param name="radians">Angle to be added to the current rotation.</param>
        public void Rotate(float radians)
        {
            Rotation += radians;
        }

        /// <summary>
        /// Rescales the current component.
        /// </summary>
        /// <param name="scale">Module to multiply the current scale.</param>
        public void Rescale(Vector2 scale)
        {
            Scale *= scale;
        }
    }
}