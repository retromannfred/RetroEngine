using RetroEngine.Physics;

namespace RetroEngine.UnitTest.Physics.Extensions
{
    /// <summary>
    /// Defines unit test cases for LinearPhysics2DExtensions class.
    /// </summary>
    public class LinearPhysics2DExtensionsTests
    {
        [Theory]
        [InlineData(FreezedMovement.None)]
        [InlineData(FreezedMovement.Horizontal)]
        [InlineData(FreezedMovement.Vertical)]
        [InlineData(FreezedMovement.Both)]
        public void LinearPhysics2D_FreezingHorizontally_SetsProperFlagValue(FreezedMovement arrangedFreeze)
        {
            // Arrange
            var linear = new LinearPhysics2D()
            {
                FreezedMovement = arrangedFreeze
            };

            // Act
            linear.FreezeHorizontally();

            // Assert
            Assert.Equal(FreezedMovement.Horizontal, linear.FreezedMovement & FreezedMovement.Horizontal);
        }

        [Theory]
        [InlineData(FreezedMovement.None)]
        [InlineData(FreezedMovement.Horizontal)]
        [InlineData(FreezedMovement.Vertical)]
        [InlineData(FreezedMovement.Both)]
        public void LinearPhysics2D_FreezingVertically_SetsProperFlagValue(FreezedMovement arrangedFreeze)
        {
            // Arrange
            var linear = new LinearPhysics2D()
            {
                FreezedMovement = arrangedFreeze
            };

            // Act
            linear.FreezeVertically();

            // Assert
            Assert.Equal(FreezedMovement.Vertical, linear.FreezedMovement & FreezedMovement.Vertical);
        }

        [Theory]
        [InlineData(FreezedMovement.None)]
        [InlineData(FreezedMovement.Horizontal)]
        [InlineData(FreezedMovement.Vertical)]
        [InlineData(FreezedMovement.Both)]
        public void LinearPhysics2D_UnfreezingHorizontally_SetsProperFlagValue(FreezedMovement arrangedFreeze)
        {
            // Arrange
            var linear = new LinearPhysics2D()
            {
                FreezedMovement = arrangedFreeze
            };

            // Act
            linear.UnfreezeHorizontally();

            // Assert
            Assert.Equal(FreezedMovement.None, linear.FreezedMovement & FreezedMovement.Horizontal);
        }

        [Theory]
        [InlineData(FreezedMovement.None)]
        [InlineData(FreezedMovement.Horizontal)]
        [InlineData(FreezedMovement.Vertical)]
        [InlineData(FreezedMovement.Both)]
        public void LinearPhysics2D_UnfreezingVertically_SetsProperFlagValue(FreezedMovement arrangedFreeze)
        {
            // Arrange
            var linear = new LinearPhysics2D()
            {
                FreezedMovement = arrangedFreeze
            };

            // Act
            linear.UnfreezeVertically();

            // Assert
            Assert.Equal(FreezedMovement.None, linear.FreezedMovement & FreezedMovement.Vertical);
        }

        [Theory]
        [InlineData(FreezedMovement.None)]
        [InlineData(FreezedMovement.Horizontal)]
        [InlineData(FreezedMovement.Vertical)]
        [InlineData(FreezedMovement.Both)]
        public void LinearPhysics2D_TogglingHorizontally_SetsProperFlagValue(FreezedMovement arrangedFreeze)
        {
            // Arrange
            var linear = new LinearPhysics2D()
            {
                FreezedMovement = arrangedFreeze
            };

            // Act
            linear.ToggleHorizontalFreeze();

            // Assert
            Assert.True((arrangedFreeze & FreezedMovement.Horizontal) != (linear.FreezedMovement & FreezedMovement.Horizontal));
        }

        [Theory]
        [InlineData(FreezedMovement.None)]
        [InlineData(FreezedMovement.Horizontal)]
        [InlineData(FreezedMovement.Vertical)]
        [InlineData(FreezedMovement.Both)]
        public void LinearPhysics2D_TogglingVertically_SetsProperFlagValue(FreezedMovement arrangedFreeze)
        {
            // Arrange
            var linear = new LinearPhysics2D()
            {
                FreezedMovement = arrangedFreeze
            };

            // Act
            linear.ToggleVertialFreeze();

            // Assert
            Assert.True((arrangedFreeze & FreezedMovement.Vertical) != (linear.FreezedMovement & FreezedMovement.Vertical));
        }
    }
}
