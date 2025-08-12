namespace RetroEngine.Core.Utils
{
    /// <summary>
    /// Defines extensions methods to manipulate arrays.
    /// </summary>
    public static class ArrayHelper
    {
        /// <summary>
        /// Ensures the capacity of an array, multiplying its size if needs more capacity.
        /// </summary>
        /// <typeparam name="T">Data type of the array.</typeparam>
        /// <param name="array">Array to ensure its capacity.</param>
        /// <param name="capacity">Capacity to ensure.</param>
        /// <param name="multiplier">Multiplier to the size to ensure capacity.</param>
        public static void EnsureCapacity<T>(ref T[] array, int capacity, int multiplier = 2)
            where T : struct
        {
            if (array.Length <= capacity)
            {
                var newArray = new T[capacity * multiplier];
                Array.Copy(array, newArray, array.Length);
                array = newArray;
            }
        }
    }
}
