using System.Security.Cryptography.X509Certificates;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace API.Data.Repos;

public class ReservationRepo(DataContext dataContext)
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<Reservation?> AddAsync(Reservation reservation, string? userId)
    {
        var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
        {
            return null;
        }
        await _dataContext.Reservations.AddAsync(reservation);
        user.Reservations.Add(reservation);
        await _dataContext.SaveChangesAsync();
        return reservation;
    }

    public async Task<List<Reservation>> GetAllAsync(string? userId)
    {
        var user = await _dataContext.Users
            .Include(u => u.Reservations)
                .ThenInclude(r => r.ReservationLegs)
                .ThenInclude(rl => rl.Leg)
                .ThenInclude(l => l.RouteInfo)
                .ThenInclude(r => r.From)
            .Include(u => u.Reservations)
                .ThenInclude(r => r.ReservationLegs)
                .ThenInclude(rl => rl.Leg)
                .ThenInclude(l => l.RouteInfo)
                .ThenInclude(r => r.To)
            .Include(u => u.Reservations)
                .ThenInclude(r => r.ReservationLegs)
                .ThenInclude(rl => rl.Leg)
                .ThenInclude(l => l.Providers)
                .ThenInclude(p => p.Company)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user?.Reservations.ToList() ?? [];
    }
}
