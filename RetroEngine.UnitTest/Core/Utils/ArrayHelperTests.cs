using RetroEngine.Core.Utils;

namespace RetroEngine.UnitTest.Core.Utils
{
    /// <summary>
    /// Defines unit test cases for ArrayHelper class.
    /// </summary>
    public class ArrayHelperTests
    {
        [Theory]
        [InlineData(100, 120, 2)]
        [InlineData(150, 200, 3)]
        [InlineData(2048, 2049, 10)]
        public void ArrayHelper_EnsureCapacity_KeepsOldValues(int initialCapacity, int neededCapacity, int multiplier)
        {
            // Arrange
            var rand = new Random();
            var oldArray = new float[initialCapacity];
            var newArray = new float[initialCapacity];
            for (int i = 0; i < initialCapacity; i++)
            {
                oldArray[i] = rand.NextSingle();
                newArray[i] = oldArray[i];
            }

            // Act
            ArrayHelper.EnsureCapacity(ref newArray, neededCapacity, multiplier);

            // Assange
            for (int i = 0; i < oldArray.Length; i++)
            {
                Assert.Equal(oldArray[i], newArray[i]);
            }
        }

        [Theory]
        [InlineData(100, 120, 2)]
        [InlineData(150, 200, 3)]
        [InlineData(2048, 2049, 10)]
        public void ArrayHelper_EnsureCapacity_AddsExpectedCapacity(int initialCapacity, int neededCapacity, int multiplier)
        {
            // Arrange
            var newArray = new float[initialCapacity];

            // Act
            ArrayHelper.EnsureCapacity(ref newArray, neededCapacity, multiplier);

            // Assange
            Assert.Equal(neededCapacity * multiplier, newArray.Length);
        }

        [Theory]
        [InlineData(100, 120, 2)]
        [InlineData(150, 200, 3)]
        [InlineData(2048, 2049, 10)]
        public void ArrayHelper_EnsureCapacity_AddsDefaultValueToNewData(int initialCapacity, int neededCapacity, int multiplier)
        {
            // Arrange
            var rand = new Random();
            var oldArray = new float[initialCapacity];
            var newArray = new float[initialCapacity];
            for (int i = 0; i < initialCapacity; i++)
            {
                oldArray[i] = rand.NextSingle();
                newArray[i] = oldArray[i];
            }

            // Act
            ArrayHelper.EnsureCapacity(ref newArray, neededCapacity, multiplier);

            // Assange
            for (int i = oldArray.Length; i < newArray.Length; i++)
            {
                Assert.Equal(default, newArray[i]);
            }
        }
    }
}
