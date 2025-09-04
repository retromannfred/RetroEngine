using OpenTK.Mathematics;
using RetroEngine.Core;

namespace RetroEngine.UnitTest.Core.Components
{
    /// <summary>
    /// Defines unit test cases for Transform struct.
    /// </summary>
    public class TransformTests
    {
        [Theory]
        [InlineData(-12, 5, 7, 4, 18, 22)]
        [InlineData(46, 17, 20, 19, 28, 10)]
        [InlineData(-30, -7, 2, 46, 9, -22)]
        [InlineData(5, -10, 38, -13, -16, -6)]
        public void Transform_CallingTranslate_SumsValueToPositionVector(float ix, float iy, float iz, float tx, float ty, float tz)
        {
            // Arrange
            var initial = new Vector3(ix, iy, iz);
            var translation = new Vector3(tx, ty, tz);
            var expected = initial + translation;
            var transform = new Transform() { Position = initial };

            // Act
            transform.Translate(translation);

            // Assert
            Assert.Equal(expected, transform.Position);
        }

        [Theory]
        [InlineData(-12, 5, 7, 4, 18, 22)]
        [InlineData(46, 17, 20, 19, 28, 10)]
        [InlineData(-30, -7, 2, 46, 9, -22)]
        [InlineData(5, -10, 38, -13, -16, -6)]
        public void Transform_CallingRotate_SumsAndClampsRadiansValueToRotationVector(float ix, float iy, float iz, float rx, float ry, float rz)
        {
            // Arrange
            var initial = new Vector3(
                MathHelper.ClampRadians(ix),
                MathHelper.ClampRadians(iy),
                MathHelper.ClampRadians(iz));
            var rotation = new Vector3(rx, ry, rz);
            var expected = initial + rotation;
            var expectedClamped = new Vector3(
                MathHelper.ClampRadians(expected.X),
                MathHelper.ClampRadians(expected.Y),
                MathHelper.ClampRadians(expected.Z));
            var transform = new Transform() { Rotation = initial };

            // Act
            transform.Rotate(rotation);

            // Assert
            Assert.Equal(expectedClamped, transform.Rotation);
        }

        [Theory]
        [InlineData(-12, 5, 7, 4, 18, 22)]
        [InlineData(46, 17, 20, 19, 28, 10)]
        [InlineData(-30, -7, 2, 46, 9, -22)]
        [InlineData(5, -10, 38, -13, -16, -6)]
        public void Transform_CallingRescale_MultipliesValueToScaleVector(float ix, float iy, float iz, float sx, float sy, float sz)
        {
            // Arrange
            var initial = new Vector3(ix, iy, iz);
            var scaling = new Vector3(sx, sy, sz);
            var expected = initial * scaling;
            var transform = new Transform() { Scale = initial };

            // Act
            transform.Rescale(scaling);

            // Assert
            Assert.Equal(expected, transform.Scale);
        }
    }
}
