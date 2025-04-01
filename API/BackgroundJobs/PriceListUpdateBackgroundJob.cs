using API.Data.Repos;
using API.Services;
using Quartz;

namespace API.BackgroundJobs;

[DisallowConcurrentExecution]
public class PriceListUpdateBackgroundJob(PriceListRepo priceListRepo, ILogger<PriceListUpdateBackgroundJob> logger) : IJob
{
    private readonly PriceListRepo _priceListRepo = priceListRepo;
    private readonly ILogger<PriceListUpdateBackgroundJob> _logger = logger;
    public Task Execute(IJobExecutionContext context)
    {
        if (!_priceListRepo.IsActivePriceListValidAsync().Result)
        {
            _logger.LogInformation("Price list is not valid, fetching new price list from API.");
            var newPriceList = PriceListFetchService.FetchNewPriceListFromApiAsync().Result;

            if (newPriceList is not null)
            {
                _logger.LogInformation("New price list fetched successfully, adding to database.");
                _priceListRepo.AddAsync(newPriceList).Wait();
            }
            else
            {
                _logger.LogError("Failed to fetch new price list from API.");
            }
        }
        else
        {
            _logger.LogInformation("Price list is valid, no need to fetch new price list.");
        }

        return Task.CompletedTask;
    }
}