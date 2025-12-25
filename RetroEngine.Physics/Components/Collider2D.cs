using OpenTK.Mathematics;
using RetroEngine.Core;

namespace RetroEngine.Physics
{
    /// <summary>
    /// Defines how an entity behaves when collides with another entity.
    /// </summary>
    public struct Collider2D()
    {
        private float _restitution = 1f;
        private float _friction = 0f;

        /// <summary>
        /// Gets or sets the shape of the collider.
        /// </summary>
        public Shape2D Shape { get; set; } = Shape2D.Circle;

        /// <summary>
        /// Gets or sets the position of the collider relative to its related transform.
        /// </summary>
        /// <remarks>Final collider position will be transform position + collider offset.</remarks>
        public Vector2 Offset { get; set; } = Vector2.Zero;

        /// <summary>
        /// Gets or sets the restitution of this collider.
        /// </summary>
        /// <remarks>Set value will be clamped between 0 and 1.</remarks>
        public float Restitution { readonly get => _restitution; set => _restitution = Math.Clamp(value, 0, 1); }

        /// <summary>
        /// Gets or sets the friction of this collider.
        /// </summary>
        /// <remarks>Set value will be clamped between 0 and 1.</remarks>
        public float Friction { readonly get => _friction; set => _friction = Math.Clamp(value, 0, 1); }

        /// <summary>
        /// Gets or sets the density of this collider.
        /// </summary>
        public float Density { get; set; } = 1f;

        /// <summary>
        /// Gets or sets de radius of this collider.
        /// </summary>
        /// <remarks>This property will be used when the shape is set to circle.</remarks>
        public float Radius { get; set; } = .5f;

        /// <summary>
        /// Gets or sets the width of this collider.
        /// </summary>
        /// <remarks>This property will be used when the shape is set to rectangle.</remarks>
        public float Width { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the height of this collider.
        /// </summary>
        /// <remarks>This property will be used when the shape is set to rectangle.</remarks>
        public float Height { get; set; } = 1f;

        /// <summary>
        /// Determines if an entity is intersecting with another entity.
        /// </summary>
        /// <param name="transformA">Transform component of the first entity.</param>
        /// <param name="colliderA">Collider2D component of the first entity.</param>
        /// <param name="transformB">Transform component of the second entity.</param>
        /// <param name="colliderB">Collider2D component of the second entity.</param>
        /// <param name="direction">If both entities are intersected, returns normal vector indicating in which direction the first entity has intersected the second one.</param>
        /// <param name="depth">If both entities are intersected, returns the depth of the intersection between the two entities.</param>
        /// <returns>True if both entities are intersected, and false otherwise.</returns>
        public static bool Intersects(
            Transform transformA, Collider2D colliderA,
            Transform transformB, Collider2D colliderB,
            out Vector2 direction, out float depth)
        {
            direction = Vector2.Zero;
            depth = 0;

            if (colliderA.Shape == Shape2D.Circle && colliderB.Shape == Shape2D.Circle)
            {
                return CollisionMath.IntersectCircles(
                    transformA.Position.Xy + colliderA.Offset, colliderA.Radius * MathHelper.Max(transformA.Scale.X, transformA.Scale.Y),
                    transformB.Position.Xy + colliderB.Offset, colliderB.Radius * MathHelper.Max(transformB.Scale.X, transformB.Scale.Y),
                    out direction, out depth);
            }
            else if (colliderA.Shape == Shape2D.Rectangle && colliderB.Shape == Shape2D.Rectangle)
            {
                var verticesA = colliderA.GetRectangleVertices(transformA);
                var verticesB = colliderB.GetRectangleVertices(transformB);

                return CollisionMath.IntersectPolygons(verticesA, verticesB, out direction, out depth);
            }

            return false;
        }

        /// <summary>
        /// Determines if a collider is intersecting with another collider.
        /// </summary>
        /// <param name="transformA">Transform component of the entity with this collider.</param>
        /// <param name="transformB">Transform component of the other entity.</param>
        /// <param name="colliderB">Collider2D component of the other entity.</param>
        /// <param name="direction">If both colliders are intersected, returns normal vector indicating in which direction this collider has intersected the second one.</param>
        /// <param name="depth">If both colliders are intersected, returns the depth of the intersection between the two colliders.</param>
        /// <returns>True if both colliders are intersected, and false otherwise.</returns>
        public readonly bool Intersects(
            Transform transformA,
            Transform transformB, Collider2D colliderB,
            out Vector2 direction, out float depth)
        {
            return Intersects(
                transformA, this,
                transformB, colliderB,
                out direction, out depth);
        }

        /// <summary>
        /// Gets the vertices of a rectangle collider.
        /// </summary>
        /// <param name="transform">Transform component related to this collider.</param>
        /// <returns>An array with the four vertices of the rectangle collider.</returns>
        public readonly Vector2[] GetRectangleVertices(Transform transform)
        {
            float halfW = Width * transform.Scale.X * 0.5f;
            float halfH = Height * transform.Scale.Y * 0.5f;

            Vector2[] localVertices =
            [
                new Vector2(-halfW, -halfH), // BL
                new Vector2( halfW, -halfH), // BR
                new Vector2( halfW,  halfH), // TR
                new Vector2(-halfW,  halfH)  // TL
            ];

            float cos = MathF.Cos(transform.Rotation.Z);
            float sin = MathF.Sin(transform.Rotation.Z);

            Vector2[] worldVertices = new Vector2[4];
            for (int i = 0; i < 4; i++)
            {
                Vector2 v = localVertices[i] + Offset;
                float x = v.X * cos - v.Y * sin;
                float y = v.X * sin + v.Y * cos;

                worldVertices[i] = new Vector2(
                    x + transform.Position.X,
                    y + transform.Position.Y
                );
            }

            return worldVertices;
        }
    }
}
