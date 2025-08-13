namespace RetroEngine.Core.Elements
{
    /// <summary>
    /// Defines how an ECS element is signed for a group of components.
    /// </summary>
    public readonly struct Signature
    {
        private const int BitShiftPerInt32 = 5;
        private const int BitShiftForBytesPerInt32 = 2;

        private readonly int[] _data;
        private readonly int _length;

        /// <summary>
        /// Creates a new signature.
        /// </summary>
        public Signature() : this(32) { }

        /// <summary>
        /// Creates a new signature.
        /// </summary>
        /// <param name="length">How many components this signature is for.</param>
        /// <exception cref="ArgumentException">Thrown if length is less than 1.</exception>
        public Signature(int length)
        {
            var dataLength = (int)((uint)(length - 1 + (1 << BitShiftForBytesPerInt32)) >> BitShiftForBytesPerInt32);
            _data = new int[dataLength];
            _length = length;
        }

        /// <summary>
        /// Gets this signature's length.
        /// </summary>
        public int Length => _length;

        /// <summary>
        /// Gets or sets the value of this signature for a component.
        /// </summary>
        /// <param name="compId">Component ID.</param>
        /// <returns></returns>
        public bool this[int compId]
        {
            get => Get(compId);
            set => Set(compId, value);
        }

        /// <summary>
        /// Gets the value of this signature for a component.
        /// </summary>
        /// <param name="compId">Component ID.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the component ID is greater or equal than the signature length.</exception>
        public bool Get(int compId)
        {
            if ((uint)compId >= (uint)_length)
                throw new IndexOutOfRangeException("Component ID must be less than the signature's length.");

            return (_data[compId >> BitShiftPerInt32] & 1 << compId) != 0;
        }

        /// <summary>
        /// Sets the value of this signature for a component.
        /// </summary>
        /// <param name="compId">Component ID.</param>
        /// <param name="value">Value indicating if the signature if or not for this component ID.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if the component ID is greater or equal than the signature length.</exception>
        public void Set(int compId, bool value)
        {
            if ((uint)compId >= (uint)_length)
                throw new IndexOutOfRangeException("Component ID must be less than the signature's length.");

            int bitMask = 1 << compId;
            ref int segment = ref _data[compId >> BitShiftPerInt32];

            if (value)
            {
                segment |= bitMask;
            }
            else
            {
                segment &= ~bitMask;
            }
        }

        /// <summary>
        /// Determines wether this signature is signed at least for the same components than other signature.
        /// </summary>
        /// <param name="other">Other signature to compare.</param>
        /// <returns>True if this signature has at least same components signed for than the other one, and false otherwise.</returns>
        /// <remarks>If this signature has aditional signed components than the other one, those are ignored in the compraison.</remarks>
        /// <exception cref="ArgumentException"></exception>
        public bool IsSignedFor(Signature other)
        {
            if (_length != other.Length)
                throw new ArgumentException("Cannot compare signatures of different length.");

            for (int i = 0; i < _length; i++)
            {
                if (other[i] && !this[i])
                    return false;
            }

            return true;
        }
    }
}
