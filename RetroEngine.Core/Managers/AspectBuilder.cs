using RetroEngine.Core.Elements;

namespace RetroEngine.Core.Managers
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
            this._allTypes = new List<Type>();
            this._anyTypes = new List<Type>();
            this._noneTypes = new List<Type>();
        }

        /// <summary>
        /// Specifies that all entites must have a component of a specified type.
        /// </summary>
        /// <typeparam name="T">Type of the component in the filter.</typeparam>
        /// <returns>This aspect builder.</returns>
        public AspectBuilder All<T>() where T : struct, IComponent
        {
            this._allTypes.Add(typeof(T));
            return this;
        }

        /// <summary>
        /// Specifies that at least one entity must have a component of a specified type.
        /// </summary>
        /// <typeparam name="T">Type of the component in the filter.</typeparam>
        /// <returns>This aspect builder.</returns>
        public AspectBuilder Any<T>() where T : struct, IComponent
        {
            this._anyTypes.Add(typeof(T));
            return this;
        }

        /// <summary>
        /// Specifies that all entities must not have a component of a specified type.
        /// </summary>
        /// <typeparam name="T">Type of the component in the filter.</typeparam>
        /// <returns>This aspect builder.</returns>
        public AspectBuilder None<T>() where T : struct, IComponent
        {
            this._noneTypes.Add(typeof(T));
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

            if (this._noneTypes.Any())
                entities = entities.Except(
                    entities.Where(e => this._noneTypes.Any(t => world.EntityHasComponent(e, t)))
                );

            if (this._allTypes.Any())
                entities = entities.Where(e => this._allTypes.All(t => world.EntityHasComponent(e, t)));

            if (this._anyTypes.Any())
                entities = entities.Where(e => this._anyTypes.Any(t => world.EntityHasComponent(e, t)));

            return new Aspect(entities);
        }
    }
}
