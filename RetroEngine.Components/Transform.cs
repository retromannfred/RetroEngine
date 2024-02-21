using OpenTK.Mathematics;
using RetroEngine.ECS.Elements;

namespace RetroEngine.Components
{
    public struct Transform : IComponent
    {
        public Vector2 Position { get; set; }

        private float _rotation;
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = MathHelper.ClampRadians(value); }
        }

        public Vector2 Scale { get; set; }

        public Transform()
        {
            Position = Vector2.Zero;
            _rotation = 0f;
            Scale = Vector2.One;
        }

        public void Translate(Vector2 translation)
        {
            Position += translation;
        }

        public void Rotate(float radians)
        {
            Rotation += radians;
        }

        public void Rescale(Vector2 scale)
        {
            Scale *= scale;
        }
    }
}