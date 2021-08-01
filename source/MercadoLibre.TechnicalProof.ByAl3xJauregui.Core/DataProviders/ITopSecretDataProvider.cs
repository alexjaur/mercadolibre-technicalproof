using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Data.Entities;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.DataProviders
{
    internal interface ITopSecretDataProvider
    {
        bool AddOrUpdateReceivedMessage(ReceivedMessages receivedMessage);
        ReceivedMessages[] GetReceivedMessages();
    }
}
