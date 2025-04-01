using API.Services;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace API.Data.Repos;

public class PriceListRepo(DataContext dataContext)
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<PriceList> GetActivePriceListAsync()
    {
        var mostRecentPriceList = await _dataContext.PriceList
            .Include(p => p.Legs)
                .ThenInclude(l => l.Providers)
                .ThenInclude(p => p.Company)
            .Include(p => p.Legs)
                .ThenInclude(l => l.RouteInfo)
                .ThenInclude(r => r.From)
            .Include(p => p.Legs)
                .ThenInclude(l => l.RouteInfo)
                .ThenInclude(r => r.To)
            .OrderByDescending(p => p.ValidUntil)
            .FirstOrDefaultAsync() ?? null;

        if (mostRecentPriceList is null || mostRecentPriceList.ValidUntil < DateTime.UtcNow)
        {
            mostRecentPriceList = await PriceListFetchService.FetchNewPriceListFromApiAsync();
        }
        return mostRecentPriceList;
    }

    public async Task AddAsync(PriceList priceList)
    {
        if (await _dataContext.PriceList.CountAsync() >= 15)
        {
            var priceListToRemove = await _dataContext.PriceList.OrderBy(p => p.ValidUntil).FirstOrDefaultAsync();
            if (priceListToRemove is not null)
            {
                _dataContext.PriceList.Remove(priceListToRemove);
                await _dataContext.SaveChangesAsync();
            }
        }

        _dataContext.ChangeTracker.Clear();

        var companyCache = new Dictionary<Guid, Company>();

        foreach (var leg in priceList.Legs)
        {
            var existingLeg = await _dataContext.Legs
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == leg.Id);

            if (existingLeg != null)
            {
                leg.Id = existingLeg.Id;
            }

            foreach (var provider in leg.Providers)
            {
                Guid companyId = provider.Company.Id;

                if (companyCache.TryGetValue(companyId, out Company cachedCompany))
                {
                    provider.Company = cachedCompany;
                }
                else
                {
                    var existingCompany = await _dataContext.Companies
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Id == companyId);

                    if (existingCompany != null)
                    {
                        provider.Company.Id = existingCompany.Id;
                        companyCache[companyId] = provider.Company;
                    }
                    else
                    {
                        companyCache[companyId] = provider.Company;
                    }
                }
            }
        }
        _dataContext.PriceList.Add(priceList);

        await _dataContext.SaveChangesAsync();
    }

    public async Task<bool> IsActivePriceListValidAsync()
    {
        var activePriceList = await _dataContext.PriceList
            .OrderByDescending(p => p.ValidUntil)
            .FirstOrDefaultAsync() ?? null;
        if (activePriceList is null)
        {
            return false;
        }
        return activePriceList.ValidUntil > DateTime.UtcNow;
    }
}
