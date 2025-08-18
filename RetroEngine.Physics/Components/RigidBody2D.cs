using OpenTK.Mathematics;

namespace RetroEngine.Physics.Components
{
    public struct RigidBody2D
    {
        public BodyType Type { get; set; }

        public bool Simulated { get; set; }

        public float Mass { get; set; }

        public Vector2 LinearVelocity { get; set; }

        public float AngularVelocity { get; set; }

        public float LinearDrag { get; set; }

        public float AngularDrag { get; set; }

        public float GravityScale { get; set; }

        public FreezePosition FreezePosition { get; set; }

        public bool FreezeRotation { get; set; }

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
    }

    public struct FreezePosition
    {
        public bool X { get; set; }

        public bool Y { get; set; }

        public FreezePosition()
        {
            X = false;
            Y = false;
        }

        public FreezePosition(bool horizontal, bool vertical)
        {
            X = horizontal;
            Y = vertical;
        }
    }

    public enum BodyType
    {
        Static,
        Kinetic,
        Dynamic
    }
}
