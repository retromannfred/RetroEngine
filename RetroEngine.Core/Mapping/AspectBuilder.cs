using RetroEngine.Core.Elements;

namespace RetroEngine.Core.Mapping
{
    /// <summary>
    /// Defines how an aspect of a system is created.
    /// </summary>
    public class AspectBuilder
    {
        private List<Type> _allTypes;
        private List<Type> _anyTypes;
        private List<Type> _noneTypes;

        /// <summary>
        /// Creates a new aspect builder.
        /// </summary>
        public AspectBuilder()
        {
            _allTypes = new List<Type>();
            _anyTypes = new List<Type>();
            _noneTypes = new List<Type>();
        }

        /// <summary>
        /// Specifies that all entites must have a component of a specified type.
        /// </summary>
        /// <typeparam name="T">Type of the component in the filter.</typeparam>
        /// <returns>This aspect builder.</returns>
        public AspectBuilder All<T>() where T : struct, IComponent
        {
            _allTypes.Add(typeof(T));
            return this;
        }

        /// <summary>
        /// Specifies that at least one entity must have a component of a specified type.
        /// </summary>
        /// <typeparam name="T">Type of the component in the filter.</typeparam>
        /// <returns>This aspect builder.</returns>
        public AspectBuilder Any<T>() where T : struct, IComponent
        {
            _anyTypes.Add(typeof(T));
            return this;
        }

        /// <summary>
        /// Specifies that all entities must not have a component of a specified type.
        /// </summary>
        /// <typeparam name="T">Type of the component in the filter.</typeparam>
        /// <returns>This aspect builder.</returns>
        public AspectBuilder None<T>() where T : struct, IComponent
        {
            _noneTypes.Add(typeof(T));
            return this;
        }

        /// <summary>
        /// Builds a new aspect.
        /// </summary>
        /// <param name="world">world</param>
        /// <returns></returns>
        internal Aspect Build(World world)
        {
            IQueryable<long> entities = world.GetAllEntityIDs();

            if (_noneTypes.Any())
                entities = entities.Except(
                    entities.Where(e => _noneTypes.Any(t => world.EntityHasComponent(e, t)))
                );

            if (_allTypes.Any())
                entities = entities.Where(e => _allTypes.All(t => world.EntityHasComponent(e, t)));

            if (_anyTypes.Any())
                entities = entities.Where(e => _anyTypes.Any(t => world.EntityHasComponent(e, t)));

            return new Aspect(entities);
        }
    }
}
