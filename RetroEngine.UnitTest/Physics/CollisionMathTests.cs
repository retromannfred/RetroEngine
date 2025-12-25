using OpenTK.Mathematics;
using RetroEngine.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.UnitTest.Physics
{
    /// <summary>
    /// Defines unit test cases for CollisionMath class.
    /// </summary>
    public class CollisionMathTests
    {
        [Fact]
        public void CollisionMath_IntersectCircles_ReturnFalseWhenNotColliding()
        {
            // Arrange
            var centarA = new Vector2(0f, 4f);
            var centarB = new Vector2(-4f, 0f);
            var radiusA = 2f;
            var radiusB = 2f;

            // Act
            var result = CollisionMath.IntersectCircles(centarA, radiusA, centarB, radiusB, out var direction, out var depth);

            // Assert
            Assert.False(result);
            Assert.Equal(Vector2.Zero, direction);
            Assert.Equal(float.MaxValue, depth);
        }

        [Fact]
        public void CollisionMath_IntersectCircles_ReturnTrueWithDataWhenColliding()
        {
            // Arrange
            var centarA = new Vector2(0f, 2f);
            var centarB = new Vector2(-2f, 0f);
            var radiusA = 2f;
            var radiusB = 2f;

            // Act
            var result = CollisionMath.IntersectCircles(centarA, radiusA, centarB, radiusB, out var direction, out var depth);

            // Assert
            Assert.True(result);
            Assert.Equal(Vector2.One * -0.707f, new Vector2((float)Math.Round(direction.X, 3), (float)Math.Round(direction.Y, 3)));
            Assert.Equal(4 - (float)MathHelper.Sqrt(8), depth);
        }

        [Fact]
        public void CollisionMath_IntersectPolygons_ReturnFalseWhenNotColliding()
        {
            // Arrange
            var triangleVertices = new Vector2[]
            {
                new ( 0f,  2f),
                new (-2f, -2f),
                new ( 2f, -2f)
            };

            var pentagonVertices = new Vector2[]
            {
                new ( 0f, 2.5f),
                new (-2f, 3f),
                new (-2f, 5f),
                new ( 2f, 5f),
                new ( 2f, 3f)
            };

            // Act
            var result = CollisionMath.IntersectPolygons(triangleVertices, pentagonVertices, out var direction, out var depth);
            Assert.Equal(new Vector2(4f, -2f), direction);
            Assert.Equal(6, depth);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CollisionMath_IntersectPolygons_ReturnTrueWithDataWhenColliding()
        {
            // Arrange
            var triangleVertices = new Vector2[]
            {
                new ( 0f,  2f),
                new (-2f, -2f),
                new ( 2f, -2f)
            };

            var pentagonVertices = new Vector2[]
            {
                new ( 0f, 1f),
                new (-2f, 1.5f),
                new (-2f, 3.5f),
                new ( 2f, 3.5f),
                new ( 2f, 1.5f)
            };

            // Act
            var result = CollisionMath.IntersectPolygons(triangleVertices, pentagonVertices, out var direction, out var depth);

            // Assert
            Assert.True(result);
            Assert.Equal(
                new Vector2((float)MathHelper.Round(-.2425f, 4), (float)MathHelper.Round(-.9701f, 4)),
                new Vector2((float)MathHelper.Round(direction.X, 4), (float)MathHelper.Round(direction.Y, 4)));
            Assert.Equal(.970142543f, depth);
        }
    }
}
