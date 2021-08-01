using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Domains
{
    public interface ITopSecretDomain
    {
        bool SaveData(MessageReceived messageReceived);
        TopSecretData GetData();
    }
}
