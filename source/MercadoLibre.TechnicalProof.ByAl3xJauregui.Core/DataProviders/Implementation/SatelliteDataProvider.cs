using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Enums;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.DataProviders.Implementation
{
    internal class SatelliteDataProvider : ISatelliteDataProvider
    {
        public SatelliteLocation[] GetAvailableSatellites()
        {
            var satellites = new SatelliteLocation[]
            {
                new SatelliteLocation(SatelliteType.Kenobi, -500, -200),
                new SatelliteLocation(SatelliteType.Skywalker, 100, -100),
                new SatelliteLocation(SatelliteType.Sato, 500, 100)
            };

            return satellites;
        }
    }
}
