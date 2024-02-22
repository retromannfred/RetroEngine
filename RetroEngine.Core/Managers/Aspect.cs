using RetroEngine.Core.Elements;

namespace RetroEngine.Core.Managers
{
    /// <summary>
    /// Defines how entities are filter in a system, so just entities with specified components are used in the system.
    /// </summary>
    public class Aspect
    {
        private bool _mustRebuild;
        private List<long> _activeEntities;
        private IQueryable<long> _aspectFilter;

        /// <summary>
        /// Creates a new system aspect.
        /// </summary>
        /// <param name="entities">Query to retrieve entities list.</param>
        internal Aspect(IQueryable<long> entities)
        {
            _aspectFilter = entities;
            _mustRebuild = false;
            _activeEntities = entities.ToList();
        }

        /// <summary>
        /// Creates a new system aspect.
        /// </summary>
        internal Aspect() : this(new List<long>().AsQueryable()) { }

        /// <summary>
        /// Creates a new aspect builder, specifying that all entites must have a component of a specified type.
        /// </summary>
        public static AspectBuilder All<T>() where T : struct, IComponent => new AspectBuilder().All<T>();

        /// <summary>
        /// Creates a new aspect builder, specifies that at least one entity must have a component of a specified type.
        /// </summary>
        public static AspectBuilder Any<T>() where T : struct, IComponent => new AspectBuilder().Any<T>();

        /// <summary>
        /// Creates a new aspect builder, specifies that all entities must not have a component of a specified type.
        /// </summary>
        public static AspectBuilder None<T>() where T : struct, IComponent => new AspectBuilder().None<T>();

        /// <summary>
        /// Handler to be called when and entity has changed (created, destroyed or its components changed).
        /// </summary>
        /// <param name="entityId">ID of the entity.</param>
        internal void EntitiesChangedHandler(long entityId)
        {
            _mustRebuild = true;
        }

        /// <summary>
        /// Gets a list of the active entities for this system aspect.
        /// </summary>
        /// <returns></returns>
        internal List<long> GetActiveEntities()
        {
            if (_mustRebuild)
            {
                _activeEntities = _aspectFilter.ToList();
                _mustRebuild = false;
            }

            return _activeEntities;
        }
    }
}
