using System.Security.Cryptography.X509Certificates;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace API.Data.Repos;

public class ReservationRepo(DataContext dataContext)
{
    private readonly DataContext _dataContext = dataContext;

    public async Task AddAsync(Reservation reservation)
    {
        await _dataContext.Reservations.AddAsync(reservation);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<List<Reservation>> GetAllAsync()
    {
        var reservations = await _dataContext.Reservations
            .Include(r => r.ReservationLegs)
                .ThenInclude(rl => rl.Leg)
                .ThenInclude(l => l.Providers)
                    .ThenInclude(p => p.Company)
            .Include(r => r.ReservationLegs)
                .ThenInclude(rl => rl.Leg)
                .ThenInclude(l => l.RouteInfo)
                    .ThenInclude(r => r.From)
            .Include(r => r.ReservationLegs)
                .ThenInclude(rl => rl.Leg)
                .ThenInclude(l => l.RouteInfo)
                    .ThenInclude(r => r.To)
            .ToListAsync();
        return reservations ?? [];
    }
}
