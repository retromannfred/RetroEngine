namespace RetroEngine.ECS
{
    /// <summary>
    /// Defines functionallity to build an entity-component-system world.
    /// </summary>
    public class WorldBuilder
    {
        private List<Elements.System> _systems;

        /// <summary>
        /// Creates a new world builder.
        /// </summary>
        public WorldBuilder()
        {
            _systems = new List<Elements.System>();
        }

        /// <summary>
        /// Adds a new system to the builder.
        /// </summary>
        /// <param name="system">System to add to the world.</param>
        /// <returns>This builder.</returns>
        public WorldBuilder AddSystem(Elements.System system)
        {
            _systems.Add(system);
            return this;
        }

        /// <summary>
        /// Creates and builds a new world with the specified systems.
        /// </summary>
        /// <returns></returns>
        public World Build()
        {
            World world = new World();

            foreach (var system in _systems)
            {
                world.Register(system);
            }

            return world;
        }
    }
}
