using RetroEngine.Core;
using RetroEngine.ECS.Elements;
using RetroEngine.ECS.Managers;

namespace RetroEngine.ECS
{
    /// <summary>
    /// Represents an entity-component-system abstraction.
    /// </summary>
    public class World
    {
        private List<IUpdateSystem> _updateSystems;
        private List<IRenderSystem> _renderSystems;

        private EntityManager _entityManager { get; set; }
        private ComponentManager _componentManager { get; set; }

        internal event Action<long>? OnEntityCreated;
        internal event Action<long>? OnEntityDestroyed;
        internal event Action<long>? OnComponentAdded;
        internal event Action<long>? OnComponentRemoved;

        /// <summary>
        /// Creates a new world.
        /// </summary>
        internal World()
        {
            _updateSystems = new List<IUpdateSystem>();
            _renderSystems = new List<IRenderSystem>();

            _entityManager = new EntityManager(this);
            _componentManager = new ComponentManager(this);
        }

        /// <summary>
        /// Creates an entity on this world.
        /// </summary>
        /// <returns></returns>
        public Entity CreateEntity()
        {
            var entity =_entityManager.Create();

            if (OnEntityCreated != null)
                OnEntityCreated(entity.Id);

            return entity;
        }

        /// <summary>
        /// Gets an entity in this world.
        /// </summary>
        /// <param name="id">Entity's ID.</param>
        /// <returns>An instance of this entity if exists.</returns>
        public Entity? GetEntity(long id) => _entityManager.Get(id);

        /// <summary>
        /// Removes an entity from this world.
        /// </summary>
        /// <param name="id">Entity's ID.</param>
        /// <returns>True if the entity weas removed sucessfully, false otherwise.</returns>
        public bool DestroyEntity(long id)
        {
            if (_entityManager.Destroy(id))
            {
                if (OnEntityDestroyed != null)
                    OnEntityDestroyed(id);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a new system to this world.
        /// </summary>
        /// <param name="system">System to add.</param>
        internal void Register(Elements.System system)
        {
            if (system is IUpdateSystem u)
                _updateSystems.Add(u);

            if (system is IRenderSystem d)
                _renderSystems.Add(d);

            system.Initialize(this);
        }

        /// <summary>
        /// Updates the logic of the world.
        /// </summary>
        /// <param name="gameTime">Elapsed time of the game.</param>
        public void Update(GameTime gameTime)
        {
            foreach (var system in _updateSystems)
            {
                system.Update(gameTime);
            }
        }

        /// <summary>
        /// Renders the graphgics of the world.
        /// </summary>
        /// <param name="gameTime">Elapsed time of the game.</param>
        public void Render(GameTime gameTime)
        {
            foreach (var system in _renderSystems)
            {
                system.Render(gameTime);
            }
        }

        /// <summary>
        /// Gets a query list of all entities IDs in the game.
        /// </summary>
        /// <returns>A queryable list of all entities ready to filter them.</returns>
        public IQueryable<long> GetAllEntityIDs()
        {
            return _entityManager.GetAllIDs();
        }

        /// <summary>
        /// Gets a query list of all entities IDs in the game.
        /// </summary>
        /// <returns>A queryable list of all entities ready to filter them.</returns>
        internal bool EntityHasComponent(long entity, Type type)
        {
            return _componentManager.HasComponent(entity, type);
        }

        /// <summary>
        /// Adds a component to an entity.
        /// </summary>
        /// <typeparam name="T">Type of the component.</typeparam>
        /// <param name="entityId">Entity containing the compomnent.</param>
        /// <param name="component">Component to add.</param>
        public void AddComponent<T>(long entityId, T component) where T : struct, IComponent
        {
            _componentManager.AddComponent(entityId, component);

            if (OnComponentAdded != null)
                OnComponentAdded(entityId);
        }

        /// <summary>
        /// Gets the component of a specified type from an entity.
        /// </summary>
        /// <typeparam name="T">Type of the component.</typeparam>
        /// <param name="entityId">Entity containing the component.</param>
        /// <returns>A reference to the component.</returns>
        /// <remarks>As this method is obtaining a reference, all modification on the returned component will affect the entity behaviour.</remarks>
        public ref T GetComponent<T>(long entityId) where T : struct, IComponent
        {
            return ref _componentManager.GetComponent<T>(entityId);
        }

        /// <summary>
        /// Removes a component from an entity.
        /// </summary>
        /// <typeparam name="T">Type of the component to remove.</typeparam>
        /// <param name="entityId">Entity to remove the component from.</param>
        /// <returns>True if the component was succesfully removed, false otherwise.</returns>
        public bool RemoveComponent<T>(long entityId) where T : struct, IComponent
        {
            if (_componentManager.RemoveComponent<T>(entityId))
            {
                if (OnComponentRemoved != null)
                    OnComponentRemoved(entityId);

                return true;
            }

            return false;
        }
    }
}