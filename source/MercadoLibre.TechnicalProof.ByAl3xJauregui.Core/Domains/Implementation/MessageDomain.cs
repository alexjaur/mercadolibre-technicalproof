using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Extensions;
using System;
using System.Linq;
using System.Text;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Domains.Implementation
{
    internal class MessageDomain : IMessageDomain
    {
        public string GetMessage(string[][] messages)
        {
            if (!messages.HasValues())
            {
                throw new ArgumentException("Is null or empty.", nameof(messages));
            }

            if (messages.Length != 3)
            {
                throw new ArgumentException("Exactly 3 messages are required.", nameof(messages));
            }

            if (messages.Where(m => m.HasValues()).Count() != 3)
            {
                throw new ArgumentException("There are some null or empty messages.", nameof(messages));
            }

            var maxLength = messages.Max(x => x.Length);

            var sanitizedMessages = messages
                                        .Select(message =>
                                            Enumerable
                                                .Repeat(string.Empty, maxLength - message.Length)
                                                .Concat(message)
                                                .ToArray()
                                        )
                                        .ToArray();

            string[]
                message0 = sanitizedMessages[0],
                message1 = sanitizedMessages[1],
                message2 = sanitizedMessages[2];

            var stringBuilder = new StringBuilder();

            for (int index = 0; index < maxLength; index++)
            {
                var messagePart = new[]
                                  {
                                      message0[index],
                                      message1[index],
                                      message2[index]
                                  }
                                  .Where(x => x.HasValue())
                                  .GroupBy(x => x)
                                  .OrderByDescending(x => x.Count())
                                  .SelectMany(x => x)
                                  .FirstOrDefault();

                if (messagePart.HasValue())
                {
                    stringBuilder.Append($"{messagePart} ");
                }
            }

            string finalMessage = stringBuilder
                                    .ToString()
                                    .Trim();

            return finalMessage;
        }
    }
}
