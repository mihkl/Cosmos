using Newtonsoft.Json;
using Shared;

namespace API.Services
{
    public class PriceListService
    {
        private const string API_URL = "https://cosmosodyssey.azurewebsites.net/api/v1.0/TravelPrices";
        public static async Task<PriceList> FetchNewPriceListFromApiAsync()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(API_URL);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var priceList = JsonConvert.DeserializeObject<PriceList>(json);

            return priceList ?? throw new Exception("Failed to deserialize PriceList from API response.");
        }
    }
}