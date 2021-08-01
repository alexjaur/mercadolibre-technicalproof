using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Data.Entities
{
    class ReceivedMessages
    {
        public ReceivedMessages()
        { }

        public ReceivedMessages(SatelliteType satellite, float distance, string messageWords)
        {
            Id = satellite;
            Distance = distance;
            MessageWords = messageWords;
        }

        [Key]
        public SatelliteType Id { get; set; }
        public float Distance { get; set; }
        public string MessageWords  { get; set; }
    }
}
