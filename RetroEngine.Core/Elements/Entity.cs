using RetroEngine.Core.Exceptions;
using RetroEngine.Core.Managers;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RetroEngine.UnitTest")]
namespace RetroEngine.Core.Elements
{
    /// <summary>
    /// Defines a wrapper for both entity and component manager to create entities and attach components to it.
    /// </summary>
    public class Entity
    {
        private readonly int _id;
        private readonly World _world;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="id">New entity ID created.</param>
        /// <param name="world">World where the entity is created.</param>
        internal Entity(int id, World world)
        {
            if (id < 1)
                throw new EntityException("Entity ID must be greater than zero.");

            _id = id;
            _world = world;
        }

        /// <summary>
        /// Gets this entity's ID.
        /// </summary>
        public int Id => _id;

        /// <summary>
        /// Attaches a component to this entity.
        /// </summary>
        /// <typeparam name="T">Type of component to attach.</typeparam>
        /// <param name="component">Component to attach.</param>
        /// <returns>This entity instance.</returns>
        public Entity Attach<T>(T component)
            where T : struct
        {
            _world.AttachComponent(_id, component);
            return this;
        }

        /// <summary>
        /// Deattaches the component of specified type from this entity.
        /// </summary>
        /// <typeparam name="T">Type of component to deattach.</typeparam>
        /// <returns>This entity instance.</returns>
        public Entity Deattach<T>()
            where T : struct
        {
            _world.DeattachComponent<T>(_id);
            return this;
        }
    }
}
