using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Enums;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Extensions;
using System;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models
{
    public class MessageReceived
    {
        public MessageReceived()
        { }

        public MessageReceived(SatelliteType satellite, float distance, string[] message)
        {
            if (distance == float.NaN)
            {
                throw new ArgumentException("Value cannot be 'float.NaN'", nameof(distance));
            }

            if (!message.HasValues())
            {
                throw new ArgumentException("Is null or empty", nameof(message));
            }

            Satellite = satellite;
            Distance = distance;
            Message = message;
        }

        public SatelliteType Satellite { get; set; }
        public float Distance { get; set; }
        public string[] Message { get; set; }
    }
}
