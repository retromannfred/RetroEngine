namespace RetroEngine.Core.Signing
{
    /// <summary>
    /// Defines a list of types which and ECS element can sign for.
    /// </summary>
    public readonly struct Contract()
    {
        private readonly List<Type> _types = [];

        /// <summary>
        /// Creates a new negotation to sign a contract, including one type of component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Negotiation Include<T>()
            where T : struct
            => new Negotiation().Include<T>();

        /// <summary>
        /// Gets a type of component by its clause index as it was included.
        /// </summary>
        /// <param name="clause">Index indicating the order the type was included.</param>
        /// <returns>A type of a component.</returns>
        public Type this[int clause]
        {
            get => _types[clause];
        }

        /// <summary>
        /// Gets all the clause types of this contract in the order they were included.
        /// </summary>
        /// <returns>An instance of IEnumerable class with all the types.</returns>
        public IEnumerable<Type> GetClauses() => _types.AsEnumerable();

        /// <summary>
        /// Extends this contract adding a new type to sign for.
        /// </summary>
        /// <param name="type">Type of the component.</param>
        /// <returns>This contract instance.</returns>
        /// <exception cref="ArgumentException">Thrown if the type is not a struct.</exception>
        public Contract Extend(Type type)
        {
            if (!type.IsValueType || type.IsPrimitive)
                throw new ArgumentException("Type for a contract must be a struct.");

            if (_types.Contains(type) is false)
                _types.Add(type);

            return this;
        }

        /// <summary>
        /// Extends this contract adding a list of types to sign for.
        /// </summary>
        /// <param name="clauses">List of types to add.</param>
        /// <returns></returns>
        public Contract Extend(IEnumerable<Type> clauses)
        {
            foreach (var clause in clauses)
            {
                Extend(clause);
            }

            return this;
        }

        /// <summary>
        /// Extends this contract adding a new type to sign for.
        /// </summary>
        /// <typeparam name="T">Type of the component.</typeparam>
        /// <returns>This contract instance.</returns>
        public Contract Extend<T>()
            where T : struct
            => Extend(typeof(T));

        /// <summary>
        /// Extends this contract adding a list of types of another contract.
        /// </summary>
        /// <param name="other">Contract containing clauses to add.</param>
        /// <returns></returns>
        public Contract Extend(Contract other) => Extend(other.GetClauses());
    }
}
