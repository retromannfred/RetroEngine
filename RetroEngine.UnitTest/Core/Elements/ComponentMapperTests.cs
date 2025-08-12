using RetroEngine.Core.Elements;
using RetroEngine.Core.Exceptions;
using RetroEngine.UnitTest.TestData.Components;

namespace RetroEngine.UnitTest.Core.Elements
{
    /// <summary>
    /// Defines unit test cases for ComponentMapper class.
    /// </summary>
    public class ComponentMapperTests
    {
        [Fact]
        public void ComponentMapper_InsertingNegativeEntity_ThrowsEntityException()
        {
            // Arrange
            var mapper = new ComponentMapper<TagComponent>();

            // Act
            void action() => mapper.Insert(-1, default);

            // Assert
            Assert.Throws<EntityException>(action);
        }

        [Fact]
        public void ComponentMapper_InsertingEntityZero_ThrowsEntityException()
        {
            // Arrange
            var mapper = new ComponentMapper<TagComponent>();

            // Act
            void action() => mapper.Insert(0, default);

            // Assert
            Assert.Throws<EntityException>(action);
        }

        [Fact]
        public void ComponentMapper_AfterInsertingEntity_CanGetAttachedComponent()
        {
            // Arrange
            var testTag = new Guid().ToString();
            var mapper = new ComponentMapper<TagComponent>();

            // Act
            mapper.Insert(1, new TagComponent(testTag));

            // Assert
            Assert.Equal(testTag, mapper.Get(1).Tag);
        }

        [Fact]
        public void ComponentMapper_InsertingEntityHighIdOutOfBounds_RearrangesSizes()
        {
            // Arrange
            var testTag = new Guid().ToString();
            var mapper = new ComponentMapper<TagComponent>(100);

            // Act
            mapper.Insert(200, new TagComponent(testTag));

            // Assert
            Assert.Equal(testTag, mapper.Get(200).Tag);
        }

        [Fact]
        public void ComponentMapper_GettingFromEntityZero_ThrowsEntityException()
        {
            // Arrange
            var mapper = new ComponentMapper<TagComponent>();

            // Act
            void action() { mapper.Get(0); }

            // Assert
            Assert.Throws<EntityException>(action);
        }

        [Fact]
        public void ComponentMapper_GettingFromNegativeEntity_ThrowsEntityException()
        {
            // Arrange
            var mapper = new ComponentMapper<TagComponent>();

            // Act
            void action() { mapper.Get(-1); }

            // Assert
            Assert.Throws<EntityException>(action);
        }

        [Fact]
        public void ComponentMapper_GettingFromNonExistingEntity_ThrowsComponentException()
        {
            // Arrange
            var testTag = Guid.NewGuid().ToString();
            var mapper = new ComponentMapper<TagComponent>();

            // Act
            void action() => mapper.Get(100);

            // Assert
            Assert.Throws<ComponentException>(action);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        [InlineData(2000)]
        public void ComponentMapper_AfterInsertingComponents_SizeReturnsDesiredValue(int quantity)
        {
            // Arrange
            var mapper = new ComponentMapper<TagComponent>();

            // Act
            for (int i = 1; i <= quantity; i++)
            {
                mapper.Insert(i, new TagComponent($"Tag #{i}"));
            }

            // Assert
            Assert.Equal(quantity, mapper.Size());
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(4, 2)]
        [InlineData(2000, 197)]
        [InlineData(10, 100)]
        public void ComponentMapper_AfterInsertingThenRemovingComponents_SizeReturnsDesiredValue(int insertsCount, int deletesCount)
        {
            // Arrange
            var mapper = new ComponentMapper<TagComponent>();

            // Act
            for (int i = 1; i <= insertsCount; i++)
            {
                mapper.Insert(i, new TagComponent($"Tag #{i}"));
            }
            for (int i = 1; i <= deletesCount && i <= insertsCount; i++)
            {
                mapper.Remove(i);
            }

            // Assert
            Assert.Equal(Math.Max(insertsCount - deletesCount, 0), mapper.Size());
        }

        [Fact]
        public void ComponentMapper_GettingEntityFromNegativeIndex_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            var mapper = new ComponentMapper<TagComponent>();
            mapper.Insert(1, default);
            mapper.Insert(2, default);
            mapper.Insert(3, default);
            mapper.Insert(4, default);

            // Act
            void action() => mapper.GetEntityOnIndex(-1);

            // Arrange
            Assert.Throws<IndexOutOfRangeException>(action);
        }

        [Fact]
        public void ComponentMapper_GettingEntityFromIndexMoreThanSize_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            var mapper = new ComponentMapper<TagComponent>();
            mapper.Insert(1, default);
            mapper.Insert(2, default);
            mapper.Insert(3, default);
            mapper.Insert(4, default);

            // Act
            void actionEqual() => mapper.GetEntityOnIndex(4);
            void actionGreater() => mapper.GetEntityOnIndex(10);

            // Arrange
            Assert.Throws<IndexOutOfRangeException>(actionEqual);
            Assert.Throws<IndexOutOfRangeException>(actionGreater);
        }

        [Fact]
        public void ComponentMapper_GettingIndexOfNegativeOrZeroEntity_ThrowsEntityException()
        {
            // Arrange
            var mapper = new ComponentMapper<TagComponent>();
            mapper.Insert(1, default);
            mapper.Insert(2, default);
            mapper.Insert(3, default);
            mapper.Insert(4, default);

            // Act
            void actionZero() => mapper.GetIndexOfEntity(0);
            void actionNegative() => mapper.GetIndexOfEntity(-1);

            // Arrange
            Assert.Throws<EntityException>(actionZero);
            Assert.Throws<EntityException>(actionNegative);
        }

        [Fact]
        public void ComponentMapper_GettingIndexOfNonExistingEntity_ThrowsComponentException()
        {
            // Arrange
            var mapper = new ComponentMapper<TagComponent>();
            mapper.Insert(1, default);
            mapper.Insert(2, default);
            mapper.Insert(4, default);

            // Act
            void actionJump() => mapper.GetIndexOfEntity(3);
            void actionGreater() => mapper.GetIndexOfEntity(10);

            // Arrange
            Assert.Throws<ComponentException>(actionJump);
            Assert.Throws<ComponentException>(actionGreater);
        }

        [Fact]
        public void ComponentMapper_InsertingComponents_KeepsArraysWellFormed()
        {
            // Arrange
            var mapper = new ComponentMapper<TagComponent>();

            // Act
            mapper.Insert(1, new TagComponent("A"));
            mapper.Insert(2, new TagComponent("B"));
            mapper.Insert(3, new TagComponent("C"));
            mapper.Insert(4, new TagComponent("D"));

            var components = mapper.ToList();

            // Arrange
            Assert.Equal("A", components[0].Tag);
            Assert.Equal("B", components[1].Tag);
            Assert.Equal("C", components[2].Tag);
            Assert.Equal("D", components[3].Tag);

            Assert.Equal(1, mapper.GetEntityOnIndex(0));
            Assert.Equal(2, mapper.GetEntityOnIndex(1));
            Assert.Equal(3, mapper.GetEntityOnIndex(2));
            Assert.Equal(4, mapper.GetEntityOnIndex(3));

            Assert.Equal(0, mapper.GetIndexOfEntity(1));
            Assert.Equal(1, mapper.GetIndexOfEntity(2));
            Assert.Equal(2, mapper.GetIndexOfEntity(3));
            Assert.Equal(3, mapper.GetIndexOfEntity(4));
        }

        [Fact]
        public void ComponentMapper_InsertingComponentsThenRemoving_ReorganizesArraysProperly()
        {
            // Arrange
            var mapper = new ComponentMapper<TagComponent>();

            // Act
            mapper.Insert(1, new TagComponent("A"));
            mapper.Insert(2, new TagComponent("B"));
            mapper.Insert(3, new TagComponent("C"));
            mapper.Insert(4, new TagComponent("D"));
            mapper.Remove(2);

            var components = mapper.ToList();

            // Arrange
            Assert.Equal("A", components[0].Tag);
            Assert.Equal("D", components[1].Tag);
            Assert.Equal("C", components[2].Tag);

            Assert.Equal(1, mapper.GetEntityOnIndex(0));
            Assert.Equal(4, mapper.GetEntityOnIndex(1));
            Assert.Equal(3, mapper.GetEntityOnIndex(2));

            Assert.Equal(0, mapper.GetIndexOfEntity(1));
            Assert.Equal(1, mapper.GetIndexOfEntity(4));
            Assert.Equal(2, mapper.GetIndexOfEntity(3));
        }

        [Fact]
        public void ComponentMapper_AfterEditingComponentGotFromMapperAsRef_GetComponentGetsItModified()
        {
            // Arrange
            var mapper = new ComponentMapper<TagComponent>();
            mapper.Insert(1, new TagComponent("A"));
            ref var firstGet = ref mapper.Get(1);

            // Act
            firstGet.Tag = "modified";

            // Assert
            Assert.Equal("modified", mapper.Get(1).Tag);
        }
    }
}
