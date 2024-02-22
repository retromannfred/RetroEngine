namespace RetroEngine.Core.Elements
{
    /// <summary>
    /// Defines an object inside the game.
    /// </summary>
    public class Entity
    {
        private World _world;

        /// <summary>
        /// Gets the ID of this entity.
        /// </summary>
        public long Id { get; private set; }

        internal Entity(long id, World world)
        {
            Id = id;
            _world = world;
        }

        /// <summary>
        /// Adds a component to this entity.
        /// </summary>
        /// <typeparam name="T">Type of the component to add.</typeparam>
        /// <param name="component">Component to add.</param>
        /// <returns>This entity instance.</returns>
        public Entity Attach<T>(T component) where T : struct, IComponent
        {
            _world.AddComponent(Id, component);
            return this;
        }

        /// <summary>
        /// Gets the component of a specified type from this entity.
        /// </summary>
        /// <typeparam name="T">Type of the component to get.</typeparam>
        /// <returns>And reference instance of the component to get.</returns>
        /// <remarks>As this method gets the component struct as reference, any modification on it will be reflected on the entity behaviour.</remarks>
        public ref T Get<T>() where T : struct, IComponent
        {
            return ref _world.GetComponent<T>(Id);
        }

        /// <summary>
        /// Removes a component from this entity.
        /// </summary>
        /// <typeparam name="T">Type of the component to remove.</typeparam>
        /// <returns>True if the component was removed successfully, false otherwise.</returns>
        public bool Remove<T>() where T : struct, IComponent
        {
            return _world.RemoveComponent<T>(Id);
        }
    }
}
