using RetroEngine.Core.Managers;

namespace RetroEngine.Core.Elements
{
    /// <summary>
    /// Defines the basic behaviour of a component system.
    /// </summary>
    public abstract class System
    {
        private Aspect _aspect;
        private readonly AspectBuilder _aspectBuilder;

        protected World World { get; private set; }

        /// <summary>
        /// Creates a new System with a given aspect specification.
        /// </summary>
        /// <param name="builder">AspectBuilder for component restriction.</param>
        public System(AspectBuilder builder)
        {
            _aspect = new Aspect();
            _aspectBuilder = builder;
        }

        /// <summary>
        /// Initializes this system in a given world.
        /// </summary>
        /// <param name="world"></param>
        internal void Initialize(World world)
        {
            World = world;
            _aspect = _aspectBuilder.Build(world);

            World.OnEntityCreated += _aspect.EntitiesChangedHandler;
            World.OnEntityDestroyed += _aspect.EntitiesChangedHandler;
            World.OnComponentAdded += _aspect.EntitiesChangedHandler;
            World.OnComponentRemoved += _aspect.EntitiesChangedHandler;
        }

        /// <summary>
        /// Gets active entities for this system.
        /// </summary>
        protected List<long> ActiveEntities => _aspect.GetActiveEntities();
    }
}
