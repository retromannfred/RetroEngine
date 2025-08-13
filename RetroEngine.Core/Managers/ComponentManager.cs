using RetroEngine.Core.Elements;
using RetroEngine.Core.Exceptions;

namespace RetroEngine.Core.Managers
{
    /// <summary>
    /// Manages collection of component mappers.
    /// </summary>
    public class ComponentManager()
    {
        private readonly IDictionary<Type, IComponentMapper> _mappers = new Dictionary<Type, IComponentMapper>();
        private readonly IDictionary<Type, int> _signatureIndices = new Dictionary<Type, int>();

        /// <summary>
        /// Registers a new type of component, adding its mapper to the manager.
        /// </summary>
        /// <typeparam name="T">Type of the component to add.</typeparam>
        public void Register<T>()
            where T : struct
        {
            _mappers.Add(typeof(T), new ComponentMapper<T>());
            _signatureIndices.Add(typeof(T), _signatureIndices.Count);
        }

        /// <summary>
        /// Gets the register index of a component, that matches same position in a signature.
        /// </summary>
        /// <typeparam name="T">Type of the component.</typeparam>
        /// <returns>Flag position of the component in a signature.</returns>
        public int GetSignatureIndex<T>()
            where T : struct
        {
            return _signatureIndices[typeof(T)];
        }

        /// <summary>
        /// Adds a component of a specified type to an entity.
        /// </summary>
        /// <typeparam name="T">Type of the component to add.</typeparam>
        /// <param name="entityId">Entity ID to attach the component.</param>
        /// <param name="component">Component to add.</param>
        /// <exception cref="EntityException">Thrown if entityId is lower than 1.</exception>
        public void AddComponent<T>(int entityId, T component)
            where T : struct
        {
            if (_mappers.TryGetValue(typeof(T), out var mapper))
                ((ComponentMapper<T>)mapper).Insert(entityId, component);
            else
                throw new RegisterException("Cannot get a non registered component.");
        }

        /// <summary>
        /// Gets the component of an specified type attached to an entity.
        /// </summary>
        /// <typeparam name="T">Type of the component to add.</typeparam>
        /// <param name="entityId">Entity ID where the component is attached.</param>
        /// <exception cref="RegisterException">Thrown when try to retrieve a component that was not registered.</exception>
        /// <exception cref="EntityException">Thrown if entityId is lower than 1.</exception>
        /// <exception cref="ComponentException">Thrown if the entity doesn't have a component of this mapper type.</exception>
        public ref T GetComponent<T>(int entityId)
            where T : struct
        {
            if (_mappers.TryGetValue(typeof(T), out var mapper))
            {
                return ref ((ComponentMapper<T>)mapper).Get(entityId);
            }

            throw new RegisterException("Cannot get a non registered component.");
        }

        /// <summary>
        /// Removes a component of a specified type from an entity.
        /// </summary>
        /// <typeparam name="T">Type of the component to remove.</typeparam>
        /// <param name="entityId">Entity ID to remove the component from.</param>
        /// <exception cref="EntityException">Thrown if entityId is lower than 1.</exception>
        /// <exception cref="ComponentException">Thrown if the entity doesn't have a component of this mapper type.</exception>
        public void RemoveComponent<T>(int entityId)
            where T : struct
        {
            if (_mappers.TryGetValue(typeof(T), out var mapper))
            {
                ((ComponentMapper<T>)mapper).Remove(entityId);
            }
        }

        /// <summary>
        /// Removes all the components from an entity.
        /// </summary>
        /// <param name="entityId">Entity ID to remove the components from.</param>
        /// <exception cref="EntityException">Thrown if entityId is lower than 1.</exception>
        public void RemoveAllComponents(int entityId)
        {
            foreach (var mapper in _mappers.Values)
            {
                try
                {
                    mapper.Remove(entityId);
                }
                catch (ComponentException) { }
            }
        }
    }
}
