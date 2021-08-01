using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Data.Contexts;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Data.Entities;
using System;
using System.Linq;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.DataProviders.Implementation
{
    internal class TopSecretDataProvider : ITopSecretDataProvider
    {
        private TopSecretDbContext DbContext { get; }


        public TopSecretDataProvider(TopSecretDbContext dbContext)
        {
            DbContext = dbContext;
        }


        public bool AddOrUpdateReceivedMessage(ReceivedMessages receivedMessage)
        {
            if (receivedMessage == null)
            {
                throw new ArgumentNullException(nameof(receivedMessage));
            }

            var found = DbContext.ReceivedMessages.Find(receivedMessage.Id);
            if (found == null)
            {
                DbContext.ReceivedMessages.Add(receivedMessage);
            }
            else
            {
                found.Distance = receivedMessage.Distance;
                found.MessageWords = receivedMessage.MessageWords;
            }

            int rowsAffected = DbContext.SaveChanges();

            return rowsAffected > 0;
        }

        public ReceivedMessages[] GetReceivedMessages()
        {
            var receivedMessages = DbContext.ReceivedMessages.ToArray();
            return receivedMessages;
        }
    }
}
