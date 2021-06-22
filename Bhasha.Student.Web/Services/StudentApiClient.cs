using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Extensions;

namespace Bhasha.Student.Web.Services
{
    public interface IStudentApiClient
    {
        Task<ChapterEnvelope[]> ListChapters(Guid profileId, int level = int.MaxValue);
        Task<Stats> GetStats(Guid profileId, Guid chapterId);
        Task<Evaluation> SubmitPage(Guid profileId, Guid chapterId, int pageIndex, string solution);
        Task<string> RequestTip(Guid profileId, Guid chapterId, int pageIndex);
        Task<Profile> CreateProfile(string native, string target);
        Task<Profile[]> ListProfiles();
        Task<Language[]> GetLanguages();
        Task DeleteProfile(Guid profileId);
        Task DeleteUser();
    }

    public class StudentApiClient : IStudentApiClient
    {
        private readonly ISettingsProvider _settingsProvider;

        public StudentApiClient(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        private async Task<HttpClient> GetHttpClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri((await _settingsProvider.GetSettings()).StudentApi),
                Timeout = TimeSpan.FromSeconds(5)
            };
        }

        private async Task<T> Post<T>(string url, HttpContent content = default)
        {
            using var client = await GetHttpClient();
            return await client.PostAsync<T>(url, content);
        }

        private async Task<T> Get<T>(string url)
        {
            using var client = await GetHttpClient();
            return await client.GetAsync<T>(url);
        }

        private async Task Delete(string url)
        {
            using var client = await GetHttpClient();
            await client.DeleteAsync(url);
        }

        public Task<ChapterEnvelope[]> ListChapters(Guid profileId, int level = int.MaxValue)
        {
            return Get<ChapterEnvelope[]>($"api/chapter/list?profileId={profileId}&level={level}");
        }

        public Task<Stats> GetStats(Guid profileId, Guid chapterId)
        {
            return Get<Stats>($"api/chapter/stats?profileId={profileId}&chapterId={chapterId}");
        }

        public Task<Evaluation> SubmitPage(Guid profileId, Guid chapterId, int pageIndex, string solution)
        {
            return Post<Evaluation>($"api/page/submit?profileId={profileId}&chapterId={chapterId}&pageIndex={pageIndex}&solution={solution}");
        }

        public Task<string> RequestTip(Guid profileId, Guid chapterId, int pageIndex)
        {
            return Post<string>($"api/page/tip?profileId={profileId}&chapterId={chapterId}&pageIndex={pageIndex}");
        }

        public Task<Profile> CreateProfile(string native, string target)
        {
            return Post<Profile>($"api/profile/create?native={native}&target={target}");
        }

        public Task<Profile[]> ListProfiles()
        {
            return Get<Profile[]>("api/profile/list");
        }

        public Task<Language[]> GetLanguages()
        {
            return Get<Language[]>("api/profile/languages");
        }

        public Task DeleteProfile(Guid profileId)
        {
            return Delete("api/profile/delete");
        }

        public Task DeleteUser()
        {
            return Delete("api/user/delete");
        }
    }
}
