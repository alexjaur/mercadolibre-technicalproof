using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Domains;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Enums;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Extensions;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.WebApp.Controllers
{
    [ApiController]
    [Route("topsecret")]
    public class TopSecretController : ControllerBase
    {
        private readonly ILogger<TopSecretController> _logger;
        private readonly ITrilaterationDomain _trilaterationDomain;
        private readonly IMessageDomain _messageDomain;


        public TopSecretController(
            ITrilaterationDomain trilaterationDomain, 
            IMessageDomain messageDomain, 
            ILogger<TopSecretController> logger
        )
        {
            _trilaterationDomain = trilaterationDomain;
            _messageDomain = messageDomain;
            _logger = logger;
        }


        [HttpPost]
        public IActionResult Post(TopSecretRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("Invalid argument", nameof(request));
            }

            if (!request.Satellites.HasValues() || request.Satellites.Length != 3 || request.Satellites.GroupBy(x => x.Name).Count() != 3)
            {
                throw new ArgumentException("Invalid satellite configuration.", nameof(request.Satellites));
            }

            LocationPoint locationPoint = GetLocation(request.Satellites);
            if (locationPoint == null)
            {
                return NotFound();
            }

            string message = GetMessage(request.Satellites);
            if (!message.HasValue())
            {
                return NotFound();
            }

            var topSecretResponse = new TopSecretResponse()
            {
                Position = locationPoint,
                Message = message
            };

            return Ok(topSecretResponse);
        }


        private LocationPoint GetLocation(SatelliteModel[] satellites)
        {
            LocationPoint locationPoint = null;
            DistanceToSatellite[] distances = Cast(satellites);

            try
            {
                locationPoint = _trilaterationDomain.GetLocation(distances);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "POST /topsecret; GetLocation");
            }

            return locationPoint;
        }
        
        private string GetMessage(SatelliteModel[] satellites)
        {
            string message = null;

            try
            {
                string[][] messages = satellites.Select(x => x.Message).ToArray();
                message = _messageDomain.GetMessage(messages);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "POST /topsecret; GetMessage");
            }

            return message;
        }

        private DistanceToSatellite[] Cast(SatelliteModel[] satellites)
        {
            var distances = satellites
                                .Select(x => 
                                {
                                    DistanceToSatellite distanceToSatellite = null;

                                    if (Enum.TryParse(x.Name, true, out SatelliteType @enum))
                                    {
                                        distanceToSatellite = new DistanceToSatellite()
                                        {
                                            Distance = x.Distance,
                                            Satellite = @enum
                                        };
                                    }

                                    return distanceToSatellite;
                                })
                                .ToArray();

            return distances;
        }
    }
}
