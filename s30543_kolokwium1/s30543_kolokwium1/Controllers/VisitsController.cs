using Microsoft.AspNetCore.Mvc;
using s30543_kolokwium1.Models;
using s30543_kolokwium1.Services;

namespace s30543_kolokwium1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitsController : ControllerBase
    {
        private readonly iVisitService _visitService;

        public VisitsController(iVisitService visitService)
        {
            _visitService = visitService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVisitById(int id)
        {
            var visitExists = await _visitService.DoesVisitExist(id);
            if (!visitExists)
            {
                return NotFound("Visit not found");
            }
            var visit = await _visitService.GetVisitByIdAsync(id);

            visit.services = await _visitService.GetVisitServicesAsync(id);
            return Ok(visit);
        }

        [HttpPost()]
        public async Task<IActionResult> AddVisit([FromBody] VisitCreateDTO visit)
        {
            //var visit = await _visitService.AddVisitAsync(visit);
            //return Created($"/visits/{client.IdClient}", client);
            return Ok(await _visitService.AddVisitAsync(visit));
        }

    }
}