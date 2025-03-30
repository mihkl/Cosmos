using API.Data;
using API.Data.Repos;
using API.Mappers;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace API.Controllers;

[ApiController]
public class ReservationController(ReservationRepo reservationRepo, LegsRepo legsRepo, ProviderRepo providerRepo) : ControllerBase
{
    [HttpPost("api/reservations")]
    public async Task<IActionResult> CreateReservation([FromBody] ReservationRequest request)
    {
        List<(Leg Leg, Provider Provider)> legs = [];

        foreach (var leg in request.Legs)
        {
            var legEntity = await legsRepo.GetLegByIdAsync(leg.Id);

            if (legEntity is null)
            {
                return NotFound($"Leg with ID {leg.Id} not found.");
            }

            var provider = await providerRepo.GetProviderByIdAsync(leg.Provider.Id);

            if (provider is null)
            {
                return NotFound($"Provider with ID {leg.Provider.Id} not found.");
            }

            legs.Add((legEntity, provider));
        }

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            ReservationLegs = [.. legs.Select(l => new ReservationLeg
                {
                    Id = Guid.NewGuid(),
                    Leg = l.Leg,
                    Provider = l.Provider,
                })],
            TotalQuotedPrice = request.TotalQuotedPrice,
            TransportationCompanyNames = request.TransportationCompanyNames
        };

        await reservationRepo.AddAsync(reservation);

        return CreatedAtAction(nameof(CreateReservation), new { id = reservation.Id }, reservation.ToDto());
    }


    [HttpGet("api/reservations")]
    public async Task<IActionResult> GetReservations()
    {
        var reservations = await reservationRepo.GetAllAsync();

        return Ok(reservations.Select(r => r.ToDto()).ToList());
    }
}
