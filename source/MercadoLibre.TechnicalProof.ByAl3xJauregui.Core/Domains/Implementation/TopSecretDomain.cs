using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Data.Entities;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.DataProviders;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Extensions;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models;
using System;
using System.Linq;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Domains.Implementation
{
    internal class TopSecretDomain : ITopSecretDomain
    {
        private const string Separator = "¬";
        private ITrilaterationDomain TrilaterationDomain { get; }
        private IMessageDomain MessageDomain { get; } 
        private ITopSecretDataProvider TopSecretDataProvider { get; }


        public TopSecretDomain(
            ITrilaterationDomain trilaterationDomain, 
            IMessageDomain messageDomain, 
            ITopSecretDataProvider topSecretDataProvider)
        {
            TrilaterationDomain = trilaterationDomain;
            MessageDomain = messageDomain;
            TopSecretDataProvider = topSecretDataProvider;
        }


        public TopSecretData GetData()
        {
            TopSecretData topSecretData = null;

            var receivedMessages = TopSecretDataProvider.GetReceivedMessages();
            if (receivedMessages.HasValues() && receivedMessages.Count() == 3)
            {
                LocationPoint locationPoint = GetLocation(receivedMessages);
                if (locationPoint != null)
                {
                    string message = GetMessage(receivedMessages);
                    if (message.HasValue())
                    {
                        topSecretData = new TopSecretData(locationPoint.X, locationPoint.Y, message);
                    }
                }
            }

            return topSecretData;
        }

        public bool SaveData(MessageReceived messageReceived)
        {
            if (messageReceived == null)
            {
                throw new ArgumentNullException(nameof(messageReceived));
            }

            if (!messageReceived.Message.HasValues())
            {
                throw new ArgumentException("Is null or empty", nameof(messageReceived));
            }

            string messageWords = string.Join(Separator, messageReceived.Message);

            ReceivedMessages receivedMessage = new ReceivedMessages
            (
                messageReceived.Satellite, 
                messageReceived.Distance,
                messageWords
            );

            bool saved = TopSecretDataProvider.AddOrUpdateReceivedMessage(receivedMessage);

            return saved;
        }


        private LocationPoint GetLocation(ReceivedMessages[] receivedMessages)
        {
            LocationPoint locationPoint = null;
            DistanceToSatellite[] distances = receivedMessages
                                                .Select(x => new DistanceToSatellite()
                                                {
                                                    Satellite = x.Id,
                                                    Distance = x.Distance
                                                })
                                                .ToArray();

            try
            {
                locationPoint = TrilaterationDomain.GetLocation(distances);
            }
            catch
            { }

            return locationPoint;
        }

        private string GetMessage(ReceivedMessages[] receivedMessages)
        {
            string message = null;

            try
            {
                string[][] messages = receivedMessages
                                        .Where(x =>
                                            x.MessageWords.HasValue()
                                        )
                                        .Select(x => 
                                            x.MessageWords.Split(Separator)
                                        )
                                        .ToArray();

                message = MessageDomain.GetMessage(messages);
            }
            catch
            { }

            return message;
        }

    }
}
