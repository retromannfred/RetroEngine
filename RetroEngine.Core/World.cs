using RetroEngine.Core.Elements;
using RetroEngine.Core.Managers;

namespace RetroEngine.Core
{
    /// <summary>
    /// Defines an orchestrator that manages info between ECS managers.
    /// </summary>
    public class World
    {
        private readonly EntityManager _entityManager;
        private readonly ComponentManager _componentManager;
        private readonly SystemManager _systemManager;

        private readonly HashSet<int> _destroyedEntities = [];
        private readonly HashSet<int> _modifiedEntities = [];

        internal World(
            EntityManager entityManager,
            ComponentManager componentManager,
            SystemManager systemManager)
        {
            _entityManager = entityManager;
            _componentManager = componentManager;
            _systemManager = systemManager;
        }

        /// <summary>
        /// Creates a new entity in this world.
        /// </summary>
        /// <returns>An instance of Entity class.</returns>
        public Entity CreateEntity()
        {
            return new(_entityManager.Create(), this);
        }

        /// <summary>
        /// Gets a component of an entity.
        /// </summary>
        /// <typeparam name="T">Type of the component to get.</typeparam>
        /// <param name="entityId">Entity ID to get the component from.</param>
        /// <returns>A reference of the component from the respective mapper.</returns>
        public ref T GetComponent<T>(int entityId)
            where T : struct
        {
            return ref _componentManager.GetComponent<T>(entityId);
        }

        /// <summary>
        /// Gets a component of an Entity.
        /// </summary>
        /// <typeparam name="T">Type of the component to get.</typeparam>
        /// <param name="entity">Entity to get the component from.</param>
        /// <returns>A reference of the component from the respective mapper.</returns>
        public ref T GetComponent<T>(Entity entity)
            where T : struct
        {
            return ref GetComponent<T>(entity.Id);
        }

        /// <summary>
        /// Attaches a component to an entity.
        /// </summary>
        /// <typeparam name="T">Type of the component to attach.</typeparam>
        /// <param name="entityId">Entity ID where to attach the component.</param>
        /// <param name="component">Component to attach.</param>
        public void AttachComponent<T>(int entityId, T component)
            where T : struct
        {
            _componentManager.AddComponent(entityId, component);

            var signature = _entityManager.GetSignature(entityId);
            signature[_componentManager.GetSignatureIndex<T>()] = true;
            _entityManager.SetSignature(entityId, signature);

            _modifiedEntities.Add(entityId);
        }

        /// <summary>
        /// Attaches a component to an entity.
        /// </summary>
        /// <typeparam name="T">Type of the component to attach.</typeparam>
        /// <param name="entity">Entity where to attach the component.</param>
        /// <param name="component">Component to attach.</param>
        public void AttachComponent<T>(Entity entity, T component)
            where T : struct
        {
            AttachComponent(entity.Id, component);
        }

        /// <summary>
        /// Deattaches a component from an entity.
        /// </summary>
        /// <typeparam name="T">Type of the component to deattach.</typeparam>
        /// <param name="entityId">Entity ID where to dettach the component.</param>
        public void DeattachComponent<T>(int entityId)
            where T : struct
        {
            _componentManager.RemoveComponent<T>(entityId);

            var signature = _entityManager.GetSignature(entityId);
            signature[_componentManager.GetSignatureIndex<T>()] = false;
            _entityManager.SetSignature(entityId, signature);

            _modifiedEntities.Add(entityId);
        }

        /// <summary>
        /// Deattaches a component from an entity.
        /// </summary>
        /// <typeparam name="T">Type of the component to deattach.</typeparam>
        /// <param name="entity">Entity where to dettach the component.</param>
        public void DeattachComponent<T>(Entity entity)
            where T : struct
        {
            DeattachComponent<T>(entity.Id);
        }

        /// <summary>
        /// Destroys an entity from this world.
        /// </summary>
        /// <param name="entityId">Entity ID to destroy.</param>
        public void DestroyEntity(int entityId)
        {
            _entityManager.Destroy(entityId);
            _destroyedEntities.Add(entityId);
        }

        /// <summary>
        /// Destroys an entity from this world.
        /// </summary>
        /// <param name="entityId">Entity to destroy.</param>
        public void DestroyEntity(Entity entity)
        {
            DestroyEntity(entity.Id);
        }

        /// <summary>
        /// Performs the processing of update systems.
        /// </summary>
        /// <param name="time">Info about the gametime.</param>
        public void Update(GameTime time)
        {
            foreach (var entityId in _modifiedEntities)
            {
                _systemManager.NotifyChangedEntitySignature(entityId, _entityManager.GetSignature(entityId));
            }

            foreach (var entityId in _destroyedEntities)
            {
                _systemManager.NotifyDestroyedEntity(entityId);
            }

            _modifiedEntities.Clear();
            _destroyedEntities.Clear();

            _systemManager.PerformUpdate(this, time);
        }

        /// <summary>
        /// Performs the processing of render systems.
        /// </summary>
        /// <param name="time">Info about the gametime.</param>
        public void Render(GameTime time)
        {
            _systemManager.PerformRender(this, time);
        }
    }
}
