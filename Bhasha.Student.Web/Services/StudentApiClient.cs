using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bhasha.Common;

namespace Bhasha.Student.Web.Services
{
    public interface IStudentApiClient
    {
        Task<Profile> CreateProfile(string native, string target);
        Task<Language[]> GetLanguages();
    }

    public class StudentApiClient : IStudentApiClient
    {
        private readonly ISettingsProvider _settingsProvider;
        private HttpClient _client;

        public StudentApiClient(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        private async Task<HttpClient> GetHttpClient()
        {
            var uri = await _settingsProvider.GetSettings();
            return _client ??= new HttpClient
            {
                BaseAddress = new Uri(uri.StudentApi),
                Timeout = TimeSpan.FromSeconds(5),                
            };
        }

        public async Task<Profile> CreateProfile(string native, string target)
        {
            var url = $"api/profile/create?native={native}&target={target}";

            var client = await GetHttpClient();
            var response = await client.PostAsync(url, default);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<Profile>();
        }

        public async Task<Language[]> GetLanguages()
        {
            var url = "api/profile/languages";

            var client = await GetHttpClient();
            Console.WriteLine("GetAsync: " + client.BaseAddress.ToString() + " " + url);

            try
            {
                var response = await client.GetAsync(url);

                Console.WriteLine("GetAsync DONE");

                response.EnsureSuccessStatusCode();

                Console.WriteLine("OK");

                return await response.Content.ReadAsAsync<Language[]>();
            }
            catch (Exception e)
            {
                Console.WriteLine("error: " + e.Message);
                Console.WriteLine("error: " + e.StackTrace);
                throw;
            }
        }
    }
}
