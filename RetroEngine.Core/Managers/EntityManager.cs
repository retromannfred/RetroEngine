using RetroEngine.Core.Exceptions;
using RetroEngine.Core.Signing;
using RetroEngine.Core.Utils;

namespace RetroEngine.Core.Managers
{
    /// <summary>
    /// Manages a pool of game entities.
    /// </summary>
    /// <param name="maxComponents">Maximum number of components an entity will have.</param>
    /// <param name="entitiesCount">Initial size for component pool.</param>
    public class EntityManager(int maxComponents, int entitiesCount = 1024)
    {
        private int _createdEntities = 0;
        private Signature[] _signatures = new Signature[entitiesCount];

        private readonly int _maxComponents = maxComponents;
        private readonly Queue<int> _freeEntities = new();

        /// <summary>
        /// Creates a new entity in the entity pool.
        /// </summary>
        /// <returns>ID of the new entity.</returns>
        public int Create()
        {
            if (_freeEntities.Count > 0)
            {
                return _freeEntities.Dequeue();
            }

            ArrayHelper.EnsureCapacity(ref _signatures, ++_createdEntities);
            _signatures[_createdEntities] = new Signature(_maxComponents);
            return _createdEntities;
        }

        /// <summary>
        /// Gets the signature of a given entity.
        /// </summary>
        /// <param name="entityId">Entity ID to get the signature from.</param>
        /// <returns></returns>
        /// <exception cref="EntityException">Thrown if the given entity doesn't exists.</exception>
        public Signature GetSignature(int entityId)
        {
            EnsureEntityId(entityId);
            return _signatures[entityId];
        }

        /// <summary>
        /// Sets the signature of a given entity.
        /// </summary>
        /// <param name="entityId">Entity ID to get the signature from.</param>
        /// <param name="signature">Signature to set.</param>
        /// <exception cref="EntityException">Thrown if the given entity doesn't exists.</exception>
        public void SetSignature(int entityId, Signature signature)
        {
            EnsureEntityId(entityId);

            if (signature.Length != _maxComponents)
                throw new ArgumentException("The signature to set does not have the same length than the max components of the entity manager.");

            _signatures[entityId] = signature;
        }

        /// <summary>
        /// Destroys an entity from the entity pool.
        /// </summary>
        /// <param name="entityId">Entity ID to destroy.</param>
        /// <exception cref="EntityException">Thrown if the given entity doesn't exists.</exception>
        public void Destroy(int entityId)
        {
            EnsureEntityId(entityId);
            SetSignature(entityId, new Signature(_maxComponents));
            _freeEntities.Enqueue(entityId);
        }

        private void EnsureEntityId(int entityId)
        {
            if (entityId <= 0 || entityId > _createdEntities || _freeEntities.Contains(entityId))
                throw new EntityException($"Entity {entityId} doesn't exist.");
        }
    }
}
