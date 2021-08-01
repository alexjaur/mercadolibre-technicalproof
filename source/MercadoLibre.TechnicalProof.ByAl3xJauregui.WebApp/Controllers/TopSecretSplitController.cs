using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Domains;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Enums;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Extensions;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.WebApp.Controllers
{
    [ApiController]
    [Route("topsecret_split")]
    public class TopSecretSplitController : ControllerBase
    {
        private ITopSecretDomain TopSecretDomain { get; }
        private ILogger<TopSecretController> Logger { get; }


        public TopSecretSplitController(
            ITopSecretDomain topSecretDomain,
            ILogger<TopSecretController> logger
        )
        {
            TopSecretDomain = topSecretDomain;
            Logger = logger;
        }


        [HttpGet]
        public IActionResult Get()
        {
            TopSecretData topSecretData = null;

            try
            {
                topSecretData = TopSecretDomain.GetData();
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Get /topsecret_split; GetData");
            }

            if (topSecretData == null)
            {
                string errorMessage = "There is not enough information to get the location of the spaceship and the message it emits.";
                return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
            }

            return Ok(topSecretData);
        }

        [HttpPost("{satellite_name}")]
        public IActionResult Post([FromRoute] SatelliteType satellite_name, [FromBody] TopSecretSplitRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("Invalid argument", nameof(request));
            }

            if (!request.Message.HasValues())
            {
                throw new ArgumentException("The message is null or empty.", nameof(request.Message));
            }

            MessageReceived messageReceived = new MessageReceived()
            {
                Satellite = satellite_name, 
                Distance = request.Distance,
                Message = request.Message
            };

            try
            {
                TopSecretDomain.SaveData(messageReceived);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "POST /topsecret_split; SaveData");
            }
            
            return Ok();
        }
    }
}
