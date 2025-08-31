using RetroEngine.Graphics;

namespace RetroEngine.UnitTest.Graphics.Components
{
    /// <summary>
    /// Defines unit test cases for SpriteRenderer struct.
    /// </summary>
    public class SpriteRendererTests
    {
        [Fact]
        public void SpriteRenderer_FlipBothSides_GetsBothFlipedState()
        {
            // Arrange
            var renderer = new SpriteRenderer();

            // Act
            renderer.FlipHorizontally();
            renderer.FlipVertically();

            // Assert
            Assert.Equal(Flip.Both, renderer.Flip);
        }

        [Fact]
        public void SpriteRenderer_FlipHorizontallyTwice_DoesNothing()
        {
            // Arrange
            var renderer = new SpriteRenderer();

            // Act
            renderer.FlipHorizontally();
            renderer.FlipHorizontally();

            // Assert
            Assert.Equal(Flip.None, renderer.Flip);
        }

        [Fact]
        public void SpriteRenderer_FlipHorizontallyTwice_KeepsVerticalFlip()
        {
            // Arrange
            var renderer = new SpriteRenderer();

            // Act
            renderer.FlipHorizontally();
            renderer.FlipVertically();
            renderer.FlipHorizontally();

            // Assert
            Assert.Equal(Flip.Y, renderer.Flip);
        }



        [Fact]
        public void SpriteRenderer_FlipVerticallyTwice_DoesNothing()
        {
            // Arrange
            var renderer = new SpriteRenderer();

            // Act
            renderer.FlipVertically();
            renderer.FlipVertically();

            // Assert
            Assert.Equal(Flip.None, renderer.Flip);
        }

        [Fact]
        public void SpriteRenderer_FlipVerticallyTwice_KeepsHorizontalFlip()
        {
            // Arrange
            var renderer = new SpriteRenderer();

            // Act
            renderer.FlipVertically();
            renderer.FlipHorizontally();
            renderer.FlipVertically();

            // Assert
            Assert.Equal(Flip.X, renderer.Flip);
        }

        [Fact]
        public void SpriteRenderer_FlipHorizontally_ModifiesRendererFlip()
        {
            // Arrange
            var renderer = new SpriteRenderer();

            // Act
            renderer.FlipHorizontally();

            // Assert
            Assert.Equal(Flip.X, renderer.Flip);
        }

        [Fact]
        public void SpriteRenderer_FlipVertically_ModifiesRendererFlip()
        {
            // Arrange
            var renderer = new SpriteRenderer();

            // Act
            renderer.FlipVertically();

            // Assert
            Assert.Equal(Flip.Y, renderer.Flip);
        }
    }
}
