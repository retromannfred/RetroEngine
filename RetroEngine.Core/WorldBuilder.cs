using RetroEngine.Core.Elements;
using RetroEngine.Core.Exceptions;
using RetroEngine.Core.Managers;
using RetroEngine.Core.Signing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Core
{
    /// <summary>
    /// Defines a builder to create ECS worlds based on the systems that will be processed.
    /// </summary>
    public class WorldBuilder()
    {
        private readonly List<BaseSystem> _registeredSystems = [];

        /// <summary>
        /// Registers a new system to be processed in the constructed world.
        /// </summary>
        /// <typeparam name="T">Type of the system.</typeparam>
        /// <param name="system">System to register.</param>
        /// <returns>This world builder.</returns>
        public WorldBuilder RegisterSystem<T>(T system)
            where T : BaseSystem
        {
            if (_registeredSystems.Contains(system) == false)
                _registeredSystems.Add(system);

            return this;
        }

        /// <summary>
        /// Creates a new world based on the previous registered systems.
        /// </summary>
        /// <returns>An instance of the World class.</returns>
        public World Build()
        {
            var worldContract = new Contract();
            foreach (var system in _registeredSystems)
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
                var signature = system.SignNegotiation(worldContract);
                systemManager.AddSystem(system, signature);
            }

            return new World(
                entityManager,
                componentManager,
                systemManager);
        }
    }
}
