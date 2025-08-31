namespace RetroEngine.Core
{
    /// <summary>
    /// Defines a builder to create signatures.
    /// </summary>
    public class Negotiation
    {
        private readonly Contract _contract = new();

        /// <summary>
        /// Gets the clauses to sign in this negotiation.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetClauses() => _contract.GetClauses();

        /// <summary>
        /// Indicates that the contract must contain a sign for a type of component.
        /// </summary>
        /// <typeparam name="T">Type of the component.</typeparam>
        /// <returns>This signature builder.</returns>
        public Negotiation Include<T>()
            where T : struct
        {
            _contract.Extend<T>();
            return this;
        }

        /// <summary>
        /// Creates a new signature.
        /// </summary>
        /// <param name="offer">Contract with types that could be signed for.</param>
        /// <returns></returns>
        public Signature Sign(Contract offer)
        {
            var offerLength = offer.GetClauses().Count();
            var clausesToSign = _contract.GetClauses();
            var signature = new Signature(offerLength);

            for (int i = 0; i < offerLength; i++)
            {
                if (clausesToSign.Contains(offer[i]))
                    signature[i] = true;
            }

            return signature;
        }
    }
}
