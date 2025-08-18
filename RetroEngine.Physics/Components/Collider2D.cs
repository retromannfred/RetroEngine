using OpenTK.Mathematics;
using RetroEngine.Physics.Enums;

namespace RetroEngine.Physics.Components
{
    public struct Collider2D
    {
        public Shapes2D Shape { get; set; }

        public Vector2 Offset { get; set; }

        public float Restitution { get; set; }

        public float Friction { get; set; }

        public float Density { get; set; }

        public float Radius { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public Collider2D(Shapes2D shape)
        {
            Offset = Vector2.Zero;
            Restitution = 0f;
            Friction = 0f;
            Density = 1f;

            switch (shape)
            {
                case Shapes2D.Rectangle:
                    Radius = 0f;
                    Width = 1f;
                    Height = 1f;
                    break;
                case Shapes2D.Circle:
                    Radius = 1f;
                    Width = 0f;
                    Height = 0f;
                    break;
                case Shapes2D.Polygon:
                    Radius = 0f;
                    Width = 0f;
                    Height = 0f;
                    break;
                default:
                    break;
            }
        }
    }
}
