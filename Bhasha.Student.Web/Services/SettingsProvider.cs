using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.Extensions.Configuration;

namespace Bhasha.Student.Web.Services
{
    public interface ISettingsProvider
    {
        Task<AppSettings> GetSettings();
    }

    public class SettingsProvider : ISettingsProvider
    {
        private readonly string _settingsPath;
        private readonly HttpClient _httpClient;
        private readonly IAppCache _cache;
        
        public SettingsProvider(IConfiguration configuration, HttpClient httpClient, IAppCache cache)
        {
            _cache = cache;
            _settingsPath = configuration.GetValue<string>("Settings");
            _httpClient = httpClient;
        }

        private async Task<AppSettings> FetchSettings()
        {
            var response = await _httpClient.GetAsync(_settingsPath);
            response.EnsureSuccessStatusCode();
            var message = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AppSettings>(message);
        }

        public async Task<AppSettings> GetSettings()
        {
            return await _cache.GetOrAddAsync(nameof(AppSettings), FetchSettings);
        }
    }
}
