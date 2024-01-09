using RetroEngine.ECS.Elements;

namespace RetroEngine.ECS.Managers
{
    /// <summary>
    /// Defines functionallity to manage all entities in a ECS world.
    /// </summary>
    internal class EntityManager
    {
        private World _world;
        private long _maxId;
        private List<long> _existingIds;
        private Stack<long> _freeIds;

        /// <summary>
        /// Creates a new entity manager.
        /// </summary>
        /// <param name="world">World containing this entity manager.</param>
        public EntityManager(World world)
        {
            _world = world;
            _maxId = 0;
            _existingIds = new List<long>();
            _freeIds = new Stack<long>();
        }

        /// <summary>
        /// Creates a new entity in the game.
        /// </summary>
        /// <returns>An instance representing functionallity for the new entity.</returns>
        public Entity Create()
        {
            var id =
                _freeIds.Any()
                ? _freeIds.Pop()
                : ++ _maxId;

            _existingIds.Add(id);

            return new Entity(id, _world);
        }

        /// <summary>
        /// Gets an entity by its ID.
        /// </summary>
        /// <param name="id">ID of the entity.</param>
        /// <returns>An instance representing functionallity for this entity.</returns>
        public Entity? Get(long id)
        {
            return _existingIds.Contains(id) ? new Entity(id, _world) : null;
        }

        /// <summary>
        /// Removes an entity from the game.
        /// </summary>
        /// <param name="entity">Entity to remove.</param>
        /// <returns>True if the entity was removed successfuly, false otherwise.</returns>
        public bool Destroy(Entity entity)
        {
            return Destroy(entity.Id);
        }

        /// <summary>
        /// Removes an entity from the game.
        /// </summary>
        /// <param name="id">Entity ID to remove.</param>
        /// <returns>True if the entity was removed successfuly, false otherwise.</returns>
        public bool Destroy(long id)
        {
            if (_existingIds.Remove(id))
            {
                _freeIds.Push(id);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a query list of all entities IDs in the game.
        /// </summary>
        /// <returns>A queryable list of all entities ready to filter them.</returns>
        public IQueryable<long> GetAllIDs()
        {
            return _existingIds.AsQueryable();
        }
    }
}
