using NUnit.Framework;
using RetroEngine.Core;
using RetroEngine.ECS;
using RetroEngine.ECS.Elements;
using RetroEngine.UnitTest.ECS.Utils;

namespace RetroEngine.UnitTest.ECS
{
    public class SystemTests
    {
        /// <summary>
        /// Timespan with 2 hours, 35 minutes and 49 seconds elapsed time.
        /// </summary>
        private static GameTime h2m35s49 = new(TimeSpan.FromSeconds(9349), TimeSpan.FromSeconds(9349));

        [Test]
        public void TestWorldWithSystems()
        {
            World world;

            world = new WorldBuilder()
                .AddSystem(new TestUpdateSystem())
                .AddSystem(new TestRenderSystem())
                .Build();

            Assert.That(world, Is.Not.Null, "Error creating world with systems");
        }

        [Test]
        public void TestUpdateSystem()
        {
            World world = new WorldBuilder()
                .AddSystem(new TestUpdateSystem())
                .Build();
            var entity = world.CreateEntity().Attach(new TestUpdateComponent());

            world.Update(h2m35s49);
            var component = entity.Get<TestUpdateComponent>();

            Assert.That(component.Tag, Is.EqualTo("Updated in 2:35:49"), "Component from system not updated in world update");
        }

        [Test]
        public void TestRenderSystem()
        {
            World world = new WorldBuilder()
                .AddSystem(new TestRenderSystem())
                .Build();
            var entity = world.CreateEntity().Attach(new TestRenderComponent());

            world.Render(h2m35s49);

            var component = entity.Get<TestRenderComponent>();
            Assert.That(component.Tag, Is.EqualTo("Rendered in 2:35:49"), "Component from system not rendered in world render");
        }

        [Test]
        public void TestNewEntityUpdatesAfterUpdate()
        {
            Entity newEntity;
            World world = new WorldBuilder()
                .AddSystem(new TestUpdateSystem())
                .Build();
            world.CreateEntity().Attach(new TestUpdateComponent());

            world.Update(h2m35s49);
            newEntity = world.CreateEntity().Attach(new TestUpdateComponent());
            world.Update(h2m35s49);

            var component = newEntity.Get<TestUpdateComponent>();
            Assert.That(component.Tag, Is.EqualTo("Updated in 2:35:49"), "Component from system not updated in world update");
        }

        [Test]
        public void TestRemovedComponentDoesNotUpdate()
        {
            Entity firstEntity;
            Entity secondEntity;
            World world = new WorldBuilder()
                .AddSystem(new TestUpdateSystem())
                .Build();
            firstEntity = world.CreateEntity().Attach(new TestUpdateComponent());
            secondEntity = world.CreateEntity().Attach(new TestUpdateComponent());

            world.Update(h2m35s49);
            firstEntity.Remove<TestUpdateComponent>();
            world.Update(new GameTime());

            var firstComponent = firstEntity.Get<TestUpdateComponent>();
            var secondComponent = secondEntity.Get<TestUpdateComponent>();
            Assert.Multiple(() =>
            {
                Assert.That(firstComponent.Tag, Is.Null, "Component was not properly removed");
                Assert.That(secondComponent.Tag, Is.EqualTo("Updated in 0:0:0"), "Component from system not updated in world update");
            });
        }
    }
}
