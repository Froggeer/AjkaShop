using System;

namespace Ajka.Common.Helpers
{
    public static class PriceRoundHelper
    {
        public static decimal RoundToFive(decimal priceNumber)
        {
            switch(priceNumber % 10)
            {
                case 3:
                case 8:
                    return Math.Ceiling(priceNumber / 5) * 5;
                default:
                    return Math.Round(priceNumber / 5) * 5;
            }
        }
    }
}
