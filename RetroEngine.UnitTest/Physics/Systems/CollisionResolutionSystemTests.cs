using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Physics;

namespace RetroEngine.UnitTest.Physics.Systems
{
    /// <summary>
    /// Defines unit test cases for CollisionResolutionSystem class.
    /// </summary>
    public class CollisionResolutionSystemTests
    {
        [Fact]
        public void CollisionResolutionSystem_ProcessNonCollidingCircles_DoesntModifyThem()
        {
            // Arrange
            var system = new CollisionResolutionSystem();
            var world = new WorldBuilder().RegisterSystem(system).Build();
            for (int i = 0; i < 10; i++)
            {
                world.CreateEntity()
                    .Attach(new Transform()
                    {
                        Position = new Vector3(5 * i, 0, 0)
                    })
                    .Attach(new Collider2D()
                    {
                        Shape = Shape2D.Circle,
                        Radius = 2
                    });
            }

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2)));

            // Assert
            for (int i = 0; i < 10; i++)
            {
                var transform = world.GetComponent<Transform>(i + 1);
                Assert.Equal(new Vector3(5 * i, 0, 0), transform.Position);
            }
        }

        [Fact]
        public void CollisionResolutionSystem_ProcessCollidingCircles_ModifiesTheirPosition()
        {
            // Arrange
            var system = new CollisionResolutionSystem();
            var world = new WorldBuilder().RegisterSystem(system).Build();
            for (int i = 0; i < 10; i++)
            {
                world.CreateEntity()
                    .Attach(new Transform()
                    {
                        Position = new Vector3(0, 0, 0)
                    })
                    .Attach(new Collider2D()
                    {
                        Shape = Shape2D.Circle,
                        Radius = 2
                    });
            }

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2)));

            // Assert
            for (int i = 0; i < 10; i++)
            {
                var transform = world.GetComponent<Transform>(i + 1);
                Assert.NotEqual(new Vector3(0, 0, 0), transform.Position);
            }
        }

        [Fact]
        public void CollisionResolutionSystem_ProcessCollidingCircles_ResolvesCircleCollisions()
        {
            // Arrange
            var twoSqrt = (float)MathHelper.Sqrt(2);
            var system = new CollisionResolutionSystem();
            var world = new WorldBuilder().RegisterSystem(system).Build();
            var circleA = world.CreateEntity()
                .Attach(new Transform()
                {
                    Position = new Vector3(0, 0, 0)
                })
                .Attach(new Collider2D()
                {
                    Shape = Shape2D.Circle,
                    Radius = 2
                });
            var circleB = world.CreateEntity()
                .Attach(new Transform()
                {
                    Position = new Vector3(1, 1, 0)
                })
                .Attach(new Collider2D()
                {
                    Shape = Shape2D.Circle,
                    Radius = 2
                });

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2)));

            // Assert
            Assert.NotEqual(new Vector3(-twoSqrt, -twoSqrt, 0), circleA.GetComponent<Transform>().Position);
            Assert.NotEqual(new Vector3(twoSqrt, twoSqrt, 0), circleB.GetComponent<Transform>().Position);
        }

        [Fact]
        public void CollisionResolutionSystem_ProcessNonCollidingRectangles_DoesntModifyThem()
        {
            // Arrange
            var system = new CollisionResolutionSystem();
            var world = new WorldBuilder().RegisterSystem(system).Build();
            for (int i = 0; i < 10; i++)
            {
                world.CreateEntity()
                    .Attach(new Transform()
                    {
                        Position = new Vector3(5 * i, 0, 0)
                    })
                    .Attach(new Collider2D()
                    {
                        Shape = Shape2D.Rectangle,
                        Width = 2,
                        Height = 2
                    });
            }

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2)));

            // Assert
            for (int i = 0; i < 10; i++)
            {
                var transform = world.GetComponent<Transform>(i + 1);
                Assert.Equal(new Vector3(5 * i, 0, 0), transform.Position);
            }
        }

        [Fact]
        public void CollisionResolutionSystem_ProcessCollidingRectangles_ModifiesTheirPosition()
        {
            // Arrange
            var system = new CollisionResolutionSystem();
            var world = new WorldBuilder().RegisterSystem(system).Build();
            for (int i = 0; i < 10; i++)
            {
                world.CreateEntity()
                    .Attach(new Transform()
                    {
                        Position = new Vector3(0, 0, 0)
                    })
                    .Attach(new Collider2D()
                    {
                        Shape = Shape2D.Rectangle,
                        Width = 2,
                        Height = 2
                    });
            }

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2)));

            // Assert
            for (int i = 0; i < 10; i++)
            {
                var transform = world.GetComponent<Transform>(i + 1);
                Assert.NotEqual(new Vector3(0, 0, 0), transform.Position);
            }
        }

        [Fact]
        public void CollisionResolutionSystem_ProcessCollidingRectangles_ResolvesCircleCollisions()
        {
            // Arrange
            var twoSqrt = (float)MathHelper.Sqrt(2);
            var system = new CollisionResolutionSystem();
            var world = new WorldBuilder().RegisterSystem(system).Build();
            var circleA = world.CreateEntity()
                .Attach(new Transform()
                {
                    Position = new Vector3(0, 0, 0)
                })
                .Attach(new Collider2D()
                {
                    Shape = Shape2D.Rectangle,
                    Width = 2,
                    Height = 2
                });
            var circleB = world.CreateEntity()
                .Attach(new Transform()
                {
                    Position = new Vector3(1, 1, 0)
                })
                .Attach(new Collider2D()
                {
                    Shape = Shape2D.Rectangle,
                    Width = 2,
                    Height = 2
                });

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2)));

            // Assert
            Assert.NotEqual(new Vector3(-twoSqrt, -twoSqrt, 0), circleA.GetComponent<Transform>().Position);
            Assert.NotEqual(new Vector3(twoSqrt, twoSqrt, 0), circleB.GetComponent<Transform>().Position);
        }
    }
}
