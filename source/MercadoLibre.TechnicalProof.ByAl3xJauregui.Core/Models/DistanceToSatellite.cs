using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Enums;
using System;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models
{
    public class DistanceToSatellite
    {
        public DistanceToSatellite()
        { }

        public DistanceToSatellite(SatelliteType satellite, float distance)
        {
            if (distance == float.NaN)
            {
                throw new ArgumentException("Value cannot be 'float.NaN'", nameof(distance));
            }

            Satellite = satellite;
            Distance = distance;
        }

        public float Distance { get; set; }
        public SatelliteType Satellite { get; set; }
    }
}
