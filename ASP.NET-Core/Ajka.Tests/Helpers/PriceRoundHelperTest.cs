using System.Linq;
using Ajka.Common.Helpers;
using Xunit;

namespace Ajka.Tests.Helpers
{
    public class PriceRoundHelperTest
    {
        [Fact]
        public void RoundToFive_Succeeds()
        {
            // Arrange

            var expectations = new decimal[] { 10, 10, 10, 15, 15, 15, 15, 15, 20, 20, 20, 20 };
            var numbers = Enumerable.Range(10, 12).ToArray();

            // Act & Assert

            for(var index = 0; index < numbers.Length; index++)
            {
                var result = PriceRoundHelper.RoundToFive(numbers[index]);
                Assert.Equal(result, expectations[index]);
            }
        }
    }
}
