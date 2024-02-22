using RetroEngine.Core.Elements;

namespace RetroEngine.Core.Managers
{
    /// <summary>
    /// Defines functionallity to manage all component mappers in a ECS world.
    /// </summary>
    internal class ComponentManager
    {
        private World _world;
        private Dictionary<Type, IComponentMapper> _mappers;

        /// <summary>
        /// Creates a new component manager for a ECS world.
        /// </summary>
        /// <param name="world">ECS world containing this manager.</param>
        public ComponentManager(World world)
        {
            _world = world;
            _mappers = new Dictionary<Type, IComponentMapper>();
        }

        /// <summary>
        /// Gets the component mapper of a specified component type.
        /// </summary>
        /// <typeparam name="T">Type of the component.</typeparam>
        /// <returns>An instance of the mapper of the component type.</returns>
        internal ComponentMapper<T> GetMapper<T>() where T : struct, IComponent
        {
            ComponentMapper<T> mapper;
            Type type = typeof(T);

            if (_mappers.ContainsKey(type))
            {
                mapper = (ComponentMapper<T>)_mappers[type];
            }
            else
            {
                mapper = new ComponentMapper<T>();
                _mappers.Add(type, mapper);
            }

            return mapper;
        }

        /// <summary>
        /// Adds a component to an entity.
        /// </summary>
        /// <typeparam name="T">Type of the component.</typeparam>
        /// <param name="entity">Entity containing the compomnent.</param>
        /// <param name="component">Component to add.</param>
        public void AddComponent<T>(long entity, T component) where T : struct, IComponent
        {
            GetMapper<T>().Add(entity, component);
        }

        /// <summary>
        /// Gets the component of a specified type from an entity.
        /// </summary>
        /// <typeparam name="T">Type of the component.</typeparam>
        /// <param name="entity">Entity containing the component.</param>
        /// <returns>A reference to the component.</returns>
        /// <remarks>As this method is obtaining a reference, all modification on the returned component will affect the entity behaviour.</remarks>
        public ref T GetComponent<T>(long entity) where T : struct, IComponent
        {
            return ref GetMapper<T>().Get(entity);
        }

        /// <summary>
        /// Checks if a component
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="type"></param>
        /// <returns>True if the entity have a component of the specified type, and false otherwise.</returns>
        internal bool HasComponent(long entity, Type type)
        {
            return _mappers.ContainsKey(type) && _mappers[type].Has(entity);
        }

        /// <summary>
        /// Removes a component from an entity.
        /// </summary>
        /// <typeparam name="T">Type of the component to remove.</typeparam>
        /// <param name="entity">Entity to remove the component from.</param>
        /// <returns>True if the component was succesfully removed, false otherwise.</returns>
        public bool RemoveComponent<T>(long entity) where T : struct, IComponent
        {
            return GetMapper<T>().Remove(entity);
        }
    }
}
