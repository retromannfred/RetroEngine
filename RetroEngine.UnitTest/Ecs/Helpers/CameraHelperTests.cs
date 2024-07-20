using NUnit.Framework;
using OpenTK.Mathematics;
using RetroEngine.Ecs.Components;
using RetroEngine.Ecs.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.UnitTest.Ecs.Helpers
{
    public class CameraHelperTests
    {
        [Test]
        public void TestLookBackwardsGeneratesProperFrontVector()
        {
            // Arrange
            var trans = new Transform()
            {
                Position = new Vector3(4, -5, 6),
                Rotation = new Vector3(MathHelper.Pi, 0f, 0f)
            };

            // Act
            var front = CameraHelper.GetFrontDirection(trans);

            // Assert
            Assert.That(front, Is.EqualTo(new Vector3(0f, 0f, -1f)));
        }

        [Test]
        public void TestLookLeftGeneratesProperFrontVector()
        {
            // Arrange
            var trans = new Transform()
            {
                Position = new Vector3(4, -5, 6),
                Rotation = new Vector3(0, -MathHelper.PiOver2, 0f)
            };

            // Act
            var front = CameraHelper.GetFrontDirection(trans);

            // Assert
            Assert.That(front, Is.EqualTo(new Vector3(-1f, 0f, 0f)));
        }

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

            // Act
            CameraHelper.MoveForward(ref trans, 1.5f);

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

            // Act
            var initialFront = CameraHelper.GetFrontDirection(trans);
            CameraHelper.MoveBackwards(ref trans, 1.5f);

            // Assert
            Assert.That(CameraHelper.GetFrontDirection(trans), Is.EqualTo(initialFront));
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

            // Act
            CameraHelper.MoveLeft(ref trans, 1.5f);

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

            // Act
            CameraHelper.MoveRight(ref trans, 1.5f);

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

            // Act
            CameraHelper.MoveUp(ref trans, 1.5f);

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

            // Act
            CameraHelper.MoveDown(ref trans, 1.5f);

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

            // Act
            CameraHelper.MoveForward(ref trans, 1.5f);
            CameraHelper.MoveBackwards(ref trans, 1.5f);
            CameraHelper.MoveUp(ref trans, 1.5f);
            CameraHelper.MoveDown(ref trans, 1.5f);
            CameraHelper.MoveLeft(ref trans, 1.5f);
            CameraHelper.MoveRight(ref trans, 1.5f);

            // Assert
            Assert.That(trans.Position, Is.EqualTo(pos));
        }

        [Test]
        public void TestLookUpModifiesTransformRotation()
        {
            // Arrange
            var trans = new Transform()
            {
                Position = new Vector3(4, -5, 6),
                Rotation = new Vector3(0, 0, 0)
            };

            // Act
            CameraHelper.LookUp(ref trans, 1.5f);

            // Assert
            Assert.That(trans.Rotation, Is.EqualTo(new Vector3(1.5f, 0f, 0f)));
        }

        [Test]
        public void TestLookDownModifiesTransformRotation()
        {
            // Arrange
            var trans = new Transform()
            {
                Position = new Vector3(4, -5, 6),
                Rotation = new Vector3(0, 0, 0)
            };

            // Act
            CameraHelper.LookDown(ref trans, 1.5f);

            // Assert
            Assert.That(trans.Rotation, Is.EqualTo(new Vector3(MathHelper.TwoPi - 1.5f, 0f, 0f)));
        }

        [Test]
        public void TestLookLeftModifiesTransformRotation()
        {
            // Arrange
            var trans = new Transform()
            {
                Position = new Vector3(4, -5, 6),
                Rotation = new Vector3(0, 0, 0)
            };

            // Act
            CameraHelper.LookLeft(ref trans, 1.5f);

            // Assert
            Assert.That(trans.Rotation, Is.EqualTo(new Vector3(0f, MathHelper.TwoPi - 1.5f, 0f)));
        }

        [Test]
        public void TestLookRightModifiesTransformRotation()
        {
            // Arrange
            var trans = new Transform()
            {
                Position = new Vector3(4, -5, 6),
                Rotation = new Vector3(0, 0, 0)
            };

            // Act
            CameraHelper.LookRight(ref trans, 1.5f);

            // Assert
            Assert.That(trans.Rotation, Is.EqualTo(new Vector3(0f, 1.5f, 0f)));
        }
    }
}
