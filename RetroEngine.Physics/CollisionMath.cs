using OpenTK.Mathematics;
using RetroEngine.Core.Components;
using RetroEngine.Physics.Components;
using RetroEngine.Physics.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Physics
{
    public static class CollisionMath
    {
        public static bool Intersects(
            Transform transformA, Collider2D colliderA,
            Transform transformB, Collider2D colliderB,
            out Vector2 direction, out float depth)
        {
            direction = Vector2.Zero;
            depth = 0;

            if (colliderA.Shape == Shapes2D.Circle && colliderB.Shape == Shapes2D.Circle)
            {
                return CollisionMath.IntersectCircles(
                    transformA.Position.Xy + colliderA.Offset, colliderA.Radius,
                    transformB.Position.Xy + colliderB.Offset, colliderB.Radius,
                    out direction, out depth);
            }
            else if (colliderA.Shape == Shapes2D.Rectangle && colliderB.Shape == Shapes2D.Rectangle)
            {
                var verticesA = CollisionMath.GetRectangleVertices(transformA, colliderA);
                var verticesB = CollisionMath.GetRectangleVertices(transformB, colliderB);

                return CollisionMath.IntersectPolygons(verticesA, verticesB, out direction, out depth);
            }

            return false;
        }

        public static bool IntersectCircles(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB, out Vector2 direction, out float depth)
        {
            direction = Vector2.Zero;
            depth = float.MaxValue;

            var radiusSum = radiusA + radiusB;
            var centerDistanceSquared = Vector2.DistanceSquared(centerA, centerB);

            if (centerDistanceSquared < radiusSum * radiusSum)
            {
                var centerDistance = (float)MathHelper.Sqrt(centerDistanceSquared);
                depth = radiusSum - centerDistance;

                direction = Vector2.Normalize(centerB - centerA);

                return true;
            }

            return false;
        }

        public static bool IntersectPolygons(Vector2[] verticesA, Vector2[] verticesB, out Vector2 direction, out float depth)
        {
            direction = Vector2.Zero;
            depth = float.MaxValue;

            for (int i = 0; i < verticesA.Length; i++)
            {
                var vi = verticesA[i];
                var vj = verticesA[(i + 1) % verticesA.Length];

                var edge = vj - vi;
                var axis = new Vector2(-edge.Y, edge.X);

                ProjectVertices(verticesA, axis, out float minA, out float maxA);
                ProjectVertices(verticesB, axis, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA)
                    return false;

                float axisDepth = MathF.Min(maxB - minA, maxA - minB);
                if (axisDepth < depth)
                {
                    depth = axisDepth;
                    direction = axis;
                }
            }

            for (int i = 0; i < verticesB.Length; i++)
            {
                var vi = verticesB[i];
                var vj = verticesB[(i + 1) % verticesB.Length];

                var edge = vj - vi;
                var axis = new Vector2(-edge.Y, edge.X);

                ProjectVertices(verticesB, axis, out float minB, out float maxB);
                ProjectVertices(verticesA, axis, out float minA, out float maxA);

                if (minA >= maxB || minB >= maxA)
                    return false;

                float axisDepth = MathF.Min(maxA - minB, maxB - minA);
                if (axisDepth < depth)
                {
                    depth = axisDepth;
                    direction = axis;
                }
            }

            // *** Avoiding double SQRT trick ***
            // Vector2.Length does a sqrt(x^2 + y^2), and normalize use that length to scale down the its components.
            // So we precalculate Length first to use it to adjust depth, and later to scale down the direction, so we normalize it.

            var dirLength = direction.Length;
            depth /= dirLength;
            direction /= dirLength;

            return true;
        }

        private static void ProjectVertices(Vector2[] vertices, Vector2 axis, out float min, out float max)
        {
            min = float.MaxValue;
            max = float.MinValue;

            for (int i = 0; i < vertices.Length; i++)
            {
                var projection = Vector2.Dot(vertices[i], axis);

                if (projection < min)
                    min = projection;

                if (projection > max)
                    max = projection;
            }
        }

        public static Vector2[] GetRectangleVertices(Transform transform, Collider2D collider)
        {
            // Dimensiones con escala aplicada
            float halfW = collider.Width * transform.Scale.X * 0.5f;
            float halfH = collider.Height * transform.Scale.Y * 0.5f;

            // Vértices locales del collider (antes de rotación/traslación)
            Vector2[] localVertices =
            [
                new Vector2(-halfW, -halfH), // BL
                new Vector2( halfW, -halfH), // BR
                new Vector2( halfW,  halfH), // TR
                new Vector2(-halfW,  halfH)  // TL
            ];

            // Rotación Z (2D)
            float cos = MathF.Cos(transform.Rotation.Z);
            float sin = MathF.Sin(transform.Rotation.Z);

            Vector2[] worldVertices = new Vector2[4];
            for (int i = 0; i < 4; i++)
            {
                // Aplico offset
                Vector2 v = localVertices[i] + collider.Offset;

                // Rotación alrededor del origen
                float x = v.X * cos - v.Y * sin;
                float y = v.X * sin + v.Y * cos;

                // Traslación (posición del transform)
                worldVertices[i] = new Vector2(
                    x + transform.Position.X,
                    y + transform.Position.Y
                );
            }

            return worldVertices;
        }
    }
}
