using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Core.Components;
using RetroEngine.Physics.Components;
using RetroEngine.Physics.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.UnitTest.Physics.Systems
{
    public class PhysicsSystemTests
    {
        [Fact]
        public void PhysicsSystem_BodyWithVelocity_MovesConstant()
        {
            // Arrange
            var timeToTest = 2f;
            var velocity = new Vector2(3, 4);
            var world = new WorldBuilder()
                .RegisterSystem(new PhysicsSystem())
                .Build();
            var body = world.CreateEntity()
                .Attach(new Transform())
                .Attach(new RigidBody2D()
                {
                    LinearVelocity = velocity,
                });

            // Act
            world.Update(new GameTime(TimeSpan.FromSeconds(timeToTest), TimeSpan.FromSeconds(timeToTest)));

            // Assert
            Assert.Equal(new Vector3(6, 8, 0), body.GetComponent<Transform>().Position);
        }
    }
}
