using RetroEngine.Core.Exceptions;
using RetroEngine.Core.Signing;

namespace RetroEngine.Core.Elements
{
    /// <summary>
    /// Defines basic functionallity of a system in a ECS engine.
    /// <param name="negotiation">Negotiation containing component types to be processed by this system.</param>
    /// </summary>
    public abstract class BaseSystem(Negotiation negotiation)
    {
        private readonly HashSet<int> _entities = [];
        private readonly Negotiation _negotiation = negotiation;

        /// <summary>
        /// Processes this system in a specified world.
        /// </summary>
        /// <param name="world">World containing all entities and components to be processed.</param>
        /// <param name="deltaTime">Time passed in seconds since the last processing.</param>
        public abstract void Process(World world, float deltaTime);

        /// <summary>
        /// Adds an entity to be processed for this system.
        /// </summary>
        /// <param name="entityId"></param>
        /// <exception cref="EntityException"></exception>
        public void AddEntity(int entityId)
        {
            if (entityId < 1)
                throw new EntityException("Entity ID must be greater than zero.");

            _entities.Add(entityId);
        }

        /// <summary>
        /// Gets entities to be processed in this system.
        /// </summary>
        /// <returns>An instance of IEnumerable with the ID values.</returns>
        public IEnumerable<int> GetEntities()
        {
            return _entities.AsEnumerable();
        }

        /// <summary>
        /// Removes an entity from being processed in this system.
        /// </summary>
        /// <param name="entityId">Entity ID to be removed.</param>
        public void RemoveEntity(int entityId)
        {
            if (entityId < 1)
                throw new EntityException("Entity ID must be greater than zero.");

            _entities.Remove(entityId);
        }

        /// <summary>
        /// Gets the types added to this system's negotation.
        /// </summary>
        /// <returns>An instance of IEnumerable with the Type values.</returns>
        internal IEnumerable<Type> GetNegotiationClauses()
        {
            return _negotiation.GetClauses();
        }

        /// <summary>
        /// Signs the contract for this system.
        /// </summary>
        /// <param name="offer">Contract having all components of the engine.</param>
        /// <returns></returns>
        internal Signature SignNegotiation(Contract offer)
        {
            return _negotiation.Sign(offer);
        }
    }

    /// <summary>
    /// Defines a system that is processed on the update game loop.
    /// <param name="negotiation">Negotiation containing component types to be processed by this system.</param>
    /// </summary>
    public abstract class UpdateSystem(Negotiation negotiation) : BaseSystem(negotiation)
    {
        /// <summary>
        /// Wrapper for the process method.
        /// </summary>
        /// <param name="world">World containing all entities and components to be processed.</param>
        /// <param name="deltaTime">Time passed in seconds since the last updating.</param>
        public void Update(World world, float deltaTime) => this.Process(world, deltaTime);
    }

    /// <summary>
    /// Defines a system that is processed on the render game loop.
    /// <param name="negotiation">Negotiation containing component types to be processed by this system.</param>
    /// </summary>
    public abstract class RenderSystem(Negotiation negotiation) : BaseSystem(negotiation)
    {
        /// <summary>
        /// Wrapper for the process method.
        /// </summary>
        /// <param name="world">World containing all entities and components to be processed.</param>
        /// <param name="deltaTime">Time passed in seconds since the last rendering.</param>
        public void Render(World world, float deltaTime) => this.Process(world, deltaTime);
    }
}
