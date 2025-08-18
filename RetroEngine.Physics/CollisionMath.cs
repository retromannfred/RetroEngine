using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Physics
{
    public static class CollisionMath
    {
        public static bool IntersectCircles(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB, out Vector2 direction, out float depth)
        {
            direction = Vector2.Zero;
            depth = float.MaxValue;

            var radiusSum = radiusA + radiusB;
            var centerDistance = Vector2.Distance(centerA, centerB);

            if (centerDistance < radiusSum)
            {
                direction = Vector2.Normalize(centerB - centerA);
                depth = radiusSum - centerDistance;

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

                ProjectVertices(verticesA, axis, out float minA, out float maxA);
                ProjectVertices(verticesB, axis, out float minB, out float maxB);

                if (minB >= maxA || minA >= maxB)
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

            var dirLength =  direction.Length;
            depth /= dirLength;
            direction = new Vector2(direction.X / dirLength, direction.Y / dirLength);

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
    }
}
