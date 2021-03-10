using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bhasha.Common
{
    public interface IDatabase
    {
        ValueTask<User> CreateUser(User user);
        ValueTask<User> GetUser(Guid userId);
        ValueTask UpdateUser(User user);
        ValueTask<int> DeleteUser(Guid userId);

        ValueTask<IEnumerable<Profile>> GetProfiles(Guid userId);
        ValueTask<Profile> CreateProfile(Profile profile);
        ValueTask UpdateProfile(Guid profileId, int level);
        ValueTask<int> DeleteProfile(Guid profileId);
        ValueTask<int> DeleteProfiles(Guid userId);

        ValueTask<IEnumerable<Chapter>> GetChapters(int level);
        ValueTask<Chapter> CreateChapter(Chapter chapter);
        ValueTask UpdateChapter(Chapter chapter);
        ValueTask<int> DeleteChapter(Guid chapterId);

        ValueTask<IEnumerable<Tip>> GetTips(Guid chapterId, int pageIndex);
        ValueTask<Tip> CreateTip(Tip tip);
        ValueTask UpdateTip(Tip tip);
        ValueTask<int> DeleteTip(Guid tipId);
        ValueTask<int> DeleteTips(Guid chapterId, int pageIndex);

        ValueTask<ChapterStats> CreateChapterStats(ChapterStats chapterStats);
        ValueTask<ChapterStats> GetChapterStats(Guid profileId, Guid chapterId);
        ValueTask UpdateChapterStats(ChapterStats chapterStats);
        ValueTask<int> DeleteChapterStats(Guid profileId);
    }
}
