using OpenTK.Mathematics;
using RetroEngine.Physics.Enums;

namespace RetroEngine.Physics.Components
{
    /// <summary>
    /// Defines the physics behaviour of an entity.
    /// </summary>
    public struct RigidBody2D
    {
        private float _linearDrag;
        private float _angularDrag;

        /// <summary>
        /// Gets or sets the type of the body.
        /// </summary>
        public BodyType Type { get; set; }

        /// <summary>
        /// Gets or sets whether the body is processed in the physics engine.
        /// </summary>
        public bool Simulated { get; set; }

        /// <summary>
        /// Gets or sets the mass of the body.
        /// </summary>
        public float Mass { get; set; }

        /// <summary>
        /// Gets or sets the linear velocity of the body.
        /// </summary>
        public Vector2 LinearVelocity { get; set; }

        /// <summary>
        /// Gets or sets the angular velocity of the body.
        /// </summary>
        public float AngularVelocity { get; set; }

        /// <summary>
        /// Gets or sets the linear drag of the body.
        /// </summary>
        /// <remarks>Set value will be clamped between 0 and 1.</remarks></remarks>
        public float LinearDrag { readonly get => _linearDrag; set => _linearDrag = Math.Clamp(value, 0, 1); }

        /// <summary>
        /// Gets or sets the angular drag of the body.
        /// </summary>
        /// <remarks>Set value will be clamped between 0 and 1.</remarks>
        public float AngularDrag { readonly get => _angularDrag; set => _angularDrag = Math.Clamp(value, 0, 1); }

        /// <summary>
        /// Gets or sets the coeficient indicating how gravity affects the body.
        /// </summary>
        public float GravityScale { get; set; }

        /// <summary>
        /// Gets or sets wether linear movement is freezed in an axis.
        /// </summary>
        public FreezePosition FreezePosition { get; set; }

        /// <summary>
        /// Gets or sets wether angular movement is freezed.
        /// </summary>
        public bool FreezeRotation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RigidBody2D"/> struct.
        /// </summary>
        public RigidBody2D()
        {
            Type = BodyType.Static;
            Simulated = true;
            Mass = 1f;
            LinearVelocity = Vector2.Zero;
            AngularVelocity = 0f;
            LinearDrag = 0f;
            AngularDrag = .05f;
            GravityScale = 1f;
            FreezePosition = new FreezePosition();
            FreezeRotation = false;
        }

        public void ApplyForce(Vector2 force)
        {
            LinearVelocity += force / Mass;
        }
    }

    /// <summary>
    /// Defines the freezing of linear movement in any axis.
    /// </summary>
    /// <param name="horizontal">Value indicating whether linear movement if frozen in the X-axis.</param>
    /// <param name="vertical">Value indicating whether linear movement if frozen in the Y-axis.</param>
    public struct FreezePosition(bool horizontal, bool vertical)
    {
        /// <summary>
        /// Gets or sets whether the movement is frozen in the X-axis.
        /// </summary>
        public bool X { get; set; } = horizontal;

        /// <summary>
        /// Gets or sets whether the movement is frozen in the Y-axis.
        /// </summary>
        public bool Y { get; set; } = vertical;

        /// <summary>
        /// Creates a new FreezePosition without freezing.
        /// </summary>
        public FreezePosition()
            : this(false, false)
        { }
    }
}
