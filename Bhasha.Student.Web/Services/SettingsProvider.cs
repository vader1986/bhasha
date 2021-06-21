using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
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
        private AppSettings _settings;

        public SettingsProvider(IConfiguration configuration, HttpClient httpClient)
        {
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
            return _settings ??= await FetchSettings();
        }
    }
}
