using RetroEngine.Core.Elements;

namespace RetroEngine.Core.Mapping
{
    /// <summary>
    /// Defines basic functionallity non-type dependant of a component mapper.
    /// </summary>
    internal interface IComponentMapper
    {
        bool Has(long entity);
    }

    /// <summary>
    /// Maps components of a specified type with game entities.
    /// </summary>
    internal class ComponentMapper<T> : IComponentMapper where T : struct, IComponent
    {
        private const int INITIAL_SIZE = 128;

        private T[] _components;
        private List<long> _entities;

        /// <summary>
        /// Creates a new component mapper.
        /// </summary>
        public ComponentMapper()
        {
            _components = new T[INITIAL_SIZE];
            _entities = new List<long>();
        }

        /// <summary>
        /// Checks if an entity have a component on this mapper.
        /// </summary>
        /// <param name="entity">Entity to check.</param>
        /// <returns>True if the entity have the component, false otherwise.</returns>
        public bool Has(long entity)
        {
            return _entities.Contains(entity);
        }

        private void Duplicate()
        {
            var copy = new T[_components.Length * 2];
            _components.CopyTo(copy, 0);
            _components = copy;
        }

        /// <summary>
        /// Adds a component to this mapper.
        /// </summary>
        /// <param name="entity">Entity to contain the component.</param>
        /// <param name="component">Component to add.</param>
        public void Add(long entity, T component)
        {
            if (entity >= _components.Length)
                Duplicate();

            _components[entity] = component;
            _entities.Add(entity);
        }

        /// <summary>
        /// Gets the component of an entity.
        /// </summary>
        /// <param name="entity">Entity containing the component.</param>
        /// <returns>A reference to the component.</returns>
        /// <remarks>As this method is obtaining a reference, all modification on the returned component will affect the entity behaviour.</remarks>
        public ref T Get(long entity)
        {
            return ref _components[entity];
        }

        /// <summary>
        /// Removes a component from this mapper.
        /// </summary>
        /// <param name="entity">Entity containing the component</param>
        /// <returns></returns>
        public bool Remove(long entity)
        {
            if (_entities.Remove(entity))
            {
                _components[entity] = default;
                return true;
            }

            return false;
        }
    }
}
