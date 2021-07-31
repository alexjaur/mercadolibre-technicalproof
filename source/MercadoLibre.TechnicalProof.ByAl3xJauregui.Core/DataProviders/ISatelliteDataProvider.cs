using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.DataProviders
{
    public interface ISatelliteDataProvider
    {
        SatelliteLocation[] GetAvailableSatellites();
    }
}
