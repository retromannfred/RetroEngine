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
    public class TransformTests
    {
        [Test]
        public void TestAsignRotationValueClampsAngles()
        {
            // Arrange
            var trans = new Transform();

            // Act
            trans.Rotation = new Vector3(
                -MathHelper.PiOver2,
                (float)MathHelper.Round(MathHelper.ClampRadians(MathHelper.Pi * 3f), 6),
                MathHelper.Pi * 2f
            );

            // Assert
            Assert.That(trans.Rotation, Is.EqualTo(
                new Vector3(
                    MathHelper.ThreePiOver2,
                    (float)MathHelper.Round(MathHelper.ClampRadians(MathHelper.Pi), 6),
                    0f
                )
            ));
        }

        [Test]
        public void TestCallRotateClampsAngles()
        {
            // Arrange
            var trans = new Transform()
            {
                Rotation = new Vector3(0f, 0f, 0f)
            };

            // Act
            trans.Rotate(new Vector3(
                -MathHelper.PiOver2,
                (float)MathHelper.Round(MathHelper.ClampRadians(MathHelper.Pi * 3f), 6),
                MathHelper.Pi * 2f
            ));

            // Assert
            Assert.That(trans.Rotation, Is.EqualTo(
                new Vector3(
                    MathHelper.ThreePiOver2,
                    (float)MathHelper.Round(MathHelper.ClampRadians(MathHelper.Pi), 6),
                    0f
                )
            ));
        }

        [Test]
        public void TestTranslateModifiesPosition()
        {
            // Arrange
            var trans = new Transform()
            {
                Position = new Vector3(4f, -5f, 6f)
            };

            // Act
            trans.Translate(new Vector3(-3f, 4f, -5f));

            // Assert
            Assert.That(trans.Position, Is.EqualTo(new Vector3(1f, -1f, 1f)));
        }

        [Test]
        public void TestRotateModifiesRotation()
        {
            // Arrange
            var trans = new Transform()
            {
                Rotation = new Vector3(4f, -5f, 6f)
            };

            // Act
            trans.Rotate(new Vector3(-3f, 4f, -5f));

            // Assert
            Assert.That(trans.Rotation, Is.EqualTo(new Vector3(1f, MathHelper.TwoPi - 1f, 1f)));
        }

        [Test]
        public void TestRescalateModifiesScale()
        {
            // Arrange
            var trans = new Transform()
            {
                Scale = new Vector3(4f, -5f, 6f)
            };

            // Act
            trans.Rescale(new Vector3(-3f, 4f, -5f));

            // Assert
            Assert.That(trans.Scale, Is.EqualTo(new Vector3(-12f, -20f, -30f)));
        }
    }
}
