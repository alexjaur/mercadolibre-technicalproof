using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Enums;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models
{
    public class SatelliteLocation
    {
        public SatelliteLocation()
        { }

        public SatelliteLocation(SatelliteType satellite, float x, float y)
        {
            Satellite = satellite;
            Location = new LocationPoint(x, y);
        }

        public SatelliteType Satellite { get; set; }
        public LocationPoint Location { get; set; }
    }
}
