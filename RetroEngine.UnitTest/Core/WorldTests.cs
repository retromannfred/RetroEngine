using NUnit.Framework;
using RetroEngine.Core;
using RetroEngine.Core.Elements;
using System.Reflection;
using System.Threading;

namespace RetroEngine.UnitTest.Core
{
    public class WorldTests
    {
        [Test]
        public void CannotInstanceWorld()
        {
            ConstructorInfo[] ctors;

            Type type = typeof(World);
            ctors = type.GetConstructors();

            Assert.That(ctors.Length, Is.EqualTo(0), "World constructor is accessible");
        }

        [Test]
        public void BuildEmptyWorld()
        {
            World world;

            world = new WorldBuilder().Build();

            Assert.IsNotNull(world, "World is not builded");
        }

        [Test]
        public void CreateEntities()
        {
            World world;

            world = new WorldBuilder().Build();
            for (int i = 0; i < 1000; i++)
            {
                world.CreateEntity();
            }

            Assert.That(world.GetAllEntityIDs().Count(), Is.EqualTo(1000), "There are different entities than create entity calls");
        }

        [Test]
        public void GetExistingEntities()
        {
            World world;
            Entity? one;
            Entity? two;
            Entity? three;

            world = new WorldBuilder().Build();
            for (int i = 0; i < 1000; i++)
            {
                world.CreateEntity();
            }
            one = world.GetEntity(101);
            two = world.GetEntity(24);
            three = world.GetEntity(1000);

            Assert.IsNotNull(one);
            Assert.IsNotNull(two);
            Assert.IsNotNull(three);

            Assert.That(one.Id, Is.EqualTo(101), "Entity 101 is not created");
            Assert.That(two.Id, Is.EqualTo(24), "Entity 24 is not created");
            Assert.That(three.Id, Is.EqualTo(1000), "Entity 1000 is not created");
        }

        [Test]
        public void GetNonExistingEntities()
        {
            World world;
            Entity? one;
            Entity? two;

            world = new WorldBuilder().Build();
            for (int i = 0; i < 1000; i++)
            {
                world.CreateEntity();
            }
            one = world.GetEntity(-10);
            two = world.GetEntity(1001);

            Assert.IsNull(one, "Entity -10 shouldn't exist");
            Assert.IsNull(two, "Entity 1001 shouldn't exist");
        }

        [Test]
        public void DestroyExistingEntities()
        {
            World world;
            bool one;
            bool two;
            bool three;

            world = new WorldBuilder().Build();
            for (int i = 0; i < 1000; i++)
            {
                world.CreateEntity();
            }

            one = world.DestroyEntity(101);
            two = world.DestroyEntity(24);
            three = world.DestroyEntity(1000);

            Assert.IsTrue(one, "Entity 101 is not destroyed");
            Assert.IsTrue(two, "Entity 24 is not destroyed");
            Assert.IsTrue(three, "Entity 1001 is not destroyed");
            Assert.That(world.GetAllEntityIDs().Count(), Is.EqualTo(997), "World entities are not removed when deleted");
        }

        [Test]
        public void DestroyNonExistingEntities()
        {
            World world;
            bool one;
            bool two;

            world = new WorldBuilder().Build();
            for (int i = 0; i < 1000; i++)
            {
                world.CreateEntity();
            }

            one = world.DestroyEntity(-10);
            two = world.DestroyEntity(1001);

            Assert.IsFalse(one, "Entity -10 has been removed without existing");
            Assert.IsFalse(two, "Entity 1001 has been removed without existing");
            Assert.That(world.GetAllEntityIDs().Count(), Is.EqualTo(1000), "There are removed emptities that shouldn't be removed");
        }
    }
}