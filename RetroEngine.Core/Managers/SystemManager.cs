using RetroEngine.Core.Elements;
using RetroEngine.Core.Exceptions;
using RetroEngine.Core.Signing;

namespace RetroEngine.Core.Managers
{
    /// <summary>
    /// Manages a collection of systems.
    /// </summary>
    public class SystemManager()
    {
        private readonly HashSet<UpdateSystem> _updateSystems = [];
        private readonly HashSet<RenderSystem> _renderSystems = [];
        private readonly Dictionary<Type, Signature> _signatures = [];

        /// <summary>
        /// Adds a system to be processed by this manager.
        /// </summary>
        /// <typeparam name="T">Type of system to add.</typeparam>
        /// <param name="system">System to add.</param>
        /// <exception cref="RegisterException">Thrown if the system type is not extended from UpdateSystem or RenderSystem.</exception>
        public void AddSystem<T>(T system)
            where T : BaseSystem
        {
            if (_signatures.ContainsKey(typeof(T)))
                return;

            if (system is UpdateSystem)
            {
                var updateSystem = system as UpdateSystem;
                _updateSystems.Add(updateSystem!);
            }
            else if (system is RenderSystem)
            {
                var renderSystem = system as RenderSystem;
                _renderSystems.Add(renderSystem!);
            }
            else
            {
                throw new RegisterException("Cannot register a system that is not an update or render system.");
            }

            _signatures.Add(typeof(T), default);
        }

        /// <summary>
        /// Adds a system and its signature to be processed by this manager.
        /// </summary>
        /// <typeparam name="T">Type of system to add.</typeparam>
        /// <param name="system">System to add.</param>
        /// <param name="signature">Signature of this system.</param>
        /// <exception cref="RegisterException">Thrown if the system type is not extended from UpdateSystem or RenderSystem.</exception>
        public void AddSystem<T>(T system, Signature signature)
            where T : BaseSystem
        {
            this.AddSystem<T>(system);
            this.SetSignature<T>(signature);
        }

        /// <summary>
        /// Gets the signature of a system.
        /// </summary>
        /// <typeparam name="T">Type of the system.</typeparam>
        public Signature GetSignature<T>()
        {
            return _signatures[typeof(T)];
        }

        /// <summary>
        /// Sets the signature of a system.
        /// </summary>
        /// <typeparam name="T">Type of the system.</typeparam>
        /// <param name="signature">Signature to set.</param>
        public void SetSignature<T>(Signature signature)
        {
            _signatures[typeof(T)] = signature;
        }

        /// <summary>
        /// Enumerates both update and render systems as BaseSystems.
        /// </summary>
        /// <returns>An instance of IEnumerable with all the systems.</returns>
        public IEnumerable<BaseSystem> GetAllSystems()
        {
            foreach (var system in _updateSystems)
                yield return system;

            foreach (var system in _renderSystems)
                yield return system;
        }

        /// <summary>
        /// Notifies an entity has been destroyed, to avoid being processed in all the systems.
        /// </summary>
        /// <param name="entityId">Entity that has been removed.</param>
        /// <exception cref="EntityException">Thrown if entity ID is less than 1.</exception>
        public void NotifyDestroyedEntity(int entityId)
        {
            if (entityId < 1)
                throw new EntityException("Entity ID must be greater than zero.");

            foreach (var system in GetAllSystems())
                system.RemoveEntity(entityId);
        }

        /// <summary>
        /// Notifies and entity has changed its signature, to analize if each system must process it afterwards
        /// </summary>
        /// <param name="entityId">Entity that has changed its signature.</param>
        /// <param name="signature">NEw signature of the entity.</param>
        /// <exception cref="EntityException">Thrown if entity ID is less than 1.</exception>
        public void NotifyChangedEntitySignature(int entityId, Signature signature)
        {
            if (entityId < 1)
                throw new EntityException("Entity ID must be greater than zero.");

            foreach (var system in GetAllSystems())
            {
                if (signature.IsSignedFor(_signatures[system.GetType()]))
                    system.AddEntity(entityId);
                else
                    system.RemoveEntity(entityId);
            }
        }

        /// <summary>
        /// Calls all update systems to be processed.
        /// </summary>
        /// <param name="world">World containing all entities and components to be processed.</param>
        /// <param name="deltaTime">Time passed in seconds since the last rendering.</param>
        public void PerformUpdate(World world, float deltaTime)
        {
            foreach (var updater in _updateSystems)
                updater.Update(world, deltaTime);
        }

        /// <summary>
        /// Calls all render systems to be processed.
        /// </summary>
        /// <param name="world">World containing all entities and components to be processed.</param>
        /// <param name="deltaTime">Time passed in seconds since the last rendering.</param>
        public void PerformRender(World world, float deltaTime)
        {
            foreach(var system in _renderSystems)
                system.Render(world, deltaTime);
        }
    }
}
