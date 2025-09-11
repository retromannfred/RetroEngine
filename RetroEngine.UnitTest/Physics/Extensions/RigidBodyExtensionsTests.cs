using OpenTK.Mathematics;
using RetroEngine.Physics;

namespace RetroEngine.UnitTest.Physics.Extensions
{
    /// <summary>
    /// Defines unit test cases for LinearPhysics2DExtensions class.
    /// </summary>
    public class RigidBodyExtensionsTests
    {
        [Fact]
        public void RigidBody_ApplyForceWithMassOne_AddsCompleteVelocity()
        {
            // Arrange
            var body = new RigidBody()
            {
                Mass = 1
            };
            var linear = new LinearPhysics2D()
            {
                Velocity = new Vector2(2, 2)
            };

            // Act
            body.ApplyForce(ref linear, new Vector2(3, 4));

            // Assert
            Assert.Equal(new Vector2(5, 6), linear.Velocity);
        }

        [Fact]
        public void RigidBody_ApplyForceWithMassTen_AddsCompleteVelocityDividedByTen()
        {
            // Arrange
            var body = new RigidBody()
            {
                Mass = 10
            };
            var linear = new LinearPhysics2D()
            {
                Velocity = new Vector2(20, 20)
            };

            // Act
            body.ApplyForce(ref linear, new Vector2(30, 40));

            // Assert
            Assert.Equal(new Vector2(23, 24), linear.Velocity);
        }

        [Fact]
        public void RigidBody_ApplyForceAndItsSameNegativeForce_DoesNotChangeVelocity()
        {
            // Arrange
            var body = new RigidBody()
            {
                Mass = 2.4f
            };
            var linear = new LinearPhysics2D()
            {
                Velocity = new Vector2(2.6f, 7.8f)
            };

            // Act
            body.ApplyForce(ref linear, new Vector2( 4.7f, -5.2f));
            body.ApplyForce(ref linear, new Vector2(-4.7f,  5.2f));

            // Assert
            Assert.Equal(new Vector2(2.6f, 7.8f), linear.Velocity);
        }
    }
}
