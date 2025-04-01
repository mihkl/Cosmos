using API.Data;
using API.Data.Repos;
using API.Mappers;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace API.Controllers;

[Authorize]
[ApiController]
public class ReservationController(ReservationRepo reservationRepo, LegsRepo legsRepo, ProviderRepo providerRepo,
    PriceListRepo priceListRepo, UserManager<User> userManager) : ControllerBase
{
    private readonly ReservationRepo _reservationRepo = reservationRepo;
    private readonly LegsRepo _legsRepo = legsRepo;
    private readonly ProviderRepo _providerRepo = providerRepo;
    private readonly PriceListRepo _priceListRepo = priceListRepo;
    private readonly UserManager<User> _userManager = userManager;
    [HttpPost("api/reservations")]
    public async Task<IActionResult> CreateReservation([FromBody] ReservationRequest request)
    {
        var userId = _userManager.GetUserId(User);

        List<(Leg Leg, Provider Provider)> legs = [];

        foreach (var leg in request.Legs)
        {
            var legEntity = await _legsRepo.GetLegByIdAsync(leg.Id);

            if (legEntity is null)
            {
                return NotFound($"Leg with ID {leg.Id} not found.");
            }

            var provider = await _providerRepo.GetProviderByIdAsync(leg.Provider.Id);

            if (provider is null)
            {
                return NotFound($"Provider with ID {leg.Provider.Id} not found.");
            }

            legs.Add((legEntity, provider));
        }

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            PriceListId = _priceListRepo.GetActivePriceListAsync().Result.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            ReservationLegs = [.. legs.Select(l => new ReservationLeg
                {
                    Id = Guid.NewGuid(),
                    Leg = l.Leg,
                    Provider = l.Provider,
                })],
            TotalQuotedPrice = request.TotalQuotedPrice,
            TransportationCompanyNames = request.TransportationCompanyNames,
            CreatedAt = DateTime.UtcNow,
        };

        await _reservationRepo.AddAsync(reservation, userId);

        return CreatedAtAction(nameof(CreateReservation), new { id = reservation.Id }, reservation.ToDto());
    }


    [HttpGet("api/reservations")]
    public async Task<IActionResult> GetReservations()
    {
        var userId = _userManager.GetUserId(User);
        var reservations = await _reservationRepo.GetAllAsync(userId);

        return Ok(reservations.Select(r => r.ToDto()).ToList());
    }
}
