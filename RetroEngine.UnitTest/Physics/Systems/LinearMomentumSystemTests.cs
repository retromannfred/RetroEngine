using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Physics;

namespace RetroEngine.UnitTest.Physics.Systems
{
    /// <summary>
    /// Defines unit test cases for LinearMomentumSystem class.
    /// </summary>
    public class LinearMomentumSystemTests
    {
        [Fact]
        public void LinearConservationSystem_ProcessHorizontallyWithDifferentVelocities_EntityReturnsWithOtherEntityVelocity()
        {
            // Arrange
            var system = new LinearMomentumSystem();
            var world = new WorldBuilder().RegisterSystem(system).Build();
            var entityA = world.CreateEntity()
                .Attach(new Transform() { Position = Vector3.UnitX * -1.5f })
                .Attach(new Collider2D() { Shape = Shape2D.Circle, Radius = 2 })
                .Attach(new LinearPhysics2D() { Velocity = Vector2.UnitX * 3 })
                .Attach(new RigidBody() { Mass = 6f });
            var entityB = world.CreateEntity()
                .Attach(new Transform() { Position = Vector3.UnitX * 1.5f })
                .Attach(new Collider2D() { Shape = Shape2D.Circle, Radius = 2 })
                .Attach(new LinearPhysics2D() { Velocity = Vector2.UnitX * -2 })
                .Attach(new RigidBody() { Mass = 6f });

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5)));

            // Assert
            Assert.Equal(Vector2.UnitX *  3, entityB.GetComponent<LinearPhysics2D>().Velocity);
            Assert.Equal(Vector2.UnitX * -2, entityA.GetComponent<LinearPhysics2D>().Velocity);
        }

        [Fact]
        public void LinearConservationSystem_ProcessVerticallyWithDifferentVelocities_EntityReturnsWithOtherEntityVelocity()
        {
            // Arrange
            var system = new LinearMomentumSystem();
            var world = new WorldBuilder().RegisterSystem(system).Build();
            var entityA = world.CreateEntity()
                .Attach(new Transform() { Position = Vector3.UnitY * -1.5f })
                .Attach(new Collider2D() { Shape = Shape2D.Circle, Radius = 2 })
                .Attach(new LinearPhysics2D() { Velocity = Vector2.UnitY * 3 })
                .Attach(new RigidBody() { Mass = 6f });
            var entityB = world.CreateEntity()
                .Attach(new Transform() { Position = Vector3.UnitY * 1.5f })
                .Attach(new Collider2D() { Shape = Shape2D.Circle, Radius = 2 })
                .Attach(new LinearPhysics2D() { Velocity = Vector2.UnitY * -2 })
                .Attach(new RigidBody() { Mass = 6f });

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5)));

            // Assert
            Assert.Equal(Vector2.UnitY *  3, entityB.GetComponent<LinearPhysics2D>().Velocity);
            Assert.Equal(Vector2.UnitY * -2, entityA.GetComponent<LinearPhysics2D>().Velocity);
        }

        [Fact]
        public void LinearConservationSystem_ProcessHorizontallyWithDifferentMass_EntityReturnsWithOtherEntityVelocityByMass()
        {
            // Arrange
            var system = new LinearMomentumSystem();
            var world = new WorldBuilder().RegisterSystem(system).Build();
            var entityA = world.CreateEntity()
                .Attach(new Transform() { Position = Vector3.UnitX * -1.5f })
                .Attach(new Collider2D() { Shape = Shape2D.Circle, Radius = 2 })
                .Attach(new LinearPhysics2D() { Velocity = Vector2.UnitX * 5 })
                .Attach(new RigidBody() { Mass = 30f });
            var entityB = world.CreateEntity()
                .Attach(new Transform() { Position = Vector3.UnitX * 1.5f })
                .Attach(new Collider2D() { Shape = Shape2D.Circle, Radius = 2 })
                .Attach(new LinearPhysics2D() { Velocity = Vector2.UnitX * -5 })
                .Attach(new RigidBody() { Mass = 6f });

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5)));
            var resultA = entityA.GetComponent<LinearPhysics2D>().Velocity;
            var resultB = entityB.GetComponent<LinearPhysics2D>().Velocity;

            // Assert
            Assert.Equal(Vector2.UnitX * 1.67f, new Vector2(MathF.Round(resultA.X, 2), MathF.Round(resultA.Y, 2)));
            Assert.Equal(Vector2.UnitX * 11.67f, new Vector2(MathF.Round(resultB.X, 2), MathF.Round(resultB.Y, 2)));
        }

        [Fact]
        public void LinearConservationSystem_ProcessVerticallyWithDifferentMass_EntityReturnsWithOtherEntityVelocityByMass()
        {
            // Arrange
            var system = new LinearMomentumSystem();
            var world = new WorldBuilder().RegisterSystem(system).Build();
            var entityA = world.CreateEntity()
                .Attach(new Transform() { Position = Vector3.UnitY * -1.5f })
                .Attach(new Collider2D() { Shape = Shape2D.Circle, Radius = 2 })
                .Attach(new LinearPhysics2D() { Velocity = Vector2.UnitY * 5 })
                .Attach(new RigidBody() { Mass = 30f });
            var entityB = world.CreateEntity()
                .Attach(new Transform() { Position = Vector3.UnitY * 1.5f })
                .Attach(new Collider2D() { Shape = Shape2D.Circle, Radius = 2 })
                .Attach(new LinearPhysics2D() { Velocity = Vector2.UnitY * -5 })
                .Attach(new RigidBody() { Mass = 6f });

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5)));
            var resultA = entityA.GetComponent<LinearPhysics2D>().Velocity;
            var resultB = entityB.GetComponent<LinearPhysics2D>().Velocity;

            // Assert
            Assert.Equal(Vector2.UnitY * 1.67f, new Vector2(MathF.Round(resultA.X, 2), MathF.Round(resultA.Y, 2)));
            Assert.Equal(Vector2.UnitY * 11.67f, new Vector2(MathF.Round(resultB.X, 2), MathF.Round(resultB.Y, 2)));
        }

        [Fact]
        public void LinearConservationSystem_ProcessHorizontallyWithDifferentRestitution_EntityReturnsWithLowestRestitution()
        {
            // Arrange
            var system = new LinearMomentumSystem();
            var world = new WorldBuilder().RegisterSystem(system).Build();
            var entityA = world.CreateEntity()
                .Attach(new Transform() { Position = Vector3.UnitX * -1.5f })
                .Attach(new Collider2D() { Shape = Shape2D.Circle, Radius = 2, Restitution = .25f })
                .Attach(new LinearPhysics2D() { Velocity = Vector2.UnitX * 5 })
                .Attach(new RigidBody() { Mass = 6f });
            var entityB = world.CreateEntity()
                .Attach(new Transform() { Position = Vector3.UnitX * 1.5f })
                .Attach(new Collider2D() { Shape = Shape2D.Circle, Radius = 2, Restitution = .75f })
                .Attach(new LinearPhysics2D() { Velocity = Vector2.UnitX * -5 })
                .Attach(new RigidBody() { Mass = 6f });

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5)));

            // Assert
            Assert.Equal(Vector2.UnitX * -1.25f, entityA.GetComponent<LinearPhysics2D>().Velocity);
            Assert.Equal(Vector2.UnitX *  1.25f, entityB.GetComponent<LinearPhysics2D>().Velocity);
        }

        [Fact]
        public void LinearConservationSystem_ProcessVerticallyWithDifferentRestitution_EntityReturnsWithLowestRestitution()
        {
            // Arrange
            var system = new LinearMomentumSystem();
            var world = new WorldBuilder().RegisterSystem(system).Build();
            var entityA = world.CreateEntity()
                .Attach(new Transform() { Position = Vector3.UnitY * -1.5f })
                .Attach(new Collider2D() { Shape = Shape2D.Circle, Radius = 2, Restitution = .25f })
                .Attach(new LinearPhysics2D() { Velocity = Vector2.UnitY * 5 })
                .Attach(new RigidBody() { Mass = 6f });
            var entityB = world.CreateEntity()
                .Attach(new Transform() { Position = Vector3.UnitY * 1.5f })
                .Attach(new Collider2D() { Shape = Shape2D.Circle, Radius = 2, Restitution = .75f })
                .Attach(new LinearPhysics2D() { Velocity = Vector2.UnitY * -5 })
                .Attach(new RigidBody() { Mass = 6f });

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5)));

            // Assert
            Assert.Equal(Vector2.UnitY * -1.25f, entityA.GetComponent<LinearPhysics2D>().Velocity);
            Assert.Equal(Vector2.UnitY *  1.25f, entityB.GetComponent<LinearPhysics2D>().Velocity);
        }
    }
}
