using System.Reflection;

namespace RetroEngine.Core
{
    /// <summary>
    /// Defines a builder to create ECS worlds based on the systems that will be processed.
    /// </summary>
    public class WorldBuilder()
    {
        private readonly Dictionary<Type, BaseSystem> _registeredSystems = [];

        /// <summary>
        /// Registers a new system to be processed in the constructed world.
        /// </summary>
        /// <typeparam name="T">Type of the system.</typeparam>
        /// <param name="system">System to register.</param>
        /// <returns>This world builder.</returns>
        public WorldBuilder RegisterSystem<T>(T system)
            where T : BaseSystem
        {
            if (_registeredSystems.ContainsKey(typeof(T)) == false)
                _registeredSystems.Add(typeof(T), system);

            return this;
        }

        /// <summary>
        /// Creates a new world based on the previous registered systems.
        /// </summary>
        /// <returns>An instance of the World class.</returns>
        public World Build()
        {
            var worldContract = new Contract();
            foreach (var system in _registeredSystems.Values)
            {
                worldContract.Extend(system.GetNegotiationClauses());
            }

            var entityManager = new EntityManager(worldContract.GetClauses().Count());

            var componentManager = new ComponentManager();
            foreach (var comp in worldContract.GetClauses())
            {
                componentManager.GetType()
                    .GetMethod("Register", BindingFlags.Instance | BindingFlags.Public) !
                    .MakeGenericMethod(comp)
                    .Invoke(componentManager, null);
            }

            var systemManager = new SystemManager();
            foreach (var system in _registeredSystems)
            {
                var signature = system.Value.SignNegotiation(worldContract);
                systemManager.GetType()
                    .GetMethod("AddSystem", BindingFlags.Instance | BindingFlags.Public)!
                    .MakeGenericMethod(system.Key)
                    .Invoke(systemManager, [system.Value]);
                systemManager.GetType()
                    .GetMethod("SetSignature", BindingFlags.Instance | BindingFlags.Public)!
                    .MakeGenericMethod(system.Key)
                    .Invoke(systemManager, [signature]);
            }

            return new World(
                entityManager,
                componentManager,
                systemManager);
        }
    }
}
