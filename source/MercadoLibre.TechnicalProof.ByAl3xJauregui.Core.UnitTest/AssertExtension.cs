using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Extensions;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models;
using System;
using Xunit;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.UnitTest
{
    internal static class AssertExtension
    {
        public const float DefaultToleranceForLocationPoints = 0.05F;

        public static void InRange(LocationPoint expected, LocationPoint actual, float tolerance = DefaultToleranceForLocationPoints)
        {
            Assert.NotNull(expected);

            if (expected != null)
            {
                Assert.NotNull(actual);

                if (actual != null)
                {
                    Assert.True(expected.InRange(actual, tolerance));
                }
            }
        }

        public static void InRange(Tuple<float, float> expected, Tuple<float, float> actual, float tolerance = DefaultToleranceForLocationPoints)
        {
            Assert.NotNull(expected);

            if (expected != null)
            {
                Assert.NotNull(actual);

                if (actual != null)
                {
                    Assert.True(expected.InRange(actual, tolerance));
                }
            }
        }
    }
}
