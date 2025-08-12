using RetroEngine.Core.Exceptions;
using RetroEngine.Core.Utils;
using System.Collections;

namespace RetroEngine.Core.Elements
{
    /// <summary>
    /// Defines methods a ComponentMapper must have.
    /// </summary>
    public interface IComponentMapper
    {
        /// <summary>
        /// Unassings a component of this mapper type from an entity.
        /// </summary>
        /// <param name="entityId">Entity ID that has the component assigned.</param>
        void Remove(int entityId);
    }

    /// <summary>
    /// Defines a packed array with the components of a specified type for each entity who have one of that type.
    /// </summary>
    /// <typeparam name="T">Type of the component.</typeparam>
    /// <param name="initialSize">Initial size of the component array.</param>
    public class ComponentMapper<T>(int initialSize = 1024) : IComponentMapper, IEnumerable<T>
        where T : struct
    {
        private int _size = 0;
        private T[] _components = new T[initialSize];
        private int[] _entityToIndex = new int[initialSize];
        private int[] _indexToEntity = new int[initialSize];

        /// <summary>
        /// Assigns a component of this mapper type to an entity.
        /// </summary>
        /// <param name="entityId">Entity ID to assign the component.</param>
        /// <param name="component">Component to assign.</param>
        /// <exception cref="EntityException">Thrown if entityId is lower than 1.</exception>
        public void Insert(int entityId, T component)
        {
            if (entityId < 1)
                throw new EntityException("Entity ID must be greater than zero.");

            ++_size;

            ArrayHelper.EnsureCapacity(ref _components, _size);
            ArrayHelper.EnsureCapacity(ref _entityToIndex, entityId);
            ArrayHelper.EnsureCapacity(ref _indexToEntity, _size);

            _components[_size] = component;
            _entityToIndex[entityId] = _size;
            _indexToEntity[_size] = entityId;
        }

        /// <summary>
        /// Gets the component of this mapper type assigned to an entity.
        /// </summary>
        /// <param name="entityId">Entity ID that has the component assigned.</param>
        /// <returns>An instance of a component of this mapper type.</returns>
        /// <exception cref="EntityException">Thrown if entityId is lower than 1.</exception>
        /// <exception cref="ComponentException">Thrown if the entity doesn't have a component of this mapper type.</exception>
        public ref T Get(int entityId)
        {
            if (entityId < 1)
                throw new EntityException("Entity ID must be greater than zero.");

            if (_entityToIndex[entityId] == 0)
                throw new ComponentException($"Entity {entityId} doesn't have a component of type {typeof(T)}.");

            return ref _components[_entityToIndex[entityId]];
        }

        /// <summary>
        /// Gets the quantity of components stored in this mapper.
        /// </summary>
        /// <returns></returns>
        public int Size() => _size;

        /// <summary>
        /// Unassings a component of this mapper type from an entity.
        /// </summary>
        /// <param name="entityId">Entity ID that has the component assigned.</param>
        /// <exception cref="EntityException">Thrown if entityId is lower than 1.</exception>
        /// <exception cref="ComponentException">Thrown if the entity doesn't have a component of this mapper type.</exception>
        public void Remove(int entityId)
        {
            if (entityId < 1)
                throw new EntityException("Entity ID must be greater than zero.");

            if (_entityToIndex[entityId] == 0)
                throw new ComponentException($"Entity {entityId} doesn't have a component of type {typeof(T)}.");

            int positionToDelete = _entityToIndex[entityId];
            int lastPostion = _size;

            _components[positionToDelete] = _components[lastPostion];
            _components[lastPostion] = default;
            _entityToIndex[_indexToEntity[lastPostion]] = positionToDelete;
            _entityToIndex[entityId] = default;
            _indexToEntity[positionToDelete] = _indexToEntity[lastPostion];
            _indexToEntity[lastPostion] = default;

            --_size;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _components.AsEnumerable().Skip(1).Take(_size).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Returns the entity ID of the component located at a given index of this mapper GetEnumerator().
        /// </summary>
        /// <param name="index">Index of the component.</param>
        /// <returns>Entity ID corresponding to the specified component index.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index is lower than zero, or greater or equal that Size().</exception>
        /// <remarks>If GetEnumerator()[n] contains component C of the entity E, GetEntityOfIndex(n) returns E id.</remarks>
        public int GetEntityOnIndex(int index)
        {
            if (index < 0 || index >= _size)
                throw new IndexOutOfRangeException($"There's no component in the index {index} of this mapper.");

            return _indexToEntity[index + 1];
        }

        /// <summary>
        /// Gets the index in GetEnumerator() where the component of a specified entity is located.
        /// </summary>
        /// <param name="entityId">Entity ID of the component.</param>
        /// <returns>Index of the component for the specified entity.</returns>
        /// <exception cref="EntityException">Thrown if entityId is lower than 1.</exception>
        /// <exception cref="ComponentException">Thrown if the entity doesn't have a component of this mapper type.</exception>
        /// <remarks>If entity E has component C, then GetEnumerator()[GetIndexOfEntity(E)] returns component C.</remarks>
        public int GetIndexOfEntity(int entityId)
        {
            if (entityId < 1)
                throw new EntityException("Entity ID must be greater than zero.");

            if (_entityToIndex[entityId] == 0)
                throw new ComponentException($"Entity {entityId} doesn't have a component of type {typeof(T)}.");

            return _entityToIndex[entityId] - 1;
        }
    }
}
