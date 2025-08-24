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
    /// <summary>
    /// Defines math methods that helps to calculate and resolve collisions.
    /// </summary>
    public static class CollisionMath
    {
        /// <summary>
        /// Checks if two circles are intersecting.
        /// </summary>
        /// <param name="centerA">Center of the first circle.</param>
        /// <param name="radiusA">Radius of the first circle.</param>
        /// <param name="centerB">Center of the second circle.</param>
        /// <param name="radiusB">Radius of the second circle.</param>
        /// <param name="direction">If both circles are intersected, returns normal vector indicating in which direction the first circle has intersected the second one.</param>
        /// <param name="depth">If both circles are intersected, returns the depth of the intersection between the two circles.</param>
        /// <returns>True if both circles are intersected, and false otherwise.</returns>
        public static bool IntersectCircles(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB, out Vector2 direction, out float depth)
        {
            direction = Vector2.Zero;
            depth = float.MaxValue;

            var radiusSum = radiusA + radiusB;
            var centerDistanceSquared = Vector2.DistanceSquared(centerA, centerB);

            if (centerDistanceSquared >= radiusSum * radiusSum)
            {
                return false;
            }

            var centerDistance = (float)MathHelper.Sqrt(centerDistanceSquared);
            depth = radiusSum - centerDistance;

            direction = Vector2.Normalize(centerB - centerA);

            return true;
        }

        /// <summary>
        /// Checks if two polygons are intersecting.
        /// </summary>
        /// <param name="verticesA">Vertices locations of the first polygon.</param>
        /// <param name="verticesB">Vertices locations of the second polygon.</param>
        /// <param name="direction">If both polygons are intersected, returns normal vector indicating in which direction the first polygon has intersected the second one.</param>
        /// <param name="depth">If both polygons are intersected, returns the depth of the intersection between the two polygons.</param>
        /// <returns>True if both polygons are intersected, and false otherwise.</returns>
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
    }
}
