using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models;
using System;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Domains
{
    public interface ITrilaterationDomain
    {
        Tuple<float, float> GetLocation(float[] distances);
        LocationPoint GetLocation(DistanceToSatellite[] distancesToSatellites);
    }
}
