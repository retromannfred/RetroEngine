using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Graphics;
using System.Threading.Tasks.Dataflow;

namespace RetroEngine.UnitTest.Graphics.Components
{
    /// <summary>
    /// Defines unit test cases for Camera struct.
    /// </summary>
    public class CameraTests
    {
        [Fact]
        public void Camera_GetClipSapces_ReturnsProperView()
        {
            // Arrange
            var expected = Matrix4.LookAt(Vector3.Zero, new Vector3(0f, 1f, -1f), Vector3.UnitY);
            var camera = new Camera();
            var transform = new Transform() { Rotation = new Vector3(MathHelper.PiOver4, 0f, 0f) };
            camera.UpdateCameraVectors(transform);

            // Act
            var view = camera.GetClipSpace(transform, 800, 600).View;

            // Assert
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    expected[i, j] = (float)MathHelper.Round(expected[i, j], 3);
                    view[i, j] = (float)MathHelper.Round(view[i, j], 3);
                }
            }
            Assert.Equal(expected, view);
        }

        [Fact]
        public void Camera_GetClipSapces_ReturnsProperPerspectiveProjection()
        {
            // Arrange
            var expected = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 800f / 600f, .5f, 500f);
            var camera = new Camera() { ClippingNear = .5f, ClippingFar = 500f, FieldOfView = MathHelper.PiOver4, Projection = ProjectionType.Perspective };
            var transform = new Transform() { Rotation = new Vector3(MathHelper.PiOver4, 0f, 0f) };
            camera.UpdateCameraVectors(transform);

            // Act
            var projection = camera.GetClipSpace(transform, 800, 600).Projection;

            // Assert
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    expected[i, j] = (float)MathHelper.Round(expected[i, j], 3);
                    projection[i, j] = (float)MathHelper.Round(projection[i, j], 3);
                }
            }
            Assert.Equal(expected, projection);
        }

        [Fact]
        public void Camera_GetClipSapces_ReturnsProperOrthographicProjection()
        {
            // Arrange
            var expected = Matrix4.CreateOrthographicOffCenter(-8, 8f, -6f, 6f, .5f, 500f);
            var camera = new Camera() { ClippingNear = .5f, ClippingFar = 500f, FieldOfView = MathHelper.PiOver4, Projection = ProjectionType.Orthographic };
            var transform = new Transform() { Rotation = new Vector3(MathHelper.PiOver4, 0f, 0f) };
            camera.UpdateCameraVectors(transform);

            // Act
            var projection = camera.GetClipSpace(transform, 800, 600).Projection;

            // Assert
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    expected[i, j] = (float)MathHelper.Round(expected[i, j], 3);
                    projection[i, j] = (float)MathHelper.Round(projection[i, j], 3);
                }
            }
            Assert.Equal(expected, projection);
        }

        [Fact]
        public void Camera_MoveForward_MovesTransformToTheCameraFront()
        {
            // Arrange
            var expected = new Vector3(0f, .5f, -.87f);
            var transform = new Transform() { Rotation = new Vector3(MathHelper.PiOver6, 0f, 0f) };
            var camera = new Camera();

            // Act
            camera.UpdateCameraVectors(transform);
            transform = camera.MoveForward(transform, 1f);
            transform.Position = new Vector3(
                MathF.Round(transform.Position.X, 2),
                MathF.Round(transform.Position.Y, 2),
                MathF.Round(transform.Position.Z, 2)
            );

            // Assert
            Assert.Equal(expected, transform.Position);
        }

        [Fact]
        public void Camera_MoveBackwards_MovesTransformToTheCameraBack()
        {
            // Arrange
            var expected = new Vector3(0f, -.5f, .87f);
            var transform = new Transform() { Rotation = new Vector3(MathHelper.PiOver6, 0f, 0f) };
            var camera = new Camera();

            // Act
            camera.UpdateCameraVectors(transform);
            transform = camera.MoveBackwards(transform, 1f);
            transform.Position = new Vector3(
                MathF.Round(transform.Position.X, 2),
                MathF.Round(transform.Position.Y, 2),
                MathF.Round(transform.Position.Z, 2)
            );

            // Assert
            Assert.Equal(expected, transform.Position);
        }

        [Fact]
        public void Camera_MoveRight_MovesTransformToTheCameraRight()
        {
            // Arrange
            var expected = new Vector3(.87f, 0f, .5f);
            var transform = new Transform() { Rotation = new Vector3(0f, MathHelper.PiOver6, 0f) };
            var camera = new Camera();

            // Act
            camera.UpdateCameraVectors(transform);
            transform = camera.MoveRight(transform, 1f);
            transform.Position = new Vector3(
                MathF.Round(transform.Position.X, 2),
                MathF.Round(transform.Position.Y, 2),
                MathF.Round(transform.Position.Z, 2)
            );

            // Assert
            Assert.Equal(expected, transform.Position);
        }

        [Fact]
        public void Camera_MoveLeft_MovesTransformToTheCameraLeft()
        {
            // Arrange
            var expected = new Vector3(-.87f, 0f, -.5f);
            var transform = new Transform() { Rotation = new Vector3(0f, MathHelper.PiOver6, 0f) };
            var camera = new Camera();

            // Act
            camera.UpdateCameraVectors(transform);
            transform = camera.MoveLeft(transform, 1f);
            transform.Position = new Vector3(
                MathF.Round(transform.Position.X, 2),
                MathF.Round(transform.Position.Y, 2),
                MathF.Round(transform.Position.Z, 2)
            );

            // Assert
            Assert.Equal(expected, transform.Position);
        }

        [Fact]
        public void Camera_MoveUp_MovesTransformToThePositiveY()
        {
            // Arrange
            var expected = new Vector3(0f, 1f, 0f);
            var transform = new Transform() { Rotation = new Vector3(MathHelper.PiOver6, MathHelper.PiOver6, 0f) };
            var camera = new Camera();

            // Act
            camera.UpdateCameraVectors(transform);
            transform = camera.MoveUp(transform, 1f);
            transform.Position = new Vector3(
                MathF.Round(transform.Position.X, 2),
                MathF.Round(transform.Position.Y, 2),
                MathF.Round(transform.Position.Z, 2)
            );

            // Assert
            Assert.Equal(expected, transform.Position);
        }

        [Fact]
        public void Camera_MoveDown_MovesTransformToTheNegativeY()
        {
            // Arrange
            var expected = new Vector3(0f, -1f, 0f);
            var transform = new Transform() { Rotation = new Vector3(MathHelper.PiOver6, MathHelper.PiOver6, 0f) };
            var camera = new Camera();

            // Act
            camera.UpdateCameraVectors(transform);
            transform = camera.MoveDown(transform, 1f);
            transform.Position = new Vector3(
                MathF.Round(transform.Position.X, 2),
                MathF.Round(transform.Position.Y, 2),
                MathF.Round(transform.Position.Z, 2)
            );

            // Assert
            Assert.Equal(expected, transform.Position);
        }

        [Fact]
        public void Camera_LookUp_SumsCameraPitch()
        {
            // Arrange
            var expected = new Vector3(MathHelper.PiOver6, 0f, 0f);
            var transform = new Transform();
            var camera = new Camera();

            // Act
            camera.UpdateCameraVectors(transform);
            transform = camera.LookUp(transform, MathHelper.PiOver6);

            // Assert
            Assert.Equal(expected, transform.Rotation);
        }

        [Fact]
        public void Camera_LookDown_SubstractsCameraPitch()
        {
            // Arrange
            var expected = new Vector3(MathHelper.ClampRadians(-MathHelper.PiOver6), 0f, 0f);
            var transform = new Transform();
            var camera = new Camera();

            // Act
            camera.UpdateCameraVectors(transform);
            transform = camera.LookDown(transform, MathHelper.PiOver6);

            // Assert
            Assert.Equal(expected, transform.Rotation);
        }

        [Fact]
        public void Camera_LookLeft_SumsCameraYawn()
        {
            // Arrange
            var expected = new Vector3(0f, MathHelper.ClampRadians(-MathHelper.PiOver6), 0f);
            var transform = new Transform();
            var camera = new Camera();

            // Act
            camera.UpdateCameraVectors(transform);
            transform = camera.LookLeft(transform, MathHelper.PiOver6);

            // Assert
            Assert.Equal(expected, transform.Rotation);
        }

        [Fact]
        public void Camera_LookRight_SubstractsCameraYawn()
        {
            // Arrange
            var expected = new Vector3(0f, MathHelper.PiOver6, 0f);
            var transform = new Transform();
            var camera = new Camera();

            // Act
            camera.UpdateCameraVectors(transform);
            transform = camera.LookRight(transform, MathHelper.PiOver6);

            // Assert
            Assert.Equal(expected, transform.Rotation);
        }
    }
}
