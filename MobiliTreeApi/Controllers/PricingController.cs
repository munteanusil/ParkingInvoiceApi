using Microsoft.AspNetCore.Mvc;
using MobiliTreeApi.Domain;
using MobiliTreeApi.Services;

namespace MobiliTreeApi.Controllers
{
    [ApiController]
    [Route("api/pricing")]
    public class PricingController : Controller
    {
       
        private readonly ISessionPricingService _pricingService;


        public PricingController(ISessionPricingService privateService)
        {
            _pricingService = privateService;
        }
        

        [HttpPost("costSimulation")]
        public ActionResult<decimal> CostSimulation([FromBody] PricingRequest request)
        {
            if (request == null || request.session == null)
                return BadRequest("Invalid request");

            var result = _pricingService.CalculateCost(request.session, request.hasContract);
            return Ok(result);

        }
    }
}
