using NUnit.Framework;
using OpenTK.Mathematics;
using RetroEngine.Ecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.UnitTest.Ecs.Components
{
    public class CameraTests
    {
        [Test]
        public void TestMoveForwardModifiesTransformPositionAddingFrontVector()
        {
            // Arrange
            var pos = new Vector3(4, -5, 6);
            var rot = new Vector3(-4, 5, -6);
            var trans = new Transform()
            {
                Position = pos,
                Rotation = rot
            };
            var cam = new Camera();

            // Act
            cam.MoveForward(ref trans, 1.5f);

            // Assert
            Assert.That(trans.Position, Is.EqualTo(new Vector3(4.9401927f, -3.8647966f, 5.721879f)));
        }

        [Test]
        public void TestMoveBackwardsModifiesTransformPositionSubstractingFrontVector()
        {
            // Arrange
            var pos = new Vector3(4, -5, 6);
            var rot = new Vector3(-4, 5, -6);
            var trans = new Transform()
            {
                Position = pos,
                Rotation = rot
            };
            var cam = new Camera();

            // Act
            var initialFront = cam.GetFrontDirection(trans);
            cam.MoveBackwards(ref trans, 1.5f);

            // Assert
            Assert.That(cam.GetFrontDirection(trans), Is.EqualTo(initialFront));
            Assert.That(trans.Position, Is.EqualTo(new Vector3(3.0598073f, -6.1352034f, 6.278121f)));
        }

        [Test]
        public void TestMoveLeftModifiesTransformPositionBasedOnFrontVector()
        {
            // Arrange
            var pos = new Vector3(4, -5, 6);
            var rot = new Vector3(-4, 5, -6);
            var trans = new Transform()
            {
                Position = pos,
                Rotation = rot
            };
            var cam = new Camera();

            // Act
            cam.MoveLeft(ref trans, 1.5f);

            // Assert
            Assert.That(trans.Position, Is.EqualTo(new Vector3(3.5745068f, -5, 4.5616136f)));
        }

        [Test]
        public void TestMoveRightModifiesTransformPositionBasedOnFrontVector()
        {
            // Arrange
            var pos = new Vector3(4, -5, 6);
            var rot = new Vector3(-4, 5, -6);
            var trans = new Transform()
            {
                Position = new Vector3(4, -5, 6),
                Rotation = new Vector3(-4, 5, -6)
            };
            var cam = new Camera();

            // Act
            cam.MoveRight(ref trans, 1.5f);

            // Assert
            Assert.That(trans.Position, Is.EqualTo(new Vector3(4.4254932f, -5f, 7.4383864f)));
        }

        [Test]
        public void TestMoveUpModifiesTransformPositionAddingUpVector()
        {
            // Arrange
            var pos = new Vector3(4, -5, 6);
            var rot = new Vector3(-4, 5, -6);
            var trans = new Transform()
            {
                Position = new Vector3(4, -5, 6),
                Rotation = new Vector3(-4, 5, -6)
            };
            var cam = new Camera();

            // Act
            cam.MoveUp(ref trans, 1.5f);

            // Assert
            Assert.That(trans.Position, Is.EqualTo(new Vector3(4f, -3.5f, 6f)));
        }

        [Test]
        public void TestMoveDownModifiesTransformPositionSubstractingFrontVector()
        {
            // Arrange
            var pos = new Vector3(4, -5, 6);
            var rot = new Vector3(-4, 5, -6);
            var trans = new Transform()
            {
                Position = new Vector3(4, -5, 6),
                Rotation = new Vector3(-4, 5, -6)
            };
            var cam = new Camera();

            // Act
            cam.MoveDown(ref trans, 1.5f);

            // Assert
            Assert.That(trans.Position, Is.EqualTo(new Vector3(4f, -6.5f, 6f)));
        }

        [Test]
        public void TestCallAllMovesWithSameModuleEndsInSamePosition()
        {
            // Arrange
            var pos = new Vector3(4, -5, 6);
            var rot = new Vector3(-4, 5, -6);
            var trans = new Transform()
            {
                Position = new Vector3(4, -5, 6),
                Rotation = new Vector3(-4, 5, -6)
            };
            var cam = new Camera();

            // Act
            cam.MoveForward(ref trans, 1.5f);
            cam.MoveBackwards(ref trans, 1.5f);
            cam.MoveUp(ref trans, 1.5f);
            cam.MoveDown(ref trans, 1.5f);
            cam.MoveLeft(ref trans, 1.5f);
            cam.MoveRight(ref trans, 1.5f);

            // Assert
            Assert.That(trans.Position, Is.EqualTo(pos));
        }
    }
}
