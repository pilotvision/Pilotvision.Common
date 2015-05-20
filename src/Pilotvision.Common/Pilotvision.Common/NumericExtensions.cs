using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pilotvision.Common
{
    public static class NumericExtensions
    {
        public const decimal CONSUMPTION_TAX_RATE = 5;

        public static decimal Round(this decimal source, int decimals = 0)
        {
            return Math.Round(source, decimals, MidpointRounding.AwayFromZero);
        }

        public static decimal GetConsumptionTax(this decimal value)
        {
            return Math.Floor(value * (CONSUMPTION_TAX_RATE / 100));
        }

        public static decimal GetWithConsumptionTax(this decimal value)
        {
            return Math.Floor(value * (1 + (CONSUMPTION_TAX_RATE / 100)));
        }
    }
}
