using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Enums;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Extensions
{
    public static class UtilsExtensions
	{
		public static bool HasValues<T>(this IEnumerable<T> items)
		{
			bool hasValues = items?.Any() ?? false;
			return hasValues;
		}

		public static bool HasValue(this string @string)
		{
			bool hasValue = !string.IsNullOrWhiteSpace(@string);
			return hasValue;
		}

		public static bool IsValidResult(this CirclesIntersectionsResultType @enum)
		{
			var validOptions = CirclesIntersectionsResultType.SuccessfulWithOneSolution 
							   | CirclesIntersectionsResultType.SuccessfulWithTwoSolutions;

			var valid = validOptions.HasFlag(@enum);

			return valid;
		}

		public static bool InRange(this LocationPoint locationPoint0, LocationPoint locationPoint1, double tolerance)
		{
			bool result = false;

			if (locationPoint0 != null && locationPoint1 != null)
            {
				result = InRange(locationPoint0.X, locationPoint0.Y, locationPoint1.X, locationPoint1.Y, tolerance);
			}

			return result;
		}

		public static bool InRange(this Tuple<float, float> locationPoint0, Tuple<float, float> locationPoint1, double tolerance)
		{
			bool result = false;

			if (locationPoint0 != null && locationPoint1 != null)
			{
				result = InRange(locationPoint0.Item1, locationPoint0.Item2, locationPoint1.Item1, locationPoint1.Item2, tolerance);
			}

			return result;
		}

		public static void Sanitize(this SatelliteLocation[] satellites)
		{
			if (!satellites.HasValues())
			{
				throw new InvalidOperationException("No satellites found.");
			}

			var validOptions = SatelliteType.Kenobi 
							   | SatelliteType.Skywalker 
							   | SatelliteType.Sato;

			satellites = satellites
							.Where(x => 
								validOptions.HasFlag(x.Satellite)
							)
							.ToArray();

			if (satellites.Length != 3 || satellites.GroupBy(x => x.Satellite).Count() != 3)
			{
				throw new InvalidOperationException("Exactly 3 different satellites are required.");
			}
		}


		internal static bool InRange(float x1, float y1, float x2, float y2, double tolerance)
		{
			bool isInRange = Math.Abs(Math.Abs(x1) - Math.Abs(x2)) < tolerance
						     && Math.Abs(Math.Abs(y1) - Math.Abs(y2)) < tolerance;

			return isInRange;
		}
	}
}
