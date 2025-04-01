using API.Data.Repos;
using API.Mappers;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace API.Controllers
{
    [ApiController]
    public class TravelController(PriceListRepo priceListRepo): ControllerBase
    {
        private readonly PriceListRepo _priceListRepo = priceListRepo;
        [HttpGet("api/routes")]
        public async Task<IActionResult> GetRoutes([FromQuery] Planet from, [FromQuery] Planet to, [FromQuery] SpaceCompany[] companies)
        {
            var priceList = await _priceListRepo.GetActivePriceListAsync();
            var legs = priceList.Legs.ToList();
            var routes = RouteFinder.FindRoutes(from, to, legs, 5);

            routes = [.. routes.Where(route => route.All(legProvider => companies.Contains(legProvider.Provider.Company.Name)))];

            var routeDtos = routes.Select(route => route.ToDto()).ToList();
            return Ok(routeDtos);
        }
    }
}