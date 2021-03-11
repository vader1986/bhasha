using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bhasha.Common
{
    public interface IDatabase
    {
        Task<User> CreateUser(User user);
        Task<User> GetUser(Guid userId);
        Task UpdateUser(User user);
        Task<int> DeleteUser(Guid userId);

        Task<IEnumerable<Profile>> GetProfiles(Guid userId);
        Task<Profile> CreateProfile(Profile profile);
        Task<Profile> GetProfile(Guid profileId);
        Task UpdateProfile(Guid profileId, int level);
        Task<int> DeleteProfile(Guid profileId);
        Task<int> DeleteProfiles(Guid userId);

        Task<IEnumerable<Chapter>> GetChapters(int level);
        Task<Chapter> CreateChapter(Chapter chapter);
        Task UpdateChapter(Chapter chapter);
        Task<int> DeleteChapter(Guid chapterId);

        Task<IEnumerable<Tip>> GetTips(Guid chapterId, int pageIndex);
        Task<Tip> CreateTip(Tip tip);
        Task UpdateTip(Tip tip);
        Task<int> DeleteTip(Guid tipId);
        Task<int> DeleteTips(Guid chapterId, int pageIndex);

        Task<ChapterStats> CreateChapterStats(ChapterStats chapterStats);
        Task<ChapterStats> GetChapterStats(Guid profileId, Guid chapterId);
        Task UpdateChapterStats(ChapterStats chapterStats);
        Task<int> DeleteChapterStatsForProfile(Guid profileId);
        Task<int> DeleteChapterStatsForChapter(Guid chapterId);
    }
}
